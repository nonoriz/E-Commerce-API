using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;

        public AuthenticationService(UserManager<ApplicationUser> userManager,IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
           var User= await userManager.FindByNameAsync(email);
            return User != null;
        }

        public async Task<Result<UserDTO>> GetUserByEmailAsync(string email)
        {
            var User=await userManager.FindByNameAsync(email);
            if (User is null)
                return Error.NotFound("User.NotFound", $"No User With Email {email} was found");
            return new UserDTO(User.Email, User.DisplayName, await CreateTokenAsync(User));
        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
          var user =await  userManager.FindByEmailAsync(loginDTO.Email);
            if (user is null)
                return Error.InvalidCredentials("User.InvalidCredentials");
            var Token=await CreateTokenAsync(user);
            return new UserDTO(user.Email!,user.DisplayName, Token);
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            
            var user = new ApplicationUser
            {
                Email = registerDTO.Email,
                DisplayName = registerDTO.DisplayName,
                UserName = registerDTO.UserName,
                PhoneNumber = registerDTO.PhoneNumber
            };
            var IdentityResult=await userManager.CreateAsync(user, registerDTO.Password);
            if(IdentityResult.Succeeded)
            {
                var Token = await CreateTokenAsync(user);
                return new UserDTO(user.Email!, user.DisplayName, Token);
            }
            return  IdentityResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();

        }

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            //Token [Issuer,Audience,Claims,Expires,SignInCredentials]

            var Claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email!),
                new Claim(JwtRegisteredClaimNames.Name,user.UserName!)
            };
            var Roles = await userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var SecretKey = configuration["JWTOptions:SecretKey"];
            var Key=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var Cred = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(
                issuer: configuration["JWTOptions:Issuer"],
                audience: configuration["JWTOptions:Audience"],
                expires: DateTime.UtcNow.AddHours(1),
                claims: Claims,
                signingCredentials: Cred
                );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
