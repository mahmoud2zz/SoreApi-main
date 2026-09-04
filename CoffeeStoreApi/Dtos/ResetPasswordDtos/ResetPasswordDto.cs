using System;
namespace CoffeeStoreApi.Dtos
{
	public class ResetPasswordDto
	{
		public string? Email { set; get; } = string.Empty;

		public string? Token { set; get; } = string.Empty;

		public string? NewPassword { set; get; } = string.Empty;
	}
}

