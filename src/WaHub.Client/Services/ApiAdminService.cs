using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using WaHub.Shared.Models;
using WaHub.Shared.Models.Instances; // Retained if other methods use it, can be removed if not.
using WaHub.Shared.Services;

namespace WaHub.Client.Services
{
    internal class ApiAdminService : IApiAdminService
    {
        private readonly HttpClient _http;

        public ApiAdminService(HttpClient httpClient)
        {
            _http = httpClient;
        }

        // Assuming CreateInstanceAsync is still needed, otherwise it can be removed.
        // If it's related to 'Instances' and not 'UserAdmin', its endpoint might be different.
        // For now, I'll keep it but point to a placeholder or correct endpoint if known.
        public async Task<Response<InstanceDto>> CreateInstanceAsync(InstanceDto request)
        {
            // Placeholder: Update endpoint if this method is still relevant and different from user admin
            // var result = await _http.PostAsJsonAsync("api/instanceadmin/create", request);
            // return await result.Content.ReadFromJsonAsync<Response<InstanceDto>>();
            // For now, returning not implemented or a default response:
            return await Task.FromResult(new Response<InstanceDto> { Success = false, Message = "CreateInstanceAsync endpoint not specified for ApiAdminService." });
        }

        public async Task<Response<List<UserDto>>> GetUsersAsync()
        {
            return await _http.GetFromJsonAsync<Response<List<UserDto>>>("api/useradmin/users") ?? new Response<List<UserDto>> { Success = false, Message = "Failed to load users or empty response."};
        }

        public async Task<Response<List<string>>> GetRolesAsync()
        {
            return await _http.GetFromJsonAsync<Response<List<string>>>("api/useradmin/roles") ?? new Response<List<string>> { Success = false, Message = "Failed to load roles or empty response."};
        }

        public async Task<Response<string>> CreateRoleAsync(RoleRequestDto roleRequest)
        {
            var result = await _http.PostAsJsonAsync("api/useradmin/roles", roleRequest);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<Response<string>>() ?? new Response<string> { Success = false, Message = "Failed to deserialize create role response."};
            }
            return new Response<string> { Success = false, Message = $"Error creating role: {result.ReasonPhrase}" };
        }

        public async Task<Response<string>> DeleteRoleAsync(string roleName)
        {
            var result = await _http.DeleteAsync($"api/useradmin/roles/{roleName}");
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<Response<string>>() ?? new Response<string> { Success = false, Message = "Failed to deserialize delete role response."};
            }
            return new Response<string> { Success = false, Message = $"Error deleting role: {result.ReasonPhrase}" };
        }

        public async Task<Response<List<string>>> GetUserRolesAsync(string userId)
        {
            return await _http.GetFromJsonAsync<Response<List<string>>>($"api/useradmin/users/{userId}/roles") ?? new Response<List<string>> { Success = false, Message = "Failed to load user roles or empty response."};
        }

        public async Task<Response<string>> AddUserToRoleAsync(string userId, RoleRequestDto roleRequest)
        {
            var result = await _http.PostAsJsonAsync($"api/useradmin/users/{userId}/roles", roleRequest);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<Response<string>>() ?? new Response<string> { Success = false, Message = "Failed to deserialize add user to role response."};
            }
            return new Response<string> { Success = false, Message = $"Error adding user to role: {result.ReasonPhrase}" };
        }

        public async Task<Response<string>> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            var result = await _http.DeleteAsync($"api/useradmin/users/{userId}/roles/{roleName}");
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<Response<string>>() ?? new Response<string> { Success = false, Message = "Failed to deserialize remove user from role response."};
            }
            return new Response<string> { Success = false, Message = $"Error removing user from role: {result.ReasonPhrase}" };
        }
    }
}
