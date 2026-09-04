using System;
using CoffeeStoreApi.Common;
using CoffeeStoreApi.Dtos;
using CoffeeStoreApi.Models;

namespace CoffeeStoreApi.Services.Auth
{
	public interface IAuthService
	{
		public Task<Response<RegisterDto>> Register(RegisterDto dto);

		public Task<Response<LoginResponseDto>> Login(LoginDto loginDto);

		public Task<Response<RefreshTokenResponseDto>> RefreshToken(string refreshToken);

		public Task<Response<string>> Logout(string id);

		public Task<Response<string>> ForgetPassword(string email);

		public Task<Response<string>> RestPassword(ResetPasswordDto ResetPasswordDto);

    }
}

