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
    /// <summary>
    /// Seeding data if needed when application starts.
    /// </summary>
    public static class DataSeeding
    {
        public static void DataInitialization(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var logger = serviceProvider.GetRequiredService<ILoggerManager>();

                try
                {
                    var context = serviceProvider.GetRequiredService<FileStorageContext>();
                    context.Database.Migrate();

                    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
                    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                    var folderManager = serviceProvider.GetRequiredService<IFolderManager>();

                    var config = serviceProvider.GetRequiredService<IConfiguration>();
                    string targetPath = config.GetValue<string>("StoredFilesPath");

                    SeedData(userManager, roleManager, folderManager, logger, targetPath);
                }
                catch (Exception ex)
                {
                    logger.LogError($"{ex}\n An error occurred during migration process");
                }
            }
        }

        private static void SeedData(UserManager<User> userManager,
                                      RoleManager<IdentityRole<Guid>> roleManager,
                                      IFolderManager folderManager,
                                      ILoggerManager logger,
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
                        logger.LogInfo($"Role: {role.Name} has been created.");
                    }
                }

                foreach (var user in users)
                {
                    if (userManager.FindByNameAsync(user.UserName).Result == null)
                    {
                        if (user.UserName == "Admin")
                        {
                            userManager.CreateAsync(user, "admin").Wait();
                            userManager.AddToRoleAsync(user, "Admin").Wait();
                            logger.LogInfo($"User: {user.UserName} has been created.");

                            foreach (var folder in user.StorageItems)
                            {
                                folderManager.CreateFolder(FolderFullPathMaker(targetPath, folder.RelativePath));
                                logger.LogInfo($"Folder {folder.DisplayName} has been created.");
                            }
                        }
                        else
                        {
                            userManager.CreateAsync(user, "password").Wait();
                            userManager.AddToRoleAsync(user, "Member").Wait();
                            logger.LogInfo($"User: {user.UserName} has been created.");

                            foreach (var folder in user.StorageItems)
                            {
                                folderManager.CreateFolder(FolderFullPathMaker(targetPath, folder.RelativePath));
                                logger.LogInfo($"Folder {folder.DisplayName} has been created.");
                            }
                        }
                    }
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




