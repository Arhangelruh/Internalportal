using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using InternalPortal.Web.Constants;
using InternalPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Net.Mime;
using System.Security.Claims;

namespace InternalPortal.Web.Controllers
{
    public class CashController(
		ICashTestService cashTestService,
		IFileProvider fileProvider,
		IUploadFileService uploadFileService,
        IProfileService profileService,
        IReadingReviewService readingReview
			) : Controller
    {
        private readonly ICashTestService _cashTestService = cashTestService ?? throw new ArgumentNullException(nameof(cashTestService));
        private readonly IFileProvider _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        private readonly IUploadFileService _uploadFileService = uploadFileService ?? throw new ArgumentNullException(nameof(uploadFileService));
        private readonly IProfileService _profileService = profileService ?? throw new ArgumentNullException( nameof(profileService));
        private readonly IReadingReviewService _readinReview = readingReview ?? throw new ArgumentNullException(nameof(readingReview));

		public IActionResult Cash()
        {
            return View();
        }

        public async Task<IActionResult> Test()
        {
            List<CashTestViewModel> cashTestModels = [];
            var cashTests = await _cashTestService.GetActiveCashTestsAsync();
            foreach (var cashtest in cashTests)
            {
                cashTestModels.Add(new CashTestViewModel
                {
                    Id = cashtest.Id,
                    CashTestName = cashtest.TestName,
                    IsActual = cashtest.IsActual
                });
            }
            return View(cashTestModels);
        }

        public async Task<IActionResult> Education()
        {
			var profileSID = User.Claims.Where(claim => claim.Type == ClaimTypes.Sid).Select(claim => claim.Value).SingleOrDefault();
            Profile profile = new();

            if(profileSID != null)
               profile = await _profileService.GetProfileByUserSIDAsync(profileSID);


			List<UploadFileViewModel> uploadedFiles = [];
            var physicalFiles = _fileProvider.GetDirectoryContents(string.Empty);
            foreach (var physicalFile in physicalFiles)
            {
                var getFileModel = await _uploadFileService.GetFileByGuidAsync(physicalFile.Name);
                if (getFileModel != null) {
                    var ext = Path.GetExtension(getFileModel.UntrastedName).ToLowerInvariant();

                    var ifRecExist = false;
                    
                    var getRecord = await _readinReview.CheckRecordAsync(getFileModel.Id, profile.Id);

                    if(getRecord != null)
                        ifRecExist = true;

                    uploadedFiles.Add(new UploadFileViewModel
                    {
                        Id = getFileModel.Id,
                        TrustedName = getFileModel.TrustedName,
                        UntrastedName = getFileModel.UntrastedName,
                        Extension = ext,
                        IsAlreadyDone = ifRecExist
                    });
                }
            }

            return View(uploadedFiles);
        }

        public async Task<IActionResult> DownloadPhysical(int fileId)
        {
            var getFileModel = await _uploadFileService.GetFileByIdAsync(fileId);
            var downloadFile = _fileProvider.GetFileInfo(getFileModel.TrustedName);

            return PhysicalFile(downloadFile.PhysicalPath, MediaTypeNames.Application.Octet, getFileModel.UntrastedName);
        }

        [Authorize(Roles = UserConstants.ManagerRole)]
        public async Task<IActionResult> DeletePhysical(int fileId)
        {
            var getFileModel = await _uploadFileService.GetFileByIdAsync(fileId);
            if (getFileModel != null)
            {
                var file = _fileProvider.GetFileInfo(getFileModel.TrustedName);
                System.IO.File.Delete(file.PhysicalPath);

                await _uploadFileService.DeleteAsync(getFileModel.Id);
            }

            return RedirectToAction("Education");
        }
    }
}
