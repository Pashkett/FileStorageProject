using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileStorage.Domain.Services.PublicItemsServices;
using FileStorage.API.Filters;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using Microsoft.Extensions.Configuration;
using FileStorage.Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using FileStorage.Domain.PagingHelpers;

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
            var pagingResults = await publicItemsService.GetPublicFilesPagedAsync(filesParams);

            if (pagingResults.pagedList == null || pagingResults.pagedList.Count() == 0)
                return NoContent();

            Response.AddPagination(pagingResults.paginationHeader);

            return Ok(pagingResults.pagedList);
        }

        [Authorize(Policy = "MemberRoleRequired")]
        [ServiceFilter(typeof(UserCheckerFromRequest))]
        [HttpPost("files/{fileId}")]
        public async Task<IActionResult> SetPrivateFile(string fileId)
        {
            var userRequested = (UserDto)HttpContext.Items[userParamName];

            try
            {
                await publicItemsService.MoveFilePrivateAsync(userRequested, fileId);

                return Ok(new
                {
                    Id = fileId
                });
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