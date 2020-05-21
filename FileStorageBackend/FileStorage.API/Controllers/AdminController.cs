using System.Threading.Tasks;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Domain.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService userService;

        public AdminController(IUserService userService)
        {
            this.userService = userService;
        }

        [Authorize(Policy = "AdminRoleRequired")]
        [HttpGet("usersWithRoles")]
        public async Task<IActionResult> GetUsersWithRolesAsync()
        {
            var usersWithRoles = await userService.GetUserWithRolesAsync();

            return Ok(usersWithRoles);
        }

        [Authorize(Policy = "AdminRoleRequired")]
        [HttpPost("editRoles/{userName}")]
        public async Task<IActionResult> EditRolesAsync(string userName, RoleEditDto roleEditDto)
        {
            var roles = await userService.ChangeUserRolesAsync(userName, roleEditDto);

            if (roles == null)
                return BadRequest("Failed to add or remove role for user");
                
            return Ok(roles);
        }
    }
}