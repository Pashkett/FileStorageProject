using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileStorage.Data.Models;
using FileStorage.Data.UnitOfWork;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Domain.Exceptions;


namespace FileStorage.Domain.Services.PublicItemsServices
{
    public class PublicItemsService : IPublicItemsService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PublicItemsService(IUnitOfWork unitOfWork,
                                  IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<FileItemDto>> GetPublicFilesAsync()
        {
            var files = await unitOfWork.StorageItems.GetAllPublicFilesAsync();

            var filesDto = mapper.Map<IEnumerable<StorageItem>, IEnumerable<FileItemDto>>(files);

            return filesDto;
        }

        public async Task MoveFilePrivateAsync(UserDto userDto, string fileId)
        {
            var fileItem = await GetPublicItemByUserAndItemIdAsync(userDto, fileId);

            fileItem.IsPublic = false;

            await unitOfWork.CommitAsync();
        }

        private async Task<StorageItem> GetPublicItemByUserAndItemIdAsync(UserDto userDto, string fileId)
        {
            var user = mapper.Map<UserDto, User>(userDto);

            if (Guid.TryParse(fileId, out Guid storageItemId) == false)
                throw new ArgumentException($"{fileId} is not valid id");

            var file = await unitOfWork.StorageItems
                .GetPublicFileByUserAndFileIdAsync(user, storageItemId);

            if (file == null)
                throw new StorageItemNotFoundException($"File for current user does not exist.");
            
            return file;
        }
    }
}
