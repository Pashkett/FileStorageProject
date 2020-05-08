using System;
using System.Collections.Generic;

namespace FileStorage.Domain.DataTransferredObjects.UserModels
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}
