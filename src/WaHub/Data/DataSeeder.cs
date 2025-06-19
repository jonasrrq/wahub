using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection; // Required for GetRequiredService

namespace WaHub.Data // Assuming ApplicationUser is in WaHub.Data namespace from previous context
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAndAdminUserAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Create the roles and seed them to the database
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        // Log error or handle - for now, we'll throw an exception to halt startup if critical roles fail
                        throw new Exception($"Error creating role {roleName}: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                    }
                }
            }

            // Check if an admin user exists
            // This is a simple check; you might have more sophisticated ways to identify admin users
            var adminUsers = await userManager.GetUsersInRoleAsync("Admin");
            if (!adminUsers.Any())
            {
                // Create a default admin user
                // IMPORTANT: In a real application, get these details from configuration or secure storage, not hardcoded!
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@wahub.com",
                    Email = "admin@wahub.com",
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true // Typically confirm email for admin
                };

                // IMPORTANT: Use a strong, configurable password!
                var result = await userManager.CreateAsync(adminUser, "Admin@123!");
                if (result.Succeeded)
                {
                    // Assign the new user to the "Admin" role
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    // Log error or handle
                     throw new Exception($"Error creating admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
