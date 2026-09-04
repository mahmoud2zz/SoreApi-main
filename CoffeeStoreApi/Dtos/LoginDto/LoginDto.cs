using System;
using System.ComponentModel.DataAnnotations;

namespace CoffeeStoreApi.Dtos
{
	public class LoginDto
	{
        [Required]
        public string? Email { set; get; } = string.Empty;
        [Required]
        public string? Password { set; get; } = string.Empty;
    }
}

