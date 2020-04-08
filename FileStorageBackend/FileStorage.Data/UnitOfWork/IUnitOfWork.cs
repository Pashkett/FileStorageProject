using System;
using System.Collections.Generic;
using System.Text;
using FileStorage.Persistence.Repositories.Interfaces;

namespace FileStorage.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IStorageItemRepository StorageItems { get; }
        IUserRepository Users { get; }
        int Complete { get; }
    }
}
