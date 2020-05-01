using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileStorage.Domain.DataTransferredObjects.RoleModels
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole<Guid>, RoleDto>().ReverseMap();
        }
    }
}
