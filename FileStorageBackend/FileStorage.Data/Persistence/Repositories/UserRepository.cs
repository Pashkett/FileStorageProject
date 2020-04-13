using FileStorage.Data.Models;
using FileStorage.Data.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileStorage.Data.Persistence.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private FileStorageContext fileStorageContext
        {
            get => context as FileStorageContext;
        }

        public UserRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
