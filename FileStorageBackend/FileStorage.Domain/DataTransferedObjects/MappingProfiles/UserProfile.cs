using AutoMapper;
using FileStorage.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Domain.DataTransferedObjects.Profiles
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
