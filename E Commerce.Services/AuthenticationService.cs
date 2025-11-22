using E_Commerce.Domain.Entities.IdentityModule;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AuthenticationService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO)
        {
          var user =await  userManager.FindByEmailAsync(loginDTO.Email);
            if (user is null)
                return Error.InvalidCredentials("User.InvalidCredentials");
            return new UserDTO(user.Email!,user.DisplayName,"Token");
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
                return new UserDTO(user.Email!, user.DisplayName, "Token");
            }
            return  IdentityResult.Errors.Select(e => Error.Validation(e.Code, e.Description)).ToList();

        }
    }
}
