using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FileStorage.Data.Models;
using FileStorage.Data.UnitOfWork;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Logger;

namespace FileStorage.Domain.Services.UsersServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILoggerManager loggerManager;

        public UserService(UserManager<User> userManager,
                           IUnitOfWork unitOfWork,
                           ILoggerManager loggerManager)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
            this.loggerManager = loggerManager;
        }

        public async Task<IEnumerable<UserForDisplayRoles>> GetUserWithRolesAsync()
        {
            var allUsers = await userManager.Users.ToListAsync();

            //TODO refactor all these
            //var usersWithRoles = new List<UserForDisplayRoles>();
            //var usersWithRoles = from user in allUsers
            //                     select new UserForDisplayRoles
            //                     {
            //                         Id = user.Id,
            //                         UserName = user.UserName,
            //                         Roles = (List<IdentityRole<Guid>>)unitOfWork.Roles
            //                                    .GetRolesByUser(user)
            //                     };

            //foreach (var user in allUsers)
            //{
            //    usersWithRoles.Add(new UserForDisplayRoles
            //    {
            //        Id = user.Id,
            //        UserName = user.UserName,
            //        Roles = (List<string>)userManager.GetRolesAsync(user).Result
            //    });
            //}

            var usersWithRoles = from user in allUsers
                                 select new UserForDisplayRoles
                                 {
                                     Id = user.Id,
                                     UserName = user.UserName,
                                     Roles = (List<string>)userManager.GetRolesAsync(user).Result
                                 };

            return usersWithRoles;
        }

        public async Task<List<string>> ChangeUserRoles(string userName, RoleEditDto roleEditDto)
        {
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                loggerManager.LogInfo($"User with UserName: {userName} is not exist.");

                return null;
            }

            var userRoles = await userManager.GetRolesAsync(user);
            var selectedRoles = roleEditDto.RoleNames ?? new string[] { };

            var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            if (!result.Succeeded)
            {
                loggerManager.LogInfo($"Failed to add to roles");

                return null;
            }

            result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            if (!result.Succeeded)
            {
                loggerManager.LogInfo($"Failed to remove the roles");

                return null;
            }

            var roles = await userManager.GetRolesAsync(user);

            return roles.ToList();
        }

    }
}
