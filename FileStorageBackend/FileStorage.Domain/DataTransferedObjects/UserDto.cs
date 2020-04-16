using System;
using System.Collections.Generic;

namespace FileStorage.Domain.DataTransferredObjects
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
