using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FileStorage.Data.Models;
using FileStorage.Persistence;
using FileStorage.Persistence.Repositories.Interfaces;

namespace FileStorage.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private FileStorageContext context;

        public UserRepository(FileStorageContext context)
        {
            this.context = context;
        }

        public void Create(User entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(User entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> FindAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> FindByCondition(Expression<Func<User, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public void Update(User entity)
        {
            throw new NotImplementedException();
        }
    }
}
