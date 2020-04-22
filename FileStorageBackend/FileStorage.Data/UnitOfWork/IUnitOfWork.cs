using System;
using System.Threading.Tasks;
using FileStorage.Data.Persistence.Interfaces;

namespace FileStorage.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        Task<int> CommitAsync();
    }
}
