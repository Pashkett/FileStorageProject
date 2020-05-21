using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Domain.Services.Authentication;

namespace FileStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserForRegisterDto userForRegister)
        {
            var user = await authService.RegisterAsync(userForRegister);

            if (user == null)
                return BadRequest("UserName already exists");

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLogin)
        {
            var userFromService = 
                await authService.LoginAsync(userForLogin.Username.ToLower(), 
                                              userForLogin.Password);

            if (userFromService == null)
                return Unauthorized();

            return Ok(new
            {
                token = await authService.GenerateJwtTokenAsync(userFromService),
                userFromService
            });
        }
    }
}