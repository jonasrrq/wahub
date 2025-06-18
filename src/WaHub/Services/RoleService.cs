using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaHub.Data; // Para ApplicationUser y ApplicationRole
using WaHub.Shared.Models; // Para Response
using WaHub.Shared.Services;

namespace WaHub.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<Response<List<string>>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                // Asegurarse de que Name no sea null antes de agregarlo a la lista
                var roleNames = roles.Where(name => name != null).ToList();
                return new Response<List<string>> { Success = true, Data = roleNames };
            }
            catch (Exception ex)
            {
                return new Response<List<string>> { Success = false, Message = $"Error al obtener roles: {ex.Message}" };
            }
        }

        public async Task<Response<string>> CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return new Response<string> { Success = false, Message = "El nombre del rol no puede estar vacío." };
            }

            try
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (roleExists)
                {
                    return new Response<string> { Success = false, Message = $"El rol '{roleName}' ya existe." };
                }

                var result = await _roleManager.CreateAsync(new ApplicationRole(roleName));
                if (result.Succeeded)
                {
                    return new Response<string> { Success = true, Data = roleName, Message = $"Rol '{roleName}' creado exitosamente." };
                }
                return new Response<string> { Success = false, Message = $"Error al crear el rol: {string.Join(", ", result.Errors.Select(e => e.Description))}" };
            }
            catch (Exception ex)
            {
                return new Response<string> { Success = false, Message = $"Error al crear el rol: {ex.Message}" };
            }
        }

        public async Task<Response<string>> DeleteRoleAsync(string roleName)
        {
             if (string.IsNullOrWhiteSpace(roleName))
            {
                return new Response<string> { Success = false, Message = "El nombre del rol no puede estar vacío." };
            }
            try
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    return new Response<string> { Success = false, Message = $"El rol '{roleName}' no fue encontrado." };
                }

                var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
                if (usersInRole.Any())
                {
                    return new Response<string> { Success = false, Message = $"El rol '{roleName}' no puede ser eliminado porque tiene usuarios asignados." };
                }

                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return new Response<string> { Success = true, Message = $"Rol '{roleName}' eliminado exitosamente." };
                }
                return new Response<string> { Success = false, Message = $"Error al eliminar el rol: {string.Join(", ", result.Errors.Select(e => e.Description))}" };
            }
            catch (Exception ex)
            {
                return new Response<string> { Success = false, Message = $"Error al eliminar el rol: {ex.Message}" };
            }
        }

        public async Task<Response<List<string>>> GetUserRolesAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new Response<List<string>> { Success = false, Message = "El ID de usuario no puede estar vacío." };
            }
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new Response<List<string>> { Success = false, Message = "Usuario no encontrado." };
                }
                var roles = await _userManager.GetRolesAsync(user);
                return new Response<List<string>> { Success = true, Data = roles.ToList() };
            }
            catch (Exception ex)
            {
                 return new Response<List<string>> { Success = false, Message = $"Error al obtener roles del usuario: {ex.Message}" };
            }
        }

        public async Task<Response<string>> AddUserToRoleAsync(string userId, string roleName)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(roleName))
            {
                return new Response<string> { Success = false, Message = "El ID de usuario y el nombre del rol no pueden estar vacíos." };
            }
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new Response<string> { Success = false, Message = "Usuario no encontrado." };
                }
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    return new Response<string> { Success = false, Message = $"Rol '{roleName}' no encontrado." };
                }

                if (await _userManager.IsInRoleAsync(user, roleName))
                {
                     return new Response<string> { Success = false, Message = $"El usuario ya pertenece al rol '{roleName}'." };
                }

                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    return new Response<string> { Success = true, Message = $"Usuario asignado al rol '{roleName}' exitosamente." };
                }
                return new Response<string> { Success = false, Message = $"Error al asignar rol al usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}" };
            }
            catch (Exception ex)
            {
                return new Response<string> { Success = false, Message = $"Error al asignar rol al usuario: {ex.Message}" };
            }
        }

        public async Task<Response<string>> RemoveUserFromRoleAsync(string userId, string roleName)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(roleName))
            {
                return new Response<string> { Success = false, Message = "El ID de usuario y el nombre del rol no pueden estar vacíos." };
            }
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new Response<string> { Success = false, Message = "Usuario no encontrado." };
                }
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    return new Response<string> { Success = false, Message = $"Rol '{roleName}' no encontrado." };
                }
                if (!await _userManager.IsInRoleAsync(user, roleName))
                {
                     return new Response<string> { Success = false, Message = $"El usuario no pertenece al rol '{roleName}'." };
                }
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    return new Response<string> { Success = true, Message = $"Rol '{roleName}' removido del usuario exitosamente." };
                }
                return new Response<string> { Success = false, Message = $"Error al remover rol del usuario: {string.Join(", ", result.Errors.Select(e => e.Description))}" };
            }
            catch (Exception ex)
            {
                return new Response<string> { Success = false, Message = $"Error al remover rol del usuario: {ex.Message}" };
            }
        }
    }
}
