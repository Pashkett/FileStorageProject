using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FileStorage.Domain.Services.AuthenticationServices;
using FileStorage.Domain.Services.ActualItemsServices;
using FileStorage.Domain.Exceptions;
using FileStorage.API.Filters;
using FileStorage.Domain.DataTransferredObjects.UserModels;

namespace FileStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActualItemsController : ControllerBase
    {
        private readonly IActualItemsService actualItemsService;
        private readonly IAuthService authService;

        public ActualItemsController(IActualItemsService actualItemsService, IAuthService authService)
        {
            this.actualItemsService = actualItemsService;
            this.authService = authService;
        }
        
        [Authorize(Policy = "MemberRoleRequired")]
        [ServiceFilter(typeof(CheckUserFromRequestFilterAsync))]
        [HttpGet("files")]
        public async Task<IActionResult> GetAllActualFilesForUser()
        {
            //var userRequested = await authService.GetRequestedUser(HttpContext.User);
            //if (userRequested == null)
            //    return Unauthorized();
            var userRequested = (UserDto)HttpContext.Items["UserRequested"];

            var files = await actualItemsService.GetActualFilesByUserAsync(userRequested);

            if (files == null || files.Count() == 0)
                return NoContent();

            return Ok(files);
        }

        [Authorize(Policy = "MemberRoleRequired")]
        [ServiceFilter(typeof(CheckUserFromRequestFilterAsync))]
        [HttpPost("files"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFilesAsync()
        {
            var userRequested = (UserDto)HttpContext.Items["UserRequested"];

            var files = Request.Form.Files;

            foreach (var file in files)
            {
                await actualItemsService.CreateFileAsync(userRequested, file);
            }

            return StatusCode(201);
        }

        [Authorize(Policy = "MemberRoleRequired")]
        [ServiceFilter(typeof(CheckUserFromRequestFilterAsync))]
        [HttpPost("files/{fileId}"), DisableRequestSizeLimit]
        public async Task<IActionResult> DownloadFilesAsync(string fileId)
        {
            var userRequested = (UserDto)HttpContext.Items["UserRequested"];

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

        [Authorize(Policy = "MemberRoleRequired")]
        [ServiceFilter(typeof(CheckUserFromRequestFilterAsync))]
        [HttpDelete("files/{fileId}")]
        public async Task<IActionResult> MoveToRecycleBinAsync(string fileId)
        {
            var userRequested = (UserDto)HttpContext.Items["UserRequested"];

            try
            {
                await actualItemsService.MoveFileToRecycledBinAsync(userRequested, fileId);
                return Ok();
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}