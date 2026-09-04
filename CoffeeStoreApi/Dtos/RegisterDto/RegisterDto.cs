using System;
using System.Text.Json.Serialization;

namespace CoffeeStoreApi.Dtos
{
	public class RegisterDto
	{
        public string? FullName { set; get; } = string.Empty;

		public string? UserName { set; get; } = string.Empty;

        public string? Email { set; get; } = string.Empty;

        public string? Role { set; get; } = string.Empty;
        [JsonIgnore]
        public string? Password { set; get; } = string.Empty;
    }
}

