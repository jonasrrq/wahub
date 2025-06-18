using System.Collections.Generic;
using System.Threading.Tasks;
using WaHub.Shared.Models; // Para Response u otros modelos compartidos si se usan

namespace WaHub.Shared.Services
{
    public interface IRoleService
    {
        Task<Response<List<string>>> GetAllRolesAsync();
        Task<Response<string>> CreateRoleAsync(string roleName);
        Task<Response<string>> DeleteRoleAsync(string roleName);
        // Task<Response<string>> UpdateRoleAsync(string oldRoleName, string newRoleName); // Considerar si es necesario, IdentityRole no se actualiza fácilmente por nombre
        Task<Response<List<string>>> GetUserRolesAsync(string userId);
        Task<Response<string>> AddUserToRoleAsync(string userId, string roleName);
        Task<Response<string>> RemoveUserFromRoleAsync(string userId, string roleName);
    }
}
