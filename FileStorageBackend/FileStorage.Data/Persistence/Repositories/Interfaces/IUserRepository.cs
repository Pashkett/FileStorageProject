using FileStorage.Data.Models;
using FileStorage.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Persistence.Repositories.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {
    }
}
