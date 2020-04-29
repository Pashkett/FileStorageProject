using System;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using FileStorage.Data.Models;
using FileStorage.Data.Persistence;
using FileStorage.Logger;
using FileStorage.Data.FileSystemManagers.StorageFolderManager;

namespace FileStorage.Domain.Seed
{
    public static class DataSeeding
    {
        public static void DataInitialization(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var context = serviceProvider.GetRequiredService<FileStorageContext>();
                    context.Database.Migrate();

                    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
                    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                    var folderManager = serviceProvider.GetRequiredService<IFolderManager>();
                    var config = serviceProvider.GetRequiredService<IConfiguration>();
                    string targetPath = config.GetValue<string>("StoredFilesPath");

                    SeedData(userManager, roleManager, folderManager, targetPath);
                }
                catch(Exception ex)
                {
                    var logger = serviceProvider.GetRequiredService<ILoggerManager>();
                    logger.LogError($"{ex}\n An error occurred during migration process");
                }
            }
        }

        private static void SeedData(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager,
            IFolderManager folderManager, 
            string targetPath)
        {
            var users = SeedingDataHelper.SeedingDataFromJson<User>("Users.json");
            try
            {
                List<IdentityRole<Guid>> roles = new List<IdentityRole<Guid>>
                {
                    new IdentityRole<Guid>{Name = "Member"},
                    new IdentityRole<Guid>{Name = "Moderator"},
                    new IdentityRole<Guid>{Name = "Admin"}
                };

                foreach (var role in roles)
                {
                    var isExist = roleManager.FindByNameAsync(role.Name).Result == null;
                    if (isExist)
                    {
                        roleManager.CreateAsync(role).Wait();
                    }
                }

                foreach (var user in users)
                {
                    if (userManager.FindByNameAsync(user.UserName).Result == null)
                    {
                        userManager.CreateAsync(user, "password_goes_here").Wait();
                        userManager.AddToRoleAsync(user, "Member").Wait();

                        foreach (var folder in user.StorageItems)
                        {
                            folderManager.CreateFolder(FolderFullPathMaker(targetPath, folder.RelativePath));
                        }
                    }
                }

                var adminUser = new User { UserName = "Admin" };
                var result = userManager.CreateAsync(adminUser, "Admin").Result;

                if (result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("Admin").Result;
                    userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" }).Wait();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        private static string FolderFullPathMaker(string targetPath, string relativePath)
        {
            var parentPath = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();

            if (targetPath == null || relativePath == null)
                throw new ArgumentNullException("One of the argument equals null");
            else
                return Path.Combine(parentPath, targetPath, relativePath);
        }
    }
}




