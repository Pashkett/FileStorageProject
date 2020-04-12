using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FileStorage.Data.Models;
using FileStorage.Domain.DataTransferedObjects;
using FileStorage.UnitOfWork;

namespace FileStorage.Domain.Services.Implementations
{
    public class StorageItemService : IStorageItemService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public StorageItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<StorageItemDto>> GetAllStorageItemAsync()
        {
            var storageItems = await unitOfWork.StorageItems.GetAllAsync();
            var storageItemsDto = 
                mapper.Map<IEnumerable<StorageItem>, IEnumerable<StorageItemDto>>(storageItems);

            return storageItemsDto;
        }

        public async Task<StorageItemDto> GetStorageItemByIdAsync(Guid id)
        {
            var storageItem = await unitOfWork.StorageItems.GetByIdAsync(id);
            var storageItemDto = 
                mapper.Map<StorageItem, StorageItemDto>(storageItem);

            return storageItemDto;
        }
    }
}
