using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Domain.Services.UsersServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var usersWithRoles = await userService.GetUserWithRolesAsync();

            return Ok(usersWithRoles);
        }

        [Authorize(Policy = "AdminRoleRequired")]
        [HttpPost("editRoles/{userName}")]
        public async Task<IActionResult> EditRoles(string userName, RoleEditDto roleEditDto)
        {
            var roles = await userService.ChangeUserRolesAsync(userName, roleEditDto);

            if (roles == null)
                return BadRequest("Failed to add or remove role for user");
                
            return Ok(roles);
        }

        [Authorize(Policy = "ModerateFilesRole")]
        [HttpGet("filesForModeration") ]
        public IActionResult GetFilesForModeration()
        {
            return Ok("Admins or moderators can see this");
        }

    }
}