using System.Security.Claims;
using System.Threading.Tasks;
using FileStorage.Domain.DataTransferredObjects.UserModels;


namespace FileStorage.Domain.Services.Authentication
{
    public interface IAuthService
    {
        Task<UserForRegisterDto> RegisterAsync(UserForRegisterDto user);
        Task<UserDto> LoginAsync(string username, string password);
        Task<string> GenerateJwtTokenAsync(UserDto user);
        Task<UserDto> GetRequestedUser(ClaimsPrincipal principal);
    }
}
