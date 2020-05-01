using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using FileStorage.Data.Models;

namespace FileStorage.Data.Persistence.Interfaces
{
    public interface IRolesRepository
    {
        Task<IEnumerable<IdentityRole<Guid>>> GetRolesByUserAsync(User user);
        IEnumerable<IdentityRole<Guid>> GetRolesByUser(User user);
    }
}
