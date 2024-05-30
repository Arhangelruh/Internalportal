using InternalPortal.Core.Interfaces;
using InternalPortal.Core.Models;
using InternalPortal.Web.Constants;
using InternalPortal.Web.Models;
using InternalPortal.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace InternalPortal.Web.Controllers
{
    public class StreamingController : Controller
    {
        private static readonly FormOptions _defaultFormOptions = new FormOptions();
        private readonly IWebHostEnvironment _environment;
        private readonly string[] _permittedExtensions = [".doc", ".docx", ".zip", ".jpg", ".jpeg", ".png", ".pdf"];
        private readonly ConfigurationFiles _configurationFiles;
        private readonly IUploadFileService _uploadFileService;

        public StreamingController(IWebHostEnvironment environment, IOptions<ConfigurationFiles> configurationFiles, IUploadFileService uploadFileService)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _configurationFiles = configurationFiles.Value ?? throw new ArgumentNullException(nameof(configurationFiles.Value));
            _uploadFileService = uploadFileService ?? throw new ArgumentNullException(nameof(uploadFileService));
        }

        [Authorize(Roles = UserConstants.ManagerRole)]
        [HttpPost]
        [DisableFormValueModelBinding]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPhysical()
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                ModelState.AddModelError("File",
                    $"Ошибка выполнения запроса (Error 1).");

                return BadRequest(ModelState.ToSerializedDictionary());
            }

            if (!Directory.Exists(_configurationFiles.Files))
            {
                ModelState.AddModelError("File",
                    $"Не найдена директория для загрузки файла.");

                return BadRequest(ModelState.ToSerializedDictionary());
            }

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(Request.ContentType),
                _defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);
            var section = await reader.ReadNextSectionAsync();

            while (section != null)
            {
                var hasContentDispositionHeader =
                    ContentDispositionHeaderValue.TryParse(
                        section.ContentDisposition, out var contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (!MultipartRequestHelper
                        .HasFileContentDisposition(contentDisposition))
                    {
                        ModelState.AddModelError("File",
                            $"Ошибка выполнения запроса (Error 2).");

                        return BadRequest(ModelState.ToSerializedDictionary());
                    }
                    else
                    {

                        var trustedFileNameForDisplay = WebUtility.HtmlEncode(
                                contentDisposition.FileName.Value);
                        var trustedFileNameForFileStorage = Guid.NewGuid().ToString();

                        var streamedFileContent = await FileHelpers.ProcessStreamedFile(
                            section, contentDisposition, ModelState,
                            _permittedExtensions, _configurationFiles.FileSizeLimit);

                        if (!ModelState.IsValid)
                        {                           
                            return BadRequest(ModelState.ToSerializedDictionary());
                        }

                        if (trustedFileNameForDisplay == null)
                        {
                            ModelState.AddModelError("File",
                            $"Файл без имени.");
                            return BadRequest(ModelState.ToSerializedDictionary());
                        }

                        using (var targetStream = System.IO.File.Create(
                            Path.Combine(_configurationFiles.Files, trustedFileNameForFileStorage)))
                        {
                            await targetStream.WriteAsync(streamedFileContent);
                        }

                        await _uploadFileService.AddAsync(new UploadFile
                        {
                            TrustedName = trustedFileNameForFileStorage,
                            UntrastedName = trustedFileNameForDisplay
                        });

                    }
                }

                section = await reader.ReadNextSectionAsync();
            }

            return Created(nameof(StreamingController), null);
        }
    }
}
