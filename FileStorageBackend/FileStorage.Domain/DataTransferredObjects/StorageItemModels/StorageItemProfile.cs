using AutoMapper;
using FileStorage.Data.Models;
using FileStorage.Domain.PagingHelpers;

namespace FileStorage.Domain.DataTransferredObjects.StorageItemModels
{
    public class StorageItemProfile : Profile
    {
        public StorageItemProfile()
        {
            CreateMap<StorageItem, FolderCreateRequestDto>().ReverseMap();
            CreateMap<StorageItem, FileItemDto>().ReverseMap();
        }
    }
}
