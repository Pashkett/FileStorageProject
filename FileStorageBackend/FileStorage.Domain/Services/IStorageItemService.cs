using FileStorage.Domain.DataTransferredObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileStorage.Domain.Services
{
    public interface IStorageItemService
    {
        Task<IEnumerable<StorageItemDto>> GetAllStorageItemsAsync();
        Task<StorageItemDto> GetStorageItemByIdAsync(Guid id);
    }
}