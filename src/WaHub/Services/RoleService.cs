using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WaHub.Data;
using WaHub.Shared.Models;
using WaHub.Shared.Services;

namespace WaHub.Services;

public class RoleService : IRoleService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        var roleDtos = new List<RoleDto>();

        foreach (var role in roles)
        {
            var userCount = await _userManager.GetUsersInRoleAsync(role.Name!);
            roleDtos.Add(new RoleDto
            {
                Id = role.Id,
                Name = role.Name!,
                UserCount = userCount.Count,
                CreatedDate = DateTime.UtcNow // Identity roles don't have created date by default
            });
        }

        return roleDtos;
    }

    public async Task<RoleDto?> GetRoleByIdAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null) return null;

        var userCount = await _userManager.GetUsersInRoleAsync(role.Name!);
        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name!,
            UserCount = userCount.Count,
            CreatedDate = DateTime.UtcNow
        };
    }

    public async Task<RoleDto?> GetRoleByNameAsync(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null) return null;

        var userCount = await _userManager.GetUsersInRoleAsync(role.Name!);
        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name!,
            UserCount = userCount.Count,
            CreatedDate = DateTime.UtcNow
        };
    }

    public async Task<bool> CreateRoleAsync(string roleName, string? description = null)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
            return false;

        var role = new IdentityRole(roleName);
        var result = await _roleManager.CreateAsync(role);
        return result.Succeeded;
    }

    public async Task<bool> UpdateRoleAsync(string roleId, string roleName, string? description = null)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null) return false;

        role.Name = roleName;
        role.NormalizedName = roleName.ToUpperInvariant();

        var result = await _roleManager.UpdateAsync(role);
        return result.Succeeded;
    }

    public async Task<bool> DeleteRoleAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);
        if (role == null) return false;

        // Check if role has users
        var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
        if (usersInRole.Any()) return false; // Cannot delete role with users

        var result = await _roleManager.DeleteAsync(role);
        return result.Succeeded;
    }

    public async Task<bool> AssignRoleToUserAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        if (!await _roleManager.RoleExistsAsync(roleName))
            return false;

        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded;
    }

    public async Task<bool> RemoveRoleFromUserAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.RemoveFromRoleAsync(user, roleName);
        return result.Succeeded;
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return new List<string>();

        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IEnumerable<UserDto>> GetUsersInRoleAsync(string roleName)
    {
        var users = await _userManager.GetUsersInRoleAsync(roleName);
        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userDtos.Add(new UserDto
            {
                Id = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                FullName = user.FullName,
                CompanyName = user.CompanyName,
                Avatar = user.Avatar,
                Roles = roles,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            });
        }

        return userDtos;
    }

    public async Task<bool> UserIsInRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        return await _userManager.IsInRoleAsync(user, roleName);
    }
}