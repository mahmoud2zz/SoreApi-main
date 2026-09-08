using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CoffeeStoreApi.Dtos
{
	public class RegisterDto
	{
        [Required]
        public string FullName { set; get; } = string.Empty;
        [Required]
        public string UserName { set; get; } = string.Empty;
        [Required]
        public string Email { set; get; } = string.Empty;
        [Required]
        public string Password { set; get; } = string.Empty;
    }
}

