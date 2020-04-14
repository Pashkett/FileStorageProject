using AutoMapper;
using FileStorage.Data.Models;
using FileStorage.Data.UnitOfWork;
using FileStorage.Domain.DataTransferredObjects;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.Domain.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<UserDto> LoginAsync(string username, string password)
        {
            var user = await unitOfWork.Users.SingleOrDefaultAsync(user => user.Username == username);
            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            var userDto = mapper.Map<User, UserDto>(user);

            return userDto;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }

                return true;
            }
        }

        public async Task<UserForRegisterDto> RegisterAsync(UserForRegisterDto userForRegister)
        {
            byte[] passwordHash;
            byte[] passwordSalt;
            CreatePasswordHash(userForRegister.Password, out passwordHash, out passwordSalt);
            var user = mapper.Map<UserForRegisterDto, User>(userForRegister);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            

            await unitOfWork.Users.AddAsync(user);
            await unitOfWork.CommitAsync();
           
            return userForRegister;
        }

        private void CreatePasswordHash(string password,
                                        out byte[] passwordHash,
                                        out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            if (await unitOfWork.Users.SingleOrDefaultAsync(user => user.Username == username) == null)
                return false;

            return true;
        }
    }
}
