using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FileStorage.Data.Models;
using FileStorage.Data.UnitOfWork;

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
                    var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
                    var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

                    SeedData(userManager, unitOfWork);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
        }

        private static void SeedData(UserManager<User> userManager, 
                                    IUnitOfWork unitOfWork
                                    //, RoleManager<IdentityRole> roleManager
                                    )
        {

            SeedUsers(userManager);
            SeedStorageItems(unitOfWork);
            //SeedRoles(roleManager);
        }

        private static void SeedUsers(UserManager<User> userManager)
        {
            var users = SeedingDataHelper.SeedingDataFromJson<User>("Users.json");

            foreach (var user in users)
            {
                if (userManager.FindByNameAsync(user.UserName).Result == null)
                {
                    IdentityResult result = userManager.CreateAsync(user, "password_goes_here").Result;
                }
            }
        }

        private static void SeedStorageItems(IUnitOfWork unitOfWork)
        {
            var primaryFolders = SeedingDataHelper.SeedingDataFromJson<StorageItem>("StorageItems.json");

            foreach (var folder in primaryFolders)
            {
                var result = unitOfWork.StorageItems.SingleOrDefaultAsync(f => f.Id == folder.Id).Result;
                if (result == null)
                {
                    unitOfWork.StorageItems.AddAsync(folder).Wait();
                    unitOfWork.CommitAsync().Wait();
                }
            }
        }

        private static void SeedRoles(object roleManager)
        {
            throw new NotImplementedException();
        }
    }
}




