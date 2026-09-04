using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CoffeeStoreApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace CoffeeStoreApi.Helpers
{
    public class JwtTokenHelper
    {
        private readonly JWT _jwt;

        public JwtTokenHelper(JWT jwt)
        {
            _jwt = jwt;
        }

        public async Task<JwtSecurityToken> CreateJwtToken(
            ApplicationUser user,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            var claims = new List<Claim>
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.UserName ?? string.Empty),

                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()),

                new Claim(
                    JwtRegisteredClaimNames.Email,
                    user.Email ?? string.Empty),

                new Claim(
                    JwtRegisteredClaimNames.Name,
                    user.FullName ?? string.Empty),

                new Claim(
                    "uid",
                    user.Id)
            };

            // User Claims
            var userClaims = await userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            // User Roles
            var roles = await userManager.GetRolesAsync(user);

            foreach (var roleName in roles)
            {
                // Add Role to JWT
                claims.Add(
                    new Claim(ClaimTypes.Role, roleName)
                );

                // Get Role
                var role = await roleManager.FindByNameAsync(roleName);

                if (role == null)
                    continue;

                // Get Role Permissions
                var roleClaims = await roleManager.GetClaimsAsync(role);

                claims.AddRange(roleClaims);
            }

            // JWT Key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwt.Key));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    _jwt.DurationInMinutes),
                signingCredentials: credentials
            );
        }
    }
}