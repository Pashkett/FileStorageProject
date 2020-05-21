using System.Collections.Generic;
using System.Threading.Tasks;
using FileStorage.Domain.DataTransferredObjects.UserModels;

namespace FileStorage.Domain.Services.Users
{
    public interface IUserService
    {
        Task<IEnumerable<UserForDisplayRoles>> GetUserWithRolesAsync();
        Task<List<string>> ChangeUserRolesAsync(string userName, RoleEditDto roleEditDto);
        Task<UserDto> GetUserByIdAsync(string id);
    }
}