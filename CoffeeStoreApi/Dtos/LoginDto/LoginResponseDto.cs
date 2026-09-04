using System;
namespace CoffeeStoreApi.Dtos
{
	public class LoginResponseDto
	{
		public string? FullName { set; get; }

		public string? Email { set; get; }

        public string? UserName { set; get; }

		public string? Token { set; get; } = string.Empty;

        public string? RefreshToken { set; get; }

        public DateTime Expiration { get; set; }


    }
}

