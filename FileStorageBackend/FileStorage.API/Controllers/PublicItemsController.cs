using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using FileStorage.API.Filters;
using FileStorage.API.Extensions;
using FileStorage.Domain.Services.PublicItems;
using FileStorage.Domain.Exceptions;
using FileStorage.Domain.RequestModels;

namespace FileStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicItemsController : ControllerBase
    {
        private readonly IPublicItemsService publicItemsService;
        private readonly string userParamName;

        public PublicItemsController(IPublicItemsService publicItemsService,
                                     IConfiguration configuration)
        {
            this.publicItemsService = publicItemsService;
            userParamName = configuration.GetValue<string>("UserKeyParameter");
        }

        [Authorize(Policy = "AllRegisteredUsers")]
        [HttpGet("files")]
        public async Task<IActionResult> GetAllPublicFilesAsync(
            [FromQuery]StorageItemsRequestParameters filesParams)
        {
            if (!filesParams.IsValidSizeRange)
                return BadRequest("Max size can't be less than min size.");

            var (files, header) =
                await publicItemsService.GetPublicFilesAndHeaderAsync(filesParams);

            if (files == null || files.Count() == 0)
                return NoContent();

            Response.AddPagination(header);

            return Ok(files);
        }

        [Authorize(Policy = "MemberRoleRequired")]
        [ServiceFilter(typeof(UserCheckerFromRequest))]
        [HttpPost("files/{fileId}")]
        public async Task<IActionResult> SetPrivateFile(string fileId)
        {
            var userRequested = HttpContext.GetUserFromContext(userParamName);

            try
            {
                await publicItemsService.MoveFilePrivateAsync(userRequested, fileId);

                return Ok(new { Id = fileId });
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "AllRegisteredUsers")]
        [HttpGet("files/{fileId}"), DisableRequestSizeLimit]
        public async Task<IActionResult> DownloadFilesAsync(string fileId)
        {
            try
            {
                var result = await publicItemsService.DownloadFileAsync(fileId);

                return File(result.stream, result.contentType, result.fileName);
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}