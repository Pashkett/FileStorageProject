﻿using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Data.Models;

namespace FileStorage.Domain.Services.FolderServices
{
    public interface IFolderService
    {
        Task<IEnumerable<StorageItemDto>> GetAllStorageItemsAsync();
        Task<StorageItem> CreateFolderAsync(FolderCreateRequestDto folderCreate);
        Task RenameFolderAsync(Guid id, string folderNewName);
        Task DeleteFolderAsync(Guid id);
    }
}