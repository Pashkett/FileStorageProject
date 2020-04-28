using System.Threading.Tasks;
using AutoMapper;
using FileStorage.Data.Models;
using FileStorage.Data.UnitOfWork;
using FileStorage.Domain.Services.FolderServices;
using Microsoft.AspNetCore.Identity;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;

namespace FileStorage.Domain.Services.AuthenticationServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IFolderService folderService;

        public AuthService(UserManager<User> userManager,
                           SignInManager<User> signInManager,
                           IUnitOfWork unitOfWork,
                           IMapper mapper,
                           IFolderService folderService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.folderService = folderService;
        }

        public async Task<UserDto> LoginAsync(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);
            var result = await signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded)
            {
                var userDto = mapper.Map<User, UserDto>(user);

                return userDto;
            }

            return null;
        }

        public async Task<UserForRegisterDto> RegisterAsync(UserForRegisterDto userForRegister)
        {
            var user = mapper.Map<UserForRegisterDto, User>(userForRegister);
            var result = await userManager.CreateAsync(user, userForRegister.Password);

            if (result.Succeeded)
            {
                FolderCreateRequestDto userFolder = new FolderCreateRequestDto()
                {
                    DisplayName = "MyStorage",
                    IsPrimaryFolder = true,
                    User = user,
                    ParentFolder = null
                };

                var folder = await folderService.CreateFolderAsync(userFolder);
                await unitOfWork.CommitAsync();

                return userForRegister;
            }

            return null;
        }
    }
}
