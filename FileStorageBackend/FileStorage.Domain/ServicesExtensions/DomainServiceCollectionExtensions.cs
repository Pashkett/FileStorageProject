using AutoMapper;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FileStorage.Data.Persistence;
using FileStorage.Data.UnitOfWork;
using FileStorage.Domain.Services.FolderServices;
using FileStorage.Data.FileSystemManagers.StorageFolderManager;
using FileStorage.Data.FileSystemManagers.StorageFileManager;
using FileStorage.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace FileStorage.Domain.ServiceExtensions
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

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            IdentityBuilder builder = services.AddIdentityCore<User>(opts =>
            {
                opts.Password.RequireDigit = false;
                opts.Password.RequiredLength = 4;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase = false;
            });

            builder = new IdentityBuilder(builder.UserType,
                                          typeof(IdentityRole),
                                          builder.Services);

            builder.AddEntityFrameworkStores<FileStorageContext>();
            builder.AddRoleValidator<RoleValidator<IdentityRole>>();
            builder.AddRoleManager<RoleManager<IdentityRole>>();
            builder.AddSignInManager<SignInManager<User>>();
        }

        public static void ConfigureUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        }

        public static void ConfigureFileSystemManagers(this IServiceCollection services)
        {
            services.AddScoped<IFolderManager, FolderManager>();
            services.AddScoped<IFileManager, FileManager>();
        }

        public static void ConfigureFolderService(this IServiceCollection services)
        {
            services.AddScoped<IFolderService, FolderService>();
        }

        public static void ConfigureAutomapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
