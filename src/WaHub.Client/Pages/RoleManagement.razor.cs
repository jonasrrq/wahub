using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaHub.Shared.Models;
using WaHub.Shared.Services; // Para IApiAdminService
using WaHub.Client.Services; // Para NotificationService si es local


namespace WaHub.Client.Pages
{
    public partial class RoleManagement
    {
        // Properties ApiAdminService and NotificationService are injected via @inject in the .razor file.
        // Remove [Inject] private IApiAdminService ApiAdminService { get; set; } = default!;
        // Remove [Inject] private NotificationService NotificationService { get; set; } = default!;


        private List<string> roles = new List<string>();
        private List<UserDto> users = new List<UserDto>();
        private string newRoleName = string.Empty;
        private bool isLoading = true;

        private UserDto? selectedUser;
        private List<string> selectedUserRoles = new List<string>();
        private string roleToAssignOrRemove = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await LoadRoles();
            await LoadUsers();
            isLoading = false;
        }

        private async Task LoadRoles()
        {
            var response = await ApiAdminService.GetRolesAsync();
            if (response.Success && response.Data != null)
            {
                roles = response.Data;
            }
            else
            {
                NotificationService.ShowError(response.Message ?? "Error al cargar roles.");
                roles = new List<string>(); // Asegurar que roles no sea null
            }
            StateHasChanged();
        }

        private async Task LoadUsers()
        {
            var response = await ApiAdminService.GetUsersAsync();
            if (response.Success && response.Data != null)
            {
                users = response.Data;
            }
            else
            {
                 NotificationService.ShowError(response.Message ?? "Error al cargar usuarios.");
                 users = new List<UserDto>(); // Asegurar que users no sea null
            }
             StateHasChanged();
        }

        private async Task CreateRole()
        {
            if (string.IsNullOrWhiteSpace(newRoleName))
            {
                NotificationService.ShowWarning("El nombre del rol no puede estar vacío.");
                return;
            }
            var response = await ApiAdminService.CreateRoleAsync(new RoleRequestDto { RoleName = newRoleName });
            if (response.Success)
            {
                NotificationService.ShowSuccess($"Rol '{newRoleName}' creado.");
                newRoleName = string.Empty;
                await LoadRoles();
            }
            else
            {
                NotificationService.ShowError(response.Message ?? "Error al crear rol.");
            }
        }

        private async Task DeleteRole(string roleName)
        {
            // Podríamos añadir una confirmación aquí
            var response = await ApiAdminService.DeleteRoleAsync(roleName);
            if (response.Success)
            {
                NotificationService.ShowSuccess($"Rol '{roleName}' eliminado.");
                await LoadRoles();
                if (selectedUser != null) await LoadUserRoles(selectedUser.Id); // Recargar roles del usuario seleccionado
            }
            else
            {
                NotificationService.ShowError(response.Message ?? "Error al eliminar rol.");
            }
        }

        private async Task SelectUser(UserDto user)
        {
            selectedUser = user;
            roleToAssignOrRemove = string.Empty;
            await LoadUserRoles(user.Id);
        }

        private async Task LoadUserRoles(string userId)
        {
            if (selectedUser == null || selectedUser.Id != userId) return;

            var response = await ApiAdminService.GetUserRolesAsync(userId);
            if (response.Success && response.Data != null)
            {
                selectedUserRoles = response.Data;
            }
            else
            {
                NotificationService.ShowError(response.Message ?? $"Error al cargar roles para {selectedUser.UserName}.");
                selectedUserRoles = new List<string>();
            }
            StateHasChanged();
        }

        private async Task AssignRoleToUser()
        {
            if (selectedUser == null || string.IsNullOrWhiteSpace(roleToAssignOrRemove)) return;

            var response = await ApiAdminService.AddUserToRoleAsync(selectedUser.Id, new RoleRequestDto { RoleName = roleToAssignOrRemove });
            if (response.Success)
            {
                NotificationService.ShowSuccess($"Rol '{roleToAssignOrRemove}' asignado a {selectedUser.UserName}.");
                await LoadUserRoles(selectedUser.Id);
            }
            else
            {
                NotificationService.ShowError(response.Message ?? "Error al asignar rol.");
            }
        }

        private async Task RemoveRoleFromUser()
        {
            if (selectedUser == null || string.IsNullOrWhiteSpace(roleToAssignOrRemove)) return;

            var response = await ApiAdminService.RemoveUserFromRoleAsync(selectedUser.Id, roleToAssignOrRemove);
            if (response.Success)
            {
                NotificationService.ShowSuccess($"Rol '{roleToAssignOrRemove}' removido de {selectedUser.UserName}.");
                await LoadUserRoles(selectedUser.Id);
            }
            else
            {
                NotificationService.ShowError(response.Message ?? "Error al remover rol.");
            }
        }
    }
}
