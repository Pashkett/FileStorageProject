using AutoMapper;
using FileStorage.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Domain.DataTransferedObjects.StorageItemModels
{
    public class StorageItemProfile : Profile
    {
        public StorageItemProfile()
        {
            CreateMap<StorageItem, FileCreateRequestDto>().ReverseMap();
            CreateMap<StorageItem, FolderCreateRequestDto>().ReverseMap();
        }
    }
}
