using AutoMapper;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using FileStorage.Data.Models;
using FileStorage.Domain.DataTransferredObjects.UserModels;
using FileStorage.Domain.DataTransferredObjects.StorageItemModels;
using FileStorage.Domain.Services.PrivateItems;

namespace FileStorage.Domain.Services.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        private readonly IPrivateItemsService privateItemsService;

        public AuthService(UserManager<User> userManager,
                           SignInManager<User> signInManager,
                           IMapper mapper,
                           IConfiguration configuration,
                           IPrivateItemsService privateItemsService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.configuration = configuration;
            this.privateItemsService = privateItemsService;
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
                await userManager.AddToRoleAsync(user, "Member");

                FolderCreateRequestDto userFolder = new FolderCreateRequestDto()
                {
                    DisplayName = "MyStorage",
                    IsPrimaryFolder = true,
                    User = user,
                    ParentFolder = null
                };

                await privateItemsService.CreateFolderAsync(userFolder);

                return userForRegister;
            }

            return null;
        }

        public async Task<string> GenerateJwtTokenAsync(UserDto userDto)
        {
            var signingCredentials = GetSigningCredentials();

            var user = mapper.Map<UserDto, User>(userDto);
            var claims = await GetClaimsAsync(user);

            var tokenDescriptor = GenerateTokenDescriptor(signingCredentials, claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<UserDto> GetRequestedUser(ClaimsPrincipal principal)
        {
            var user = await userManager.GetUserAsync(principal);
            var userDto = mapper.Map<User, UserDto>(user);

            return userDto;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));

            return new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        }

        private async Task<List<Claim>> GetClaimsAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private SecurityTokenDescriptor GenerateTokenDescriptor(SigningCredentials signingCredentials,
                                                                List<Claim> claims)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = signingCredentials
            };

            return tokenDescriptor;
        }
    }
}
