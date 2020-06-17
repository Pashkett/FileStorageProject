using System;
using AutoMapper;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FileStorage.Data.Persistence;
using FileStorage.Data.UnitOfWork;
using FileStorage.Data.FileSystemManagers.StorageFolderManager;
using FileStorage.Data.FileSystemManagers.StorageFileManager;
using FileStorage.Data.Models;
using System.IO.Abstractions;

namespace FileStorage.Domain.ServicesExtensions
{
    /// <summary>
    /// Class with extension that allows to keep all Data layer services configuration in one place.
    /// Also servers for decoupling Data layer from API layer.
    /// </summary>
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
                                          typeof(IdentityRole<Guid>),
                                          builder.Services);

            builder.AddEntityFrameworkStores<FileStorageContext>();
            builder.AddUserManager<UserManager<User>>();
            builder.AddRoleValidator<RoleValidator<IdentityRole<Guid>>>();
            builder.AddRoleManager<RoleManager<IdentityRole<Guid>>>();
            builder.AddSignInManager<SignInManager<User>>();
        }

        public static void ConfigureUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        }

        public static void ConfigureFileSystemAbstraction(this IServiceCollection services)
        {
            services.AddSingleton<IFileSystem, FileSystem>();
        }

        public static void ConfigureFileSystemManagers(this IServiceCollection services)
        {
            services.AddScoped<IFolderManager, FolderManager>();
            services.AddScoped<IFileManager, FileManager>();
        }

        public static void ConfigureAutomapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
