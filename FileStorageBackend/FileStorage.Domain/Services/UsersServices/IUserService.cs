using System.Collections.Generic;
using System.Threading.Tasks;
using FileStorage.Domain.DataTransferredObjects.UserModels;

namespace FileStorage.Domain.Services.UsersServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserForDisplayRoles>> GetUserWithRolesAsync();
        Task<List<string>> ChangeUserRoles(string userName, RoleEditDto roleEditDto);
    }
}