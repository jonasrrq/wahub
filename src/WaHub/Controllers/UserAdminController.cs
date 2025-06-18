using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaHub.Data; // Para ApplicationUser
using WaHub.Shared.Models; // Para Response y UserDto
using WaHub.Shared.Services; // Para IRoleService

namespace WaHub.Controllers
{
    [Authorize(Roles = "Admin")] // Proteger el controlador, ajustar el nombre del rol si es necesario
    [Route("api/[controller]")]
    [ApiController]
    public class UserAdminController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserAdminController(IRoleService roleService, UserManager<ApplicationUser> userManager)
        {
            _roleService = roleService;
            _userManager = userManager;
        }

        // --- Endpoints para Roles ---

        [HttpGet("roles")]
        public async Task<ActionResult<Response<List<string>>>> GetAllRoles()
        {
            var response = await _roleService.GetAllRolesAsync();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("roles")]
        public async Task<ActionResult<Response<string>>> CreateRole([FromBody] RoleRequestDto roleRequest)
        {
            if (roleRequest == null || string.IsNullOrWhiteSpace(roleRequest.RoleName))
            {
                return BadRequest(new Response<string> { Success = false, Message = "El nombre del rol es requerido." });
            }
            var response = await _roleService.CreateRoleAsync(roleRequest.RoleName);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("roles/{roleName}")]
        public async Task<ActionResult<Response<string>>> DeleteRole(string roleName)
        {
            var response = await _roleService.DeleteRoleAsync(roleName);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        // --- Endpoints para Usuarios y sus Roles ---

        [HttpGet("users")]
        public ActionResult<Response<List<UserDto>>> GetAllUsers()
        {
            // Nota: Considerar paginación para grandes cantidades de usuarios
            var users = _userManager.Users
                .Select(u => new UserDto {
                    Id = u.Id,
                    UserName = u.UserName ?? "N/A",
                    Email = u.Email ?? "N/A",
                    FirstName = u.FirstName,
                    LastName = u.LastName
                })
                .ToList();
            return Ok(new Response<List<UserDto>> { Success = true, Data = users });
        }

        [HttpGet("users/{userId}/roles")]
        public async Task<ActionResult<Response<List<string>>>> GetUserRoles(string userId)
        {
            var response = await _roleService.GetUserRolesAsync(userId);
            if (!response.Success)
            {
                return NotFound(response); // O BadRequest si el ID es inválido
            }
            return Ok(response);
        }

        [HttpPost("users/{userId}/roles")]
        public async Task<ActionResult<Response<string>>> AddUserToRole(string userId, [FromBody] RoleRequestDto roleRequest)
        {
            if (roleRequest == null || string.IsNullOrWhiteSpace(roleRequest.RoleName))
            {
                return BadRequest(new Response<string> { Success = false, Message = "El nombre del rol es requerido." });
            }
            var response = await _roleService.AddUserToRoleAsync(userId, roleRequest.RoleName);
            if (!response.Success)
            {
                // Podría ser NotFound si el usuario o rol no existe, o BadRequest por otras razones
                if (response.Message.Contains("no encontrado"))
                    return NotFound(response);
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("users/{userId}/roles/{roleName}")]
        public async Task<ActionResult<Response<string>>> RemoveUserFromRole(string userId, string roleName)
        {
            var response = await _roleService.RemoveUserFromRoleAsync(userId, roleName);
            if (!response.Success)
            {
                // Podría ser NotFound si el usuario o rol no existe, o BadRequest
                 if (response.Message.Contains("no encontrado") || response.Message.Contains("no pertenece"))
                    return NotFound(response);
                return BadRequest(response);
            }
            return Ok(response);
        }
    }

    // DTO para las solicitudes que solo contienen el nombre del rol
    public class RoleRequestDto
    {
        public string? RoleName { get; set; }
    }
}
