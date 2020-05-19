﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FileStorage.Data.Models;
using FileStorage.Data.Persistence.Interfaces;
using FileStorage.Data.QueryModels;
using FileStorage.Data.Persistence.Extensions;

namespace FileStorage.Data.Persistence.Repositories
{
    public class StorageItemRepository : RepositoryBase<StorageItem>, IStorageItemRepository
    {
        private FileStorageContext fileStorageContext =>
            context as FileStorageContext;

        public StorageItemRepository(DbContext dbContext)
            : base(dbContext) { }

        public async Task<(IEnumerable<StorageItem> resultItems, int totalCount)>
            GetActualFilesByUserAsync(User user, StorageItemsRequest itemsRequest)
        {
            var items = fileStorageContext.StorageItems
                .Where(file => file.User == user
                               && file.IsFolder == false
                               && file.IsRecycled == false)
                .FilterStorageItemsBySize(itemsRequest.MinSize, itemsRequest.MaxSize)
                .SearchBy(itemsRequest.SearchTerm);

            var totalCount = await items.CountAsync();

            var resultCollection = await items
                .Sort(itemsRequest.OrderBy)
                .PageStorageItems(itemsRequest.PageNumber, itemsRequest.PageSize)
                .ToListAsync();

            return (resultCollection, totalCount);
        }

        public async Task<StorageItem> GetFileByFileIdAsync(Guid fileId)
        {
            return await fileStorageContext.StorageItems
                .FirstOrDefaultAsync(
                    file => file.Id == fileId
                            && file.IsFolder == false
                            && file.IsRecycled == false);
        }

        public async Task<StorageItem> GetFileByUserAndFileIdAsync(User user,
                                                                   Guid fileId)
        {
            return await fileStorageContext.StorageItems
                .FirstOrDefaultAsync(
                    file => file.Id == fileId
                            && file.User == user
                            && file.IsFolder == false
                            && file.IsRecycled == false);
        }

        public async Task<StorageItem> GetFolderByTrustedNameAsync(string trustedName)
        {
            return await fileStorageContext.StorageItems
                .FirstOrDefaultAsync(
                    folder => folder.TrustedName == trustedName
                              && folder.IsFolder == true
                              && folder.IsRecycled == false);
        }

        public async Task<StorageItem> GetUserPrimaryFolderAsync(User user)
        {
            return await fileStorageContext.StorageItems
                .FirstOrDefaultAsync(
                    folder => folder.IsPrimaryFolder == true
                              && folder.User == user
                              && folder.IsRecycled == false);
        }

        public async Task<StorageItem> GetFolderByUserAndFolderIdIdAsync(User user,
                                                                         Guid folderId)
        {
            return await fileStorageContext.StorageItems
                .FirstOrDefaultAsync(
                    folder => folder.Id == folderId
                              && folder.User == user
                              && folder.IsFolder == true
                              && folder.IsRecycled == false);
        }

        public async Task<(IEnumerable<StorageItem> resultItems, int totalCount)> 
            GetRecycledFilesByUserAsync(User user, StorageItemsRequest itemsRequest)
        {
            var items = fileStorageContext.StorageItems
                .Where(file => file.User == user
                               && file.IsFolder == false
                               && file.IsRecycled == true)
                .SearchBy(itemsRequest.SearchTerm);

            var totalCount = await items.CountAsync();

            var resultItems = await items
                .Sort(itemsRequest.OrderBy)
                .PageStorageItems(itemsRequest.PageNumber, itemsRequest.PageSize)
                .ToListAsync();

            return (resultItems, totalCount);
        }

        public async Task<StorageItem> GetRecycledFileByUserAndFileIdAsync(User user,
                                                                           Guid fileId)
        {
            return await fileStorageContext.StorageItems
                .FirstOrDefaultAsync(
                    file => file.Id == fileId
                            && file.User == user
                            && file.IsFolder == false
                            && file.IsRecycled == true);
        }

        public async Task<(IEnumerable<StorageItem> resultItems, int totalCount)> 
            GetPublicFilesAsync(StorageItemsRequest itemsRequest)
        {
            var items = fileStorageContext.StorageItems
                .Include(file => file.User)
                .Where(file => file.IsPublic == true
                               && file.IsFolder == false
                               && file.IsRecycled == false);

            var totalCount = await items.CountAsync();

            var resultItems = await items
                .PageStorageItems(itemsRequest.PageNumber, itemsRequest.PageSize)
                .OrderBy(file => file.DisplayName)
                .ToListAsync();

            return (resultItems, totalCount);
        }

        public async Task<StorageItem> GetPublicFileByUserAndFileIdAsync(User user,
                                                                         Guid fileId)
        {
            return await fileStorageContext.StorageItems
                .FirstOrDefaultAsync(
                    file => file.Id == fileId
                            && file.User == user
                            && file.IsFolder == false
                            && file.IsRecycled == false
                            && file.IsPublic == true);
        }

        public async Task<StorageItem> GetPublicFileByFileIdAsync(Guid fileId)
        {
            return await fileStorageContext.StorageItems
                .FirstOrDefaultAsync(
                    file => file.Id == fileId
                            && file.IsFolder == false
                            && file.IsRecycled == false
                            && file.IsPublic == true);
        }
    }
}
