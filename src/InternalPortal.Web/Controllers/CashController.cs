using InternalPortal.Core.Interfaces;
using InternalPortal.Web.Constants;
using InternalPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Net.Mime;

namespace InternalPortal.Web.Controllers
{
    public class CashController : Controller
    {
        private readonly ICashTestService _cashTestService;
        private readonly IFileProvider _fileProvider;
        private readonly IUploadFileService _uploadFileService;

        public CashController(
            ICashTestService cashTestService,
            IFileProvider fileProvider,
            IUploadFileService uploadFileService
            )
        {
            _cashTestService = cashTestService ?? throw new ArgumentNullException(nameof(cashTestService));
            _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            _uploadFileService = uploadFileService ?? throw new ArgumentNullException(nameof(uploadFileService));
        }

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
            List<UploadFileViewModel> uploadedFiles = [];
            var physicalFiles = _fileProvider.GetDirectoryContents(string.Empty);
            foreach (var physicalFile in physicalFiles)
            {
                var getFileModel = await _uploadFileService.GetFileByGuidAsync(physicalFile.Name);
                if (getFileModel != null) {
                    var ext = Path.GetExtension(getFileModel.UntrastedName).ToLowerInvariant();
                    uploadedFiles.Add(new UploadFileViewModel
                    {
                        Id = getFileModel.Id,
                        TrustedName = getFileModel.TrustedName,
                        UntrastedName = getFileModel.UntrastedName,
                        Extension = ext
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

                _uploadFileService.DeleteAsync(getFileModel.Id);
            }

            return RedirectToAction("Education");
        }
    }
}
