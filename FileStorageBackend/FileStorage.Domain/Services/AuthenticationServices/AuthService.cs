using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using FileStorage.Data.Models;
using FileStorage.Data.UnitOfWork;
using FileStorage.Domain.UserModels;
using FileStorage.Domain.Services.FolderServices;
using FileStorage.Domain.DataTransferedObjects.StorageItemModels;

namespace FileStorage.Domain.Services.AuthenticationServices
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IFolderService folderService;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IFolderService folderService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.folderService = folderService;
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

        public async Task<UserForRegisterDto> RegisterAsync(UserForRegisterDto userForRegister)
        {
            byte[] passwordHash;
            byte[] passwordSalt;
            CreatePasswordHash(userForRegister.Password, out passwordHash, out passwordSalt);
            var user = mapper.Map<UserForRegisterDto, User>(userForRegister);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var userFolder = new FolderCreateRequestDto()
            {
                DisplayName = "MyStorage",
                IsRootFolder = true,
                User = user,
                ParentFolder = null
            };

            await unitOfWork.Users.AddAsync(user);

            user.UserRootFolder = await folderService.CreateFolderAsync(userFolder);

            await unitOfWork.CommitAsync();
           
            return userForRegister;
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            if (await unitOfWork.Users
                .SingleOrDefaultAsync(user => user.Username == username) == null)
            {
                return false;
            }
                

            return true;
        }

        private bool VerifyPasswordHash(string password,
                                byte[] passwordHash,
                                byte[] passwordSalt)
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
    }
}
