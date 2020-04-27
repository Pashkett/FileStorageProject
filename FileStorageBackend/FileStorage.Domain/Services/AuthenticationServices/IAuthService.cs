using System.Threading.Tasks;
using FileStorage.Domain.DataTransferredObjects.UserModels;


namespace FileStorage.Domain.Services.AuthenticationServices
{
    public interface IAuthService
    {
        Task<UserForRegisterDto> RegisterAsync(UserForRegisterDto user);
        Task<UserDto> LoginAsync(string username, string password);
    }
}
