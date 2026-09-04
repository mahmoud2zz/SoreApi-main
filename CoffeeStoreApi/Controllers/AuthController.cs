using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoffeeStoreApi.Dtos;
using CoffeeStoreApi.Services.Auth;
using CoffeeStoreApi.Helpers;
using CoffeeStoreApi.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoffeeStoreApi.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

    

        // POST api/values
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseBuilder.Failure<string?>("Data is Not Vaild", null));

            var response = await _authService.Register(dto);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginDto dto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ResponseBuilder.Failure<string?>("Data is Not Vaild", null));

            var respones = await _authService.Login(dto);

            if (!respones.Success)
                return Unauthorized(respones);

            return Ok(respones);
        }

        [HttpPost("refeshToken")]
        public async Task<IActionResult> RefreshToken([FromForm] string refreshToken)
        {
           
            var respones = await _authService.RefreshToken(refreshToken);
            if (!respones.Success)
                return Unauthorized(respones);
            return Ok(respones);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst("uid")?.Value;

            var respones = await _authService.Logout(userId!);

            if (!respones.Success)
                return NotFound(respones);

            return Ok(respones);
        }

        
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromForm] string email)
        {
            var respones = await _authService.ForgetPassword(email);
            if (!respones.Success)
            {
                return NotFound(respones);
            }

            return Ok(respones);
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> Resetpassword([FromForm] ResetPasswordDto dto)
        {
            var respones = await _authService.RestPassword(dto);
            if (!respones.Success)
            {
                return NotFound(respones);
            }
            return Ok(respones);
        }

        
    }
}

