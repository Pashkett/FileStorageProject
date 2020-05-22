using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FileStorage.API.Filters;
using FileStorage.API.Extensions;
using FileStorage.Domain.Services.PrivateItems;
using FileStorage.Domain.Exceptions;
using FileStorage.Domain.RequestModels;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;

namespace FileStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrivateItemsController : ControllerBase
    {
        private readonly IPrivateItemsService privateItemsService;
        private readonly string userParamName;

        public PrivateItemsController(IPrivateItemsService privateItemsService,
                                     IConfiguration configuration)

        {
            this.privateItemsService = privateItemsService;
            userParamName = configuration.GetValue<string>("UserKeyParameter");
        }

        [Authorize(Policy = "AllRegisteredUsers")]
        [ServiceFilter(typeof(UserCheckerFromRequest))]
        [HttpGet("files")]
        public async Task<IActionResult> GetPrivateFiles(
            [FromQuery]StorageItemsRequestParameters filesParams)
        {
            if (!filesParams.IsValidSizeRange)
                return BadRequest("Max size can't be less than min size.");

            var userRequested = HttpContext.GetUserFromContext(userParamName);

            var (files, header) =
                await privateItemsService.GetPrivateFilesAndHeaderAsync(userRequested, filesParams);

            if (files == null || files.Count() == 0)
                return NoContent();

            Response.AddPagination(header);

            return Ok(files);
        }

        [Authorize(Policy = "AllRegisteredUsers")]
        [ServiceFilter(typeof(UserCheckerFromRequest))]
        [HttpPost("files"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFilesAsync()
        {
            var userRequested = HttpContext.GetUserFromContext(userParamName);

            var files = Request.Form.Files;
            List<FileItemDto> filesDto = new List<FileItemDto>();

            foreach (var file in files)
            {
                filesDto.Add(await privateItemsService.CreateFileAsync(userRequested, file));
            }

            return Ok(filesDto);
        }

        [Authorize(Policy = "AllRegisteredUsers")]
        [ServiceFilter(typeof(UserCheckerFromRequest))]
        [HttpGet("files/{fileId}"), DisableRequestSizeLimit]
        public async Task<IActionResult> DownloadFilesAsync(string fileId)
        {
            var userRequested = HttpContext.GetUserFromContext(userParamName);

            try
            {
                var result = await privateItemsService.DownloadFileAsync(userRequested, fileId);

                return File(result.stream, result.contentType, result.fileName);
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "AllRegisteredUsers")]
        [ServiceFilter(typeof(UserCheckerFromRequest))]
        [HttpDelete("files/{fileId}")]
        public async Task<IActionResult> MoveToRecycleBinAsync(string fileId)
        {
            var userRequested = HttpContext.GetUserFromContext(userParamName);

            try
            {
                await privateItemsService.MoveFileRecycleBinAsync(userRequested, fileId);

                return Ok();
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "AllRegisteredUsers")]
        [ServiceFilter(typeof(UserCheckerFromRequest))]
        [HttpPost("files/{fileId}")]
        public async Task<IActionResult> MoveToPublicAsync(string fileId)
        {
            var userRequested = HttpContext.GetUserFromContext(userParamName);

            try
            {
                await privateItemsService.MoveFilePublicAsync(userRequested, fileId);

                return Ok(new { id = fileId });
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Use for future short-links controller
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("files/download/{fileId}"), DisableRequestSizeLimit]
        public async Task<IActionResult> DownloadFilesAnonymousAsync(string fileId)
        {
            try
            {
                var result = await privateItemsService.DownloadFileAsync(fileId);
                return File(result.stream, result.contentType, result.fileName);
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}