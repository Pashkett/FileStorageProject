﻿using FileStorage.Data.Models;
using FileStorage.Data.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FileStorage.Data.Persistence.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private FileStorageContext fileStorageContext => 
            context as FileStorageContext;

        public UserRepository(DbContext dbContext) 
            : base(dbContext) { }
    }
}
