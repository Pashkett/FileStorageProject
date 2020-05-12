using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FileStorage.Domain.Services.PublicItemsServices;
using FileStorage.API.Filters;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using Microsoft.Extensions.Configuration;
using FileStorage.Domain.Exceptions;

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

        [HttpGet("files")]
        public async Task<IActionResult> GetAllPublicFilesAsync()
        {
            var publicFiles = await publicItemsService.GetPublicFilesAsync();

            if (publicFiles == null || publicFiles.Count() == 0)
                return NoContent();

            return Ok(publicFiles);
        }

        [HttpPost("files/{fileId}")]
        [ServiceFilter(typeof(UserCheckerFromRequest))]
        public async Task<IActionResult> SetPrivateFile(string fileId)
        {
            var userRequested = (UserDto)HttpContext.Items[userParamName];

            try
            {
                await publicItemsService.MoveFilePrivateAsync(userRequested, fileId);

                return Ok(
                    new { Id = fileId });
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}