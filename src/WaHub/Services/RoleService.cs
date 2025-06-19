using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaHub.Data; // Assuming ApplicationUser is here

namespace WaHub.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // Method to create a new role
        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Role name cannot be empty.", nameof(roleName));
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
            {
                // Consider returning a specific result or error code
                return IdentityResult.Failed(new IdentityError { Description = $"Role '{roleName}' already exists." });
            }
            return await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
        }

        // Method to retrieve all roles
        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        // Method to find a role by ID
        public async Task<IdentityRole?> FindRoleByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        // Method to find a role by name
        public async Task<IdentityRole?> FindRoleByNameAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }

        // Method to update an existing role
        public async Task<IdentityResult> UpdateRoleAsync(IdentityRole role)
        {
            // Ensure role is not null and has an ID
            if (role == null || string.IsNullOrEmpty(role.Id))
            {
                throw new ArgumentException("Role cannot be null and must have an ID.", nameof(role));
            }
            return await _roleManager.UpdateAsync(role);
        }

        // Method to delete a role
        public async Task<IdentityResult> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Role with ID '{roleId}' not found." });
            }
            return await _roleManager.DeleteAsync(role);
        }

        // Method to assign a role to a user
        public async Task<IdentityResult> AssignRoleToUserAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"User with ID '{userId}' not found." });
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Role '{roleName}' not found." });
            }

            return await _userManager.AddToRoleAsync(user, roleName);
        }

        // Method to remove a role from a user
        public async Task<IdentityResult> RemoveRoleFromUserAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"User with ID '{userId}' not found." });
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                // Or, you might decide it's not an error if the role doesn't exist,
                // or if the user isn't in that role.
                // For now, let's ensure the role exists before attempting removal.
                return IdentityResult.Failed(new IdentityError { Description = $"Role '{roleName}' not found." });
            }

            return await _userManager.RemoveFromRoleAsync(user, roleName);
        }

        // Method to get roles for a specific user
        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Or throw an exception, or return an empty list
                return new List<string>();
            }
            return await _userManager.GetRolesAsync(user);
        }

        // Method to get all users in a specific role
        public async Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName)
        {
            return await _userManager.GetUsersInRoleAsync(roleName);
        }
    }
}
