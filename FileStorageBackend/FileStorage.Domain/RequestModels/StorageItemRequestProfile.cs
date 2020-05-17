using AutoMapper;
using FileStorage.Data.QueryModels;

namespace FileStorage.Domain.RequestModels
{
    public class StorageItemRequestProfile : Profile
    {
        public StorageItemRequestProfile()
        {
            CreateMap<RequestParameters, StorageItemsRequest>().ReverseMap();
            CreateMap<StorageItemsRequestParameters, StorageItemsRequest>().ReverseMap();
        }
    }
}
