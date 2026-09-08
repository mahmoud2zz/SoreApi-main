using System;
using System.IdentityModel.Tokens.Jwt;
using CoffeeStoreApi.Common;
using CoffeeStoreApi.Dtos;
using CoffeeStoreApi.Helpers;
using CoffeeStoreApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CoffeeStoreApi.Services.Auth
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IEmailService _emailService;


        private readonly JWT _jwt;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
            _emailService = emailService;
        }

        public async Task<Response<RegisterDto>> Register(RegisterDto dto)
        {
            var existUser = await _userManager.FindByEmailAsync(dto.Email);

            if (existUser != null)
                return ResponseBuilder.Failure<RegisterDto>("Email already registered.", null);


            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                UserName = dto.UserName,
                Email = dto.Email
            };

            // 1. Create User
            


            if (await _roleManager.RoleExistsAsync("USER"))
            {
                throw new Exception("Test 500 error");
            }
            var createResult = await _userManager.CreateAsync(user, dto.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ",
                    createResult.Errors.Select(e => e.Description));

                return ResponseBuilder.Failure<RegisterDto>(
                    errors,
                    null
                );

            }

           




            if (!await _roleManager.RoleExistsAsync("USER"))
                {
                    var roleCreateResult = await _roleManager.CreateAsync(
                        new IdentityRole("USER")
                    );

                    if (!roleCreateResult.Succeeded)
                    {
                        await _userManager.DeleteAsync(user);

                        var errors = string.Join(", ",
                            roleCreateResult.Errors.Select(e => e.Description));

                        return ResponseBuilder.Failure<RegisterDto>(
                            errors,
                            null
                        );
                    }
                }

                // 3. Add user to USER role
                var roleResult = await _userManager.AddToRoleAsync(user, "USER");

                if (!roleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(user);

                    var errors = string.Join(", ",
                        roleResult.Errors.Select(e => e.Description));

                    return ResponseBuilder.Failure<RegisterDto>(
                        errors,
                        null
                    );

                }
            
           
         


                return ResponseBuilder.Success("Operation Successful", dto);
        }
        public async Task<Response<LoginResponseDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.Users.Include(u => u.refreshTokens).FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return ResponseBuilder.Failure<LoginResponseDto>("Invalid email or password", null);
            }

            var jwtTokenHelper = new JwtTokenHelper(_jwt);

            var jwtToken = await jwtTokenHelper.CreateJwtToken(user, _userManager,_roleManager);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);


            foreach (var refreshToken in user!.refreshTokens.Where(r => r.IsActive).ToList())
            {
                refreshToken.RevokedOn = DateTime.UtcNow;
            }
            var RefreshToken = RefreshTokenGenerator.Generate();

            user.refreshTokens.Add(RefreshToken);

            await _userManager.UpdateAsync(user);

            return ResponseBuilder.Success("Login successful",
                new LoginResponseDto()
                { FullName = user.FullName, Email = user.Email, UserName = user.UserName, Token = accessToken, RefreshToken = RefreshToken.Token, Expiration = RefreshToken.ExpiresOn });

        }

        public async Task<Response<RefreshTokenResponseDto>> RefreshToken(string refreshToken)
        {
            var user = await _userManager.Users
                .Include(u => u.refreshTokens)
                .FirstOrDefaultAsync(u =>
                    u.refreshTokens.Any(r => r.Token == refreshToken));

            if (user == null)
                return ResponseBuilder.Failure<RefreshTokenResponseDto>("Invalid refresh token", null);

            var existingToken = user.refreshTokens
                .FirstOrDefault(r => r.Token == refreshToken);

            if (existingToken == null || !existingToken.IsActive)
                return ResponseBuilder.Failure<RefreshTokenResponseDto>("Refresh token expired or revoked", null);
            existingToken.RevokedOn = DateTime.UtcNow;
            var jwtHelper = new JwtTokenHelper(_jwt);
            var jwtToken = await jwtHelper.CreateJwtToken(user, _userManager,_roleManager);
            var newAccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            var newRefreshToken = RefreshTokenGenerator.Generate();
            user.refreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);
            return ResponseBuilder.Success(
                "Token Refreshed successfully",
                new RefreshTokenResponseDto()
                {
                    Token = newAccessToken,
                    RefreshToken = newRefreshToken.Token,
                    Expiration = newRefreshToken.ExpiresOn
                });
        }


        public async Task<Response<string>> Logout(string id)
        {
            var existsUser = await _userManager.Users.Include(u => u.refreshTokens).FirstOrDefaultAsync(u => u.Id == id);

            if (existsUser == null)
                return ResponseBuilder.Failure<string>("Not Found User", null);

            foreach (var rt in existsUser.refreshTokens.Where(r => r.IsActive).ToList())
            {
                rt.RevokedOn = DateTime.UtcNow;
            }

            await _userManager.UpdateAsync(existsUser);

            return ResponseBuilder.Success<string>("Logged out successfully", null);
        }

        public async Task<Response<string>> ForgetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return ResponseBuilder.Failure<string>("User not found", null);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = $"http://localhost:4200/reset-password?email={email}&token={Uri.EscapeDataString(token)}";

            await _emailService.SendEmailAsync(
           email,
           "Reset Password",
           $"<p>Click to reset password:</p><a href='{resetLink}'>Reset Password</a>"
       );

            return ResponseBuilder.Success<string>("Reset link sent to email", null);


        }
        public async Task<Response<string>> RestPassword(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return ResponseBuilder.Failure<string>("User not found", null);

            var token = Uri.UnescapeDataString(dto.Token!);

            var result = await _userManager.ResetPasswordAsync(user, token, dto.NewPassword);

            if (!result.Succeeded)
                return ResponseBuilder.Failure<string>(
                    string.Join(",", result.Errors.Select(e => e.Description)),
                    null
                );

            return ResponseBuilder.Success<string>("Password reset successfully", null);
        }
    }
}

