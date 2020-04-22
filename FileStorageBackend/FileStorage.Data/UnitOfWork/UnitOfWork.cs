using FileStorage.Data.Persistence;
using FileStorage.Data.Persistence.Interfaces;
using FileStorage.Data.Persistence.Repositories;
using System.Threading.Tasks;

namespace FileStorage.Data.UnitOfWork
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly FileStorageContext context;

        private IUserRepository userRepository;

        public EfUnitOfWork(FileStorageContext context)
        {
            this.context = context;
        }

        public IUserRepository Users =>
            userRepository ?? (userRepository = new UserRepository(context));

        public async Task<int> CommitAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose() => context.Dispose();
    }
}
