using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FileStorage.Domain.Services.AuthenticationServices;
using FileStorage.Domain.Services.StorageItemServices;

namespace FileStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageItemsController : ControllerBase
    {
        private readonly IStorageItemsService storageItemsService;
        private readonly IAuthService authService;

        public StorageItemsController(IStorageItemsService storageItemsService, IAuthService authService)
        {
            this.storageItemsService = storageItemsService;
            this.authService = authService;
        }

        [Authorize(Policy = "MemberRequired")]
        [HttpPost("files"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadAsync()
        {
            var userRequested = await authService.GetRequestedUser(HttpContext.User);

            var files = Request.Form.Files;

            foreach (var file in files)
            {
                await storageItemsService.CreateFileAsync(userRequested, file);
            }

            return StatusCode(201);
        }

        [Authorize(Policy = "MemberRequired")]
        [HttpPost("files/{fileId}"), DisableRequestSizeLimit]
        public async Task<IActionResult> Download(string fileId)
        {
            var userRequested = await authService.GetRequestedUser(HttpContext.User);

            var result = await storageItemsService.DownloadFileAsync(userRequested, fileId);

            return File(result.stream, result.contentType, result.fileName);
        }
    }
}