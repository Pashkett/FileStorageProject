using System.Linq;
using System.Threading.Tasks;
using FileStorage.Domain.Exceptions;
using FileStorage.Domain.Services.AuthenticationServices;
using FileStorage.Domain.Services.RecycledItemsServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecycledItemsController : ControllerBase
    {
        private readonly IRecycledItemsService recycledItemsService;
        private readonly IAuthService authService;

        public RecycledItemsController(IRecycledItemsService recycledItemsService, IAuthService authService)
        {
            this.recycledItemsService = recycledItemsService;
            this.authService = authService;
        }

        [Authorize(Policy = "MemberRoleRequired")]
        [HttpGet("files")]
        public async Task<IActionResult> GetAllRecycledFileForUser()
        {
            var userRequested = await authService.GetRequestedUser(HttpContext.User);
            if (userRequested == null)
                return Unauthorized();

            var recycledFiles = await recycledItemsService.GetRecycledItemsByUserAsync(userRequested);

            if (recycledFiles == null || recycledFiles.Count() == 0)
                return NoContent();

            return Ok(recycledFiles);
        }


        [Authorize(Policy = "MemberRoleRequired")]
        [HttpPost("files/{fileId}")]
        public async Task<IActionResult> RestoreRecycledFile(string fileId)
        {
            var userRequested = await authService.GetRequestedUser(HttpContext.User);
            if (userRequested == null)
                return Unauthorized();

            try
            {
                await recycledItemsService.RestoreRecycledItemAsync(userRequested, fileId);
                return Ok();
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "MemberRoleRequired")]
        [HttpDelete("files/{fileId}")]
        public async Task<IActionResult> DeleteRecycledFile(string fileId)
        {
            var userRequested = await authService.GetRequestedUser(HttpContext.User);
            if (userRequested == null)
                return Unauthorized();

            try
            {
                await recycledItemsService.DeleteRecycledItemAsync(userRequested, fileId);
                return Ok();
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}