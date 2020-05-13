using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FileStorage.Domain.Services.ActualItemsServices;
using FileStorage.Domain.Exceptions;
using FileStorage.API.Filters;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using System.Collections.Generic;

namespace FileStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActualItemsController : ControllerBase
    {
        private readonly IActualItemsService actualItemsService;
        private readonly string userParamName;

        public ActualItemsController(IActualItemsService actualItemsService, 
                                     IConfiguration configuration)
                                     
        {
            this.actualItemsService = actualItemsService;
            userParamName = configuration.GetValue<string>("UserKeyParameter");
        }
        
        
        [Authorize(Policy = "AllRegisteredUsers")]
        [ServiceFilter(typeof(UserCheckerFromRequest))]
        [HttpGet("files")]
        public async Task<IActionResult> GetAllActualFilesForUser()
        {
            var userRequested = GetUserFromContext(userParamName);

            var files = await actualItemsService.GetActualFilesByUserAsync(userRequested);

            if (files == null || files.Count() == 0)
                return NoContent();

            return Ok(files);
        }

        [Authorize(Policy = "AllRegisteredUsers")]
        [ServiceFilter(typeof(UserCheckerFromRequest))]
        [HttpPost("files"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFilesAsync()
        {
            var userRequested = GetUserFromContext(userParamName);

            var files = Request.Form.Files;
            List<FileItemDto> filesDto = new List<FileItemDto>();

            foreach (var file in files)
            {
                filesDto.Add(await actualItemsService.CreateFileAsync(userRequested, file));
            }

            return Ok(filesDto);
        }

        [Authorize(Policy = "AllRegisteredUsers")]
        [ServiceFilter(typeof(UserCheckerFromRequest))]
        [HttpGet("files/{fileId}"), DisableRequestSizeLimit]
        public async Task<IActionResult> DownloadFilesAsync(string fileId)
        {
            var userRequested = GetUserFromContext(userParamName);

            try
            {
                var result = await actualItemsService.DownloadFileAsync(userRequested, fileId);

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
            var userRequested = GetUserFromContext(userParamName);

            try
            {
                await actualItemsService.MoveFileRecycledBinAsync(userRequested, fileId);

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
            var userRequested = GetUserFromContext(userParamName);

            try
            {
                await actualItemsService.MoveFilePublicAsync(userRequested, fileId);

                return Ok(
                    new { id = fileId }
                    ) ;
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        private UserDto GetUserFromContext(string userParamKey)
        {
            return (UserDto)HttpContext.Items[userParamKey];
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
                var result = await actualItemsService.DownloadFileAsync(fileId);
                return File(result.stream, result.contentType, result.fileName);
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}