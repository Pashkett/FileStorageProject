using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FileStorage.Data.Models;
using FileStorage.Persistence.Repositories.Interfaces;

namespace FileStorage.Persistence.Repositories
{
    public class StorageItemRepository : IStorageItemRepository
    {
        private FileStorageContext context;

        public StorageItemRepository(FileStorageContext context)
        {
            this.context = context;
        }
        
        public void Create(StorageItem entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(StorageItem entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StorageItem> FindAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StorageItem> FindByCondition(Expression<Func<StorageItem, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public void Update(StorageItem entity)
        {
            throw new NotImplementedException();
        }
    }
}
