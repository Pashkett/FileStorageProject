﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.Reflection;
using FileStorage.Data.Persistence;
using FileStorage.Data.UnitOfWork;
using FileStorage.Domain.Services.FolderServices;

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
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
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
