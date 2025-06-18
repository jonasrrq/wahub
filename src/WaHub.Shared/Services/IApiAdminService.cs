using WaHub.Shared.Models;
using WaHub.Shared.Models.Instances;

namespace WaHub.Shared.Services;

public interface IApiAdminService
{
    Task<Response<InstanceDto>> CreateInstanceAsync(InstanceDto request);
    //Task<Response<List<InstanceDto>>> GetUsersAsync(); // Original GetUsersAsync seems to return InstanceDto, renaming for clarity or check if it should be UserDto
    Task<Response<List<UserDto>>> GetUsersAsync(); // Changed to UserDto based on new requirements

    Task<Response<List<string>>> GetRolesAsync();
    Task<Response<string>> CreateRoleAsync(RoleRequestDto roleRequest);
    Task<Response<string>> DeleteRoleAsync(string roleName);
    //Task<Response<List<UserDto>>> GetUsersAsync(); // This was already present or added above
    Task<Response<List<string>>> GetUserRolesAsync(string userId);
    Task<Response<string>> AddUserToRoleAsync(string userId, RoleRequestDto roleRequest);
    Task<Response<string>> RemoveUserFromRoleAsync(string userId, string roleName);
}