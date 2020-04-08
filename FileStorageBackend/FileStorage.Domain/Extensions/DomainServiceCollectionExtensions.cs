using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FileStorage.Persistence;
using FileStorage.UnitOfWork;

namespace FileStorage.Domain.Extensions
{
    public static class DomainServiceCollectionExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services,
                                               IConfiguration configuration)
        {
            services.AddDbContext<FileStorageContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
                    b => b.MigrationsAssembly("FileStorage.Data")));
        }

        public static void ConfigureUnitOfWork(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, EfUnitOfWork>();
        }
    }
}
