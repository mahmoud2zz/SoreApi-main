using System;
namespace CoffeeStoreApi.Dtos
{
	public class RefreshTokenResponseDto
	{
		public string? Token { set; get; } = string.Empty;

        public string? RefreshToken { set; get; } = string.Empty;

		public DateTime Expiration { set; get; }
    }
}

