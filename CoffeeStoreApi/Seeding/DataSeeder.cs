using System;
using CoffeeStoreApi.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using CoffeeStoreApi.Models;

namespace CoffeeStoreApi.Seeding
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Create Roles
            await CreateRoleAsync(roleManager, "Admin");
            await CreateRoleAsync(roleManager, "User");

            // Admin Permissions
            await AddPermissionsToRoleAsync(
                roleManager,
                "Admin",
                Permissions.All);

            // User Permissions
            var userPermissions = new[]
            {
            Permissions.GetProducts,
            Permissions.GetProductById,
            Permissions.CreateOrder,
            Permissions.CreatePayment
        };

            await AddPermissionsToRoleAsync(
                roleManager,
                "User",
                userPermissions);

            // Create Admin User
            await CreateAdminUserAsync(userManager);
        }

        private static async Task CreateRoleAsync(
            RoleManager<IdentityRole> roleManager,
            string roleName)
        {
            if (await roleManager.RoleExistsAsync(roleName))
                return;

            var result = await roleManager.CreateAsync(
                new IdentityRole(roleName));

            if (!result.Succeeded)
            {
                throw new Exception(
                    $"Failed to create role: {roleName}");
            }
        }

        private static async Task AddPermissionsToRoleAsync(
            RoleManager<IdentityRole> roleManager,
            string roleName,
            IEnumerable<string> permissions)
        {
            var role = await roleManager.FindByNameAsync(roleName);

            if (role == null)
                throw new Exception($"Role '{roleName}' not found.");

            var existingClaims =
                await roleManager.GetClaimsAsync(role);

            foreach (var permission in permissions)
            {
                var exists = existingClaims.Any(c =>
                    c.Type == "Permission" &&
                    c.Value == permission);

                if (exists)
                    continue;

                var result = await roleManager.AddClaimAsync(
                    role,
                    new Claim("Permission", permission));

                if (!result.Succeeded)
                {
                    throw new Exception(
                        $"Failed to add permission '{permission}' " +
                        $"to role '{roleName}'.");
                }
            }
        }

        private static async Task CreateAdminUserAsync(
            UserManager<ApplicationUser> userManager)
        {
            const string email = "admin@example.com";
            const string password = "Admin@123";

            var admin = await userManager.FindByEmailAsync(email);

            if (admin != null)
                return;

            admin = new ApplicationUser
            {
                FullName = "System Admin",
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                
            };

            var result = await userManager.CreateAsync(
                admin,
                password);

            if (!result.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    result.Errors.Select(e => e.Description));

                throw new Exception(
                    $"Failed to create admin user: {errors}");
            }

            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}

