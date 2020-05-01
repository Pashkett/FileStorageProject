using FileStorage.Data.Models;
using FileStorage.Data.Persistence.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileStorage.Data.Persistence.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        protected readonly FileStorageContext context;

        public RolesRepository(FileStorageContext dbContext)
        {
            context = dbContext;
        }

        public async Task<IEnumerable<IdentityRole<Guid>>> GetRolesByUserAsync(User user)
        {
            var roles = new List<IdentityRole<Guid>>();
            var userRoles = await context.UserRoles.Where(ur => ur.UserId == user.Id).ToListAsync();

            foreach (var userRole in userRoles)
            {
                roles.AddRange(await context.Roles.Where(role => role.Id == userRole.RoleId).ToListAsync());
            }

            return roles;
        }

        public IEnumerable<IdentityRole<Guid>> GetRolesByUser(User user)
        {
            var roles = new List<IdentityRole<Guid>>();
            var userRoles = context.UserRoles.Where(ur => ur.UserId == user.Id).ToList();

            foreach (var userRole in userRoles)
            {
                roles.AddRange(context.Roles.Where(role => role.Id == userRole.RoleId).ToList());
            }

            return roles;
        }
    }
}
