using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using WaHub.Shared.Models;
using WaHub.Shared.Services;

namespace WaHub.Client.Pages;

public partial class RoleManagement
{
    [Inject] private ILocalizationService _localization { get; set; } = default!;
    [Inject] private NotificationService _notification { get; set; } = default!;
    [Inject] private HttpClient _httpClient { get; set; } = default!;

    private List<RoleDto> roles = new();
    private bool isLoading = true;
    private string? errorMessage;
    
    // Modal states
    private bool showCreateEditModal = false;
    private bool showDeleteModal = false;
    private RoleDto? editingRole = null;
    private RoleDto? roleToDelete = null;
    
    // Form fields
    private string roleName = "";
    private string roleDescription = "";

    protected override async Task OnInitializedAsync()
    {
        await LoadRolesAsync();
    }

    private async Task LoadRolesAsync()
    {
        try
        {
            isLoading = true;
            errorMessage = null;
            
            var response = await _httpClient.GetAsync("api/role");
            if (response.IsSuccessStatusCode)
            {
                var rolesArray = await response.Content.ReadFromJsonAsync<RoleDto[]>();
                roles = rolesArray?.ToList() ?? new List<RoleDto>();
            }
            else
            {
                errorMessage = "Failed to load roles";
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Error loading roles: {ex.Message}";
        }
        finally
        {
            isLoading = false;
            StateHasChanged();
        }
    }

    private void ShowCreateRoleModal()
    {
        editingRole = null;
        roleName = "";
        roleDescription = "";
        showCreateEditModal = true;
    }

    private void EditRole(RoleDto role)
    {
        editingRole = role;
        roleName = role.Name;
        roleDescription = role.Description ?? "";
        showCreateEditModal = true;
    }

    private void CloseCreateEditModal()
    {
        showCreateEditModal = false;
        editingRole = null;
        roleName = "";
        roleDescription = "";
    }

    private async Task SaveRole()
    {
        try
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return;

            if (editingRole == null)
            {
                // Create new role
                var createRequest = new CreateRoleRequest
                {
                    Name = roleName.Trim(),
                    Description = string.IsNullOrWhiteSpace(roleDescription) ? null : roleDescription.Trim()
                };

                var response = await _httpClient.PostAsJsonAsync("api/role", createRequest);
                if (response.IsSuccessStatusCode)
                {
                    _notification.ShowSuccess($"Role '{roleName}' created successfully");
                    CloseCreateEditModal();
                    await LoadRolesAsync();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _notification.ShowError($"Failed to create role: {errorContent}");
                }
            }
            else
            {
                // Update existing role
                var updateRequest = new UpdateRoleRequest
                {
                    Name = roleName.Trim(),
                    Description = string.IsNullOrWhiteSpace(roleDescription) ? null : roleDescription.Trim()
                };

                var response = await _httpClient.PutAsJsonAsync($"api/role/{editingRole.Id}", updateRequest);
                if (response.IsSuccessStatusCode)
                {
                    _notification.ShowSuccess($"Role '{roleName}' updated successfully");
                    CloseCreateEditModal();
                    await LoadRolesAsync();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _notification.ShowError($"Failed to update role: {errorContent}");
                }
            }
        }
        catch (Exception ex)
        {
            _notification.ShowError($"Error saving role: {ex.Message}");
        }
    }

    private void DeleteRole(RoleDto role)
    {
        roleToDelete = role;
        showDeleteModal = true;
    }

    private void CloseDeleteModal()
    {
        showDeleteModal = false;
        roleToDelete = null;
    }

    private async Task ConfirmDeleteRole()
    {
        if (roleToDelete == null) return;

        try
        {
            var response = await _httpClient.DeleteAsync($"api/role/{roleToDelete.Id}");
            if (response.IsSuccessStatusCode)
            {
                _notification.ShowSuccess($"Role '{roleToDelete.Name}' deleted successfully");
                CloseDeleteModal();
                await LoadRolesAsync();
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _notification.ShowError($"Failed to delete role: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            _notification.ShowError($"Error deleting role: {ex.Message}");
        }
    }
}