using AutoMapper;
using FileStorage.Data.Models;

namespace FileStorage.Domain.DataTransferredObjects.UserModels
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserForRegisterDto>().ReverseMap();
            CreateMap<User, UserForLoginDto>().ReverseMap();
            CreateMap<User, UserForDisplayRoles>().ReverseMap();
        }
    }
}
