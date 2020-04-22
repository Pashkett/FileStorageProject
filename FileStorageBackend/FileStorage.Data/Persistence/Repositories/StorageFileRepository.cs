using FileStorage.Data.Models;
using FileStorage.Data.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Data.Persistence.Repositories
{
    public class StorageFileRepository : RepositoryBase<StorageFile>, IStorageFileRepository
    {
        public StorageFileRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
