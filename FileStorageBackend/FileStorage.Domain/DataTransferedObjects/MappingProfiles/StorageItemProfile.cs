using AutoMapper;
using FileStorage.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Domain.DataTransferredObjects.Profiles
{
    public class StorageItemProfile : Profile
    {
        public StorageItemProfile()
        {
            CreateMap<StorageItem, StorageItemDto>();
        }
    }
}
