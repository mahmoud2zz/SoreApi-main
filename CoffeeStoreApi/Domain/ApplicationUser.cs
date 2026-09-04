using System;
using Microsoft.AspNetCore.Identity;

namespace CoffeeStoreApi.Models
{
	public class ApplicationUser: IdentityUser
    {
		public string? FullName { set; get; } = string.Empty;
		public List<RefreshToken> refreshTokens { set; get; } =  new List<RefreshToken>() ;
		public List<Order> Orders { set; get; } = new List<Order>(); 

	}
}

