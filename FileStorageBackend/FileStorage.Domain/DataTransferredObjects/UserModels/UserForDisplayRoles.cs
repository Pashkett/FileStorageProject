using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace FileStorage.Domain.DataTransferredObjects.UserModels
{
    public class UserForDisplayRoles
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
