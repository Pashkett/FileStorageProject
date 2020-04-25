using AutoMapper;
using FileStorage.Data.Models;

namespace FileStorage.Domain.UserModels.UserModels
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<User, UserForRegisterDto>().ReverseMap();
            CreateMap<User, UserForLoginDto>().ReverseMap();
        }
    }
}
