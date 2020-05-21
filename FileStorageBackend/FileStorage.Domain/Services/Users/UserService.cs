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
using System.Security.Claims;
using AutoMapper;
using System.Net.Http;

namespace FileStorage.Domain.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly ILoggerManager loggerManager;
        private readonly IMapper mapper;

        public UserService(UserManager<User> userManager,
                           ILoggerManager loggerManager,
                           IMapper mapper)
        {
            this.userManager = userManager;
            this.loggerManager = loggerManager;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<UserForDisplayRoles>> GetUserWithRolesAsync()
        {
            var allUsers = await userManager.Users.ToListAsync();

            var usersWithRoles = from user in allUsers
                                 select new UserForDisplayRoles
                                 {
                                     Id = user.Id,
                                     UserName = user.UserName,
                                     Roles = (List<string>)userManager.GetRolesAsync(user).Result
                                 };

            return usersWithRoles;
        }

        public async Task<List<string>> ChangeUserRolesAsync(string userName, RoleEditDto roleEditDto)
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

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            if (id == null)
                throw new ArgumentException("User id is null");

            var user = await userManager.FindByIdAsync(id);
            var userDto = mapper.Map<User, UserDto>(user);

            return userDto;
        }
    }
}
