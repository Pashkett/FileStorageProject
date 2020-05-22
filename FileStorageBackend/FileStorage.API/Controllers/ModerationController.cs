using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FileStorage.API.Extensions;
using FileStorage.Domain.Exceptions;
using FileStorage.Domain.RequestModels;
using FileStorage.Domain.Services.ItemsModeration;


namespace FileStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModerationController : ControllerBase
    {
        private readonly IItemsModerationService moderationService;

        public ModerationController(IItemsModerationService moderationService)
        {
            this.moderationService = moderationService;
        }

        [Authorize(Policy = "ModeratorRoleRequired")]
        [HttpGet("files")]
        public async Task<IActionResult> GetFilesForModeration(
            [FromQuery]StorageItemsRequestParameters filesParams)
        {
            if (!filesParams.IsValidSizeRange)
                return BadRequest("Max size can't be less than min size.");

            var (files, header) =
                await moderationService.GetFilesForModerationAndHeaderAsync(filesParams);

            if (files == null || files.Count() == 0)
                return NoContent();

            Response.AddPagination(header);

            return Ok(files);
        }

        [Authorize(Policy = "ModeratorRoleRequired")]
        [HttpPost("files/public/{fileId}")]
        public async Task<IActionResult> MoveToPublicAsync(string fileId)
        {
            try
            {
                await moderationService.MoveFilePublicAsync(fileId);

                return Ok(new { id = fileId });
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "ModeratorRoleRequired")]
        [HttpPost("files/private/{fileId}")]
        public async Task<IActionResult> MoveToPrivateAsync(string fileId)
        {
            try
            {
                await moderationService.MoveFilePrivateAsync(fileId);

                return Ok(new { id = fileId });
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "ModeratorRoleRequired")]
        [HttpPost("files/recycle/{fileId}")]
        public async Task<IActionResult> MoveFileToRecycledAsync(string fileId)
        {
            try
            {
                await moderationService.MoveFileRecycleBinAsync(fileId);

                return Ok(new { id = fileId });
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "ModeratorRoleRequired")]
        [HttpPost("files/restore/{fileId}")]
        public async Task<IActionResult> RestoreFileAsync(string fileId)
        {
            try
            {
                await moderationService.RestoreRecycledFileAsync(fileId);

                return Ok(new { id = fileId });
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "ModeratorRoleRequired")]
        [HttpDelete("files/{fileId}")]
        public async Task<IActionResult> DeleteFileAsync(string fileId)
        {
            try
            {
                await moderationService.DeleteFileAsync(fileId);

                return Ok(new { id = fileId });
            }
            catch (StorageItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}