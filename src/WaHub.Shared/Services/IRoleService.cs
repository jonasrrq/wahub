using WaHub.Shared.Models;

namespace WaHub.Shared.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetAllRolesAsync();
    Task<RoleDto?> GetRoleByIdAsync(string roleId);
    Task<RoleDto?> GetRoleByNameAsync(string roleName);
    Task<bool> CreateRoleAsync(string roleName, string? description = null);
    Task<bool> UpdateRoleAsync(string roleId, string roleName, string? description = null);
    Task<bool> DeleteRoleAsync(string roleId);
    Task<bool> AssignRoleToUserAsync(string userId, string roleName);
    Task<bool> RemoveRoleFromUserAsync(string userId, string roleName);
    Task<IEnumerable<string>> GetUserRolesAsync(string userId);
    Task<IEnumerable<UserDto>> GetUsersInRoleAsync(string roleName);
    Task<bool> UserIsInRoleAsync(string userId, string roleName);
}