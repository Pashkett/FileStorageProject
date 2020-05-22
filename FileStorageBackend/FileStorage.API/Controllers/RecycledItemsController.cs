using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FileStorage.API.Filters;
using FileStorage.API.Extensions;
using FileStorage.Domain.Exceptions;
using FileStorage.Domain.Services.RecycledItems;
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
            if (!filesParams.IsValidSizeRange)
                return BadRequest("Max size can't be less than min size.");

            var userRequested = HttpContext.GetUserFromContext(userParamName);

            var (files, header) =
                await recycledItemsService.GetRecycledItemsAndHeaderAsync(userRequested, filesParams);

            if (files == null || files.Count() == 0)
                return NoContent();

            Response.AddPagination(header);

            return Ok(files);
        }

        [Authorize(Policy = "AllRegisteredUsers")]
        [ServiceFilter(typeof(UserCheckerFromRequest))]
        [HttpPost("files/{fileId}")]
        public async Task<IActionResult> RestoreRecycledFile(string fileId)
        {
            var userRequested = HttpContext.GetUserFromContext(userParamName);

            try
            {
                await recycledItemsService.RestoreRecycledItemAsync(userRequested, fileId);

                return Ok(new { id = fileId });
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
            var userRequested = HttpContext.GetUserFromContext(userParamName);

            try
            {
                await recycledItemsService.DeleteRecycledItemAsync(userRequested, fileId);

                return Ok(new { id = fileId });
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}