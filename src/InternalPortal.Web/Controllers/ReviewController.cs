using ClosedXML.Excel;
using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using InternalPortal.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InternalPortal.Web.Controllers
{
	public class ReviewController(
		IUploadFileService uploadFileService,
		IProfileService profileService,
		IReadingReviewService readingReviewService) : Controller
	{
		private readonly IUploadFileService _uploadFileService = uploadFileService ?? throw new ArgumentNullException(nameof(uploadFileService));
		private readonly IProfileService _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
		private readonly IReadingReviewService _readingReviewService = readingReviewService ?? throw new ArgumentNullException(nameof(readingReviewService));

		[HttpPost]
		public async Task<IActionResult> ConfirmReview(int fileId)
		{
			var getFile = await _uploadFileService.GetFileByIdAsync(fileId);

			if (getFile != null)
			{
				var profileSID = User.Claims.Where(claim => claim.Type == ClaimTypes.Sid).Select(claim => claim.Value).SingleOrDefault();

				if (profileSID != null)
				{
					var getProfile = await _profileService.GetProfileByUserSIDAsync(profileSID);

					if (getProfile != null)
					{

						var time = DateTime.Now;

						var readingReview = new ReadingReview
						{
							FileId = fileId,
							ProfileId = getProfile.Id,
							ReviewTime = time
						};

						await _readingReviewService.AddRecordAsync(readingReview);
					}
				}
			}
			return RedirectToAction("Education", "Cash");
		}

		[HttpGet]
		public async Task<IActionResult> ReviewUsers(int fileId)
		{
			List<ReviewViewModel> models = [];
			var getFile = await _uploadFileService.GetFileByIdAsync(fileId);
			if (getFile != null)
			{
				ViewData["fileId"] = fileId;
				var confirmModels = await _readingReviewService.GetRecordsByDocumentAsync(fileId);

				if (confirmModels.Count > 0)
				{
					foreach (var confirmModel in confirmModels)
					{
						models.Add(new ReviewViewModel
						{
							UserName = confirmModel.UserName,
							LastName = confirmModel.LastName,
							MiddleName = confirmModel.MiddleName,
							FileName = confirmModel.FileName,
							ConfirmTime = confirmModel.ConfirmTime
						});
					}
				}
			}
			return View(models);
		}

		[HttpGet]
		public async Task<IActionResult> ExportExcel(int fileId)
		{
			var getFile = await _uploadFileService.GetFileByIdAsync(fileId);

			if (getFile != null)
			{
				using var workbook = new XLWorkbook();
				var ws = workbook.Worksheets.Add("Report");

				var confirmModels = await _readingReviewService.GetRecordsByDocumentAsync(fileId);

				if (confirmModels.Count > 0)
				{
					var title = ws.Range("A1:B1").Merge();
					title.Value = $"Лист ознакомления по документу: {getFile.UntrastedName}";


					title.Style.Font.Bold = true;
					title.Style.Font.FontSize = 14;
					title.Style.Alignment.Horizontal =
						XLAlignmentHorizontalValues.Center;
					title.Style.Alignment.Vertical =
						XLAlignmentVerticalValues.Center;

					var usertitle = ws.Cell("A2");
					usertitle.Value = "Пользователь";
					usertitle.Style.Font.Bold = true;
					usertitle.Style.Alignment.Horizontal =
						XLAlignmentHorizontalValues.Center;
					usertitle.Style.Alignment.Vertical =
						XLAlignmentVerticalValues.Center;

					var reviewingtime = ws.Cell("B2");
					reviewingtime.Value = "Время ознакомления";
					reviewingtime.Style.Font.Bold = true;
					reviewingtime.Style.Alignment.Horizontal =
						XLAlignmentHorizontalValues.Center;
					reviewingtime.Style.Alignment.Vertical =
						XLAlignmentVerticalValues.Center;


					int row = 3;

					foreach (var item in confirmModels)
					{
						ws.Cell(row, 1).Value = $"{item.LastName} {item.UserName} {item.MiddleName}";
						ws.Cell(row, 2).Value = $"Ознакомлен {item.ConfirmTime.Date.ToShortDateString()} в {item.ConfirmTime.ToShortTimeString()}";
						row++;
					}
					ws.Columns("A:B").AdjustToContents();
				}

				using var stream = new MemoryStream();

				workbook.SaveAs(stream);

				return File(
					stream.ToArray(),
					"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
					"report.xlsx");
			}
			else
			{
				return RedirectToAction("ReviewUsers", new { fileId });
			}
		}
	}
}
