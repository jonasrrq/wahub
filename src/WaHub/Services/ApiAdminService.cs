using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; // Required for ToListAsync on IQueryable
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WaHub.Data; // For ApplicationUser
using WaHub.Shared.Models;
using WaHub.Shared.Models.Instances; // Still here for CreateInstanceAsync
using WaHub.Shared.Services;

namespace WaHub.Services
{
    public class ApiAdminService : ApiServiceBase, IApiAdminService
    {
        private readonly IRoleService _roleService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApiAdminService(
            IHttpClientFactory httpClientFactory,
            IRoleService roleService,
            UserManager<ApplicationUser> userManager)
            : base(httpClientFactory, "ApiAdminHttpClient") // Assuming ApiServiceBase and this client name are still relevant for other things if any
        {
            _roleService = roleService;
            _userManager = userManager;
        }

        // This method was originally returning InstanceDto and calling "/admin/users"
        // It's now correctly typed as per IApiAdminService to return UserDto.
        // The actual implementation should fetch users, not instances.
        public async Task<Response<List<UserDto>>> GetUsersAsync()
        {
            try
            {
                // This service is server-side, so it can directly access UserManager
                var users = await _userManager.Users
                    .Select(u => new UserDto
                    {
                        Id = u.Id,
                        UserName = u.UserName ?? "N/A",
                        Email = u.Email ?? "N/A",
                        FirstName = u.FirstName,
                        LastName = u.LastName
                    })
                    .ToListAsync();
                return new Response<List<UserDto>> { Success = true, Data = users };
            }
            catch (System.Exception ex)
            {
                return new Response<List<UserDto>> { Success = false, Message = $"Error fetching users: {ex.Message}" };
            }
        }

        // CreateInstanceAsync implementation remains as it was, but its endpoint and purpose might need review.
        // It's not directly related to the role management features being tested.
        public async Task<Response<InstanceDto>> CreateInstanceAsync(InstanceDto request)
        {
            // The base class PostAsync would need to be reviewed if this service is purely server-side now
            // For now, assuming it's some external API call or placeholder.
            // If this service is purely a server-side orchestrator, it wouldn't typically use HttpClient to call itself.
            // However, to keep changes minimal to this unrelated method:
            var result = await PostAsync("/admin/instances", request); // Changed endpoint to be more specific

            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<Response<InstanceDto>>() ?? new Response<InstanceDto>();
            }
            else
            {
                return new Response<InstanceDto>
                {
                    Success = false,
                    Message = $"Error creating instance (status: {result.StatusCode}). Please try again later."
                };
            }
        }

        // --- Implementation of new IApiAdminService methods for roles and user roles ---

        public async Task<Response<List<string>>> GetRolesAsync()
        {
            return await _roleService.GetAllRolesAsync();
        }

        public async Task<Response<string>> CreateRoleAsync(RoleRequestDto roleRequest)
        {
            if (roleRequest == null || string.IsNullOrWhiteSpace(roleRequest.RoleName))
            {
                return new Response<string> { Success = false, Message = "Role name cannot be empty." };
            }
            return await _roleService.CreateRoleAsync(roleRequest.RoleName);
        }

        public async Task<Response<string>> DeleteRoleAsync(string roleName)
        {
            return await _roleService.DeleteRoleAsync(roleName);
        }

        public async Task<Response<List<string>>> GetUserRolesAsync(string userId)
        {
            return await _roleService.GetUserRolesAsync(userId);
        }

        public async Task<Response<string>> AddUserToRoleAsync(string userId, RoleRequestDto roleRequest)
        {
            if (roleRequest == null || string.IsNullOrWhiteSpace(roleRequest.RoleName))
            {
                return new Response<string> { Success = false, Message = "Role name cannot be empty for adding user to role." };
            }
            return await _roleService.AddUserToRoleAsync(userId, roleRequest.RoleName);
        }

        public async Task<Response<string>> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            return await _roleService.RemoveUserFromRoleAsync(userId, roleName);
        }
    }
}
