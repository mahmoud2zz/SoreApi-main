using System;
using System.Security.Cryptography;
using CoffeeStoreApi.Models;

namespace CoffeeStoreApi.Helpers
{
	public class RefreshTokenGenerator
	{
        public static RefreshToken Generate()
        {
            var randomBytes = new byte[32];
            using var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow
            };
        }
    }
}

