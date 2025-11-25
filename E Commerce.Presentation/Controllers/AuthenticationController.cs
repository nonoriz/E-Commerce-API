using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.DTOs.IdentityDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        //Login
        //POST:baseURL/api/authentication/login
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var Result = await authenticationService.LoginAsync(loginDTO);
            return HandleResult(Result);
        }

        //Register
        //POST:baseURL/api/authentication/register
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var Result = await authenticationService.RegisterAsync(registerDTO);
            return HandleResult(Result);

        }

        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmail(string Email)
        {
            var Result=await authenticationService.CheckEmailAsync(Email);
            return Ok(Result);

        }

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var Email=User.FindFirstValue(ClaimTypes.Email)!;
            var Result=await authenticationService.GetUserByEmailAsync(Email);
            return HandleResult(Result);
        }
    }
}
