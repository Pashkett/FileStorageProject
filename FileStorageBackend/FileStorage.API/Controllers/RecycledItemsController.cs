using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FileStorage.API.Filters;
using FileStorage.API.Extensions;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Domain.Exceptions;
using FileStorage.Domain.Services.RecycledItemsServices;
using FileStorage.Domain.RequestModels;

namespace FileStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecycledItemsController : ControllerBase
    {
        private readonly IRecycledItemsService recycledItemsService;
        private readonly string userParamName;

        public RecycledItemsController(IRecycledItemsService recycledItemsService,
                                       IConfiguration configuration)
        {
            this.recycledItemsService = recycledItemsService;
            userParamName = configuration.GetValue<string>("UserKeyParameter");
        }

        [Authorize(Policy = "AllRegisteredUsers")]
        [ServiceFilter(typeof(UserCheckerFromRequest))]
        [HttpGet("files")]
        public async Task<IActionResult> GetAllRecycledFilesForUser(
            [FromQuery]StorageItemsRequestParameters filesParams)
        {
            var userRequested = (UserDto)HttpContext.Items[userParamName];

            var paginResults =
                await recycledItemsService.GetRecycledItemsByUserPagedAsync(userRequested, filesParams);

            if (paginResults.pagedList == null || paginResults.pagedList.Count() == 0)
                return NoContent();

            Response.AddPagination(paginResults.paginationHeader);

            return Ok(paginResults.pagedList);
        }

        [Authorize(Policy = "AllRegisteredUsers")]
        [ServiceFilter(typeof(UserCheckerFromRequest))]
        [HttpPost("files/{fileId}")]
        public async Task<IActionResult> RestoreRecycledFile(string fileId)
        {
            var userRequested = (UserDto)HttpContext.Items[userParamName];

            try
            {
                await recycledItemsService.RestoreRecycledItemAsync(userRequested, fileId);

                return Ok(new
                {
                    id = fileId
                });
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "AllRegisteredUsers")]
        [ServiceFilter(typeof(UserCheckerFromRequest))]
        [HttpDelete("files/{fileId}")]
        public async Task<IActionResult> DeleteRecycledFile(string fileId)
        {
            var userRequested = (UserDto)HttpContext.Items[userParamName];

            try
            {
                await recycledItemsService.DeleteRecycledItemAsync(userRequested, fileId);

                return Ok(new
                {
                    id = fileId
                });
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}