using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaHub.Services; // For RoleService
using WaHub.Data;     // For ApplicationUser (if needed directly, though unlikely here)

namespace WaHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class RoleAdminController : ControllerBase
    {
        private readonly RoleService _roleService;
        private readonly UserManager<ApplicationUser> _userManager; // For user operations if not in RoleService

        public RoleAdminController(RoleService roleService, UserManager<ApplicationUser> userManager)
        {
            _roleService = roleService;
            _userManager = userManager;
        }

        // GET: api/roleadmin/roles
        [HttpGet("roles")]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> GetRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        // GET: api/roleadmin/roles/{id}
        [HttpGet("roles/{id}")]
        public async Task<ActionResult<IdentityRole>> GetRole(string id)
        {
            var role = await _roleService.FindRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound($"Role with ID '{id}' not found.");
            }
            return Ok(role);
        }

        // GET: api/roleadmin/roles/name/{name}
        [HttpGet("roles/name/{name}")]
        public async Task<ActionResult<IdentityRole>> GetRoleByName(string name)
        {
            var role = await _roleService.FindRoleByNameAsync(name);
            if (role == null)
            {
                return NotFound($"Role with name '{name}' not found.");
            }
            return Ok(role);
        }

        // POST: api/roleadmin/roles
        [HttpPost("roles")]
        public async Task<ActionResult<IdentityRole>> CreateRole([FromBody] RoleCreationModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.RoleName))
            {
                return BadRequest("Role name is required.");
            }

            var result = await _roleService.CreateRoleAsync(model.RoleName);
            if (result.Succeeded)
            {
                var newRole = await _roleService.FindRoleByNameAsync(model.RoleName);
                return CreatedAtAction(nameof(GetRoleByName), new { name = newRole.Name }, newRole);
            }
            return BadRequest(result.Errors);
        }

        // PUT: api/roleadmin/roles/{id}
        [HttpPut("roles/{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] RoleUpdateModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.NewName))
            {
                return BadRequest("New role name is required.");
            }

            var role = await _roleService.FindRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound($"Role with ID '{id}' not found.");
            }

            role.Name = model.NewName;
            // role.NormalizedName will be updated by RoleManager
            var result = await _roleService.UpdateRoleAsync(role);
            if (result.Succeeded)
            {
                return NoContent();
            }
            return BadRequest(result.Errors);
        }

        // DELETE: api/roleadmin/roles/{id}
        [HttpDelete("roles/{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            if (result.Succeeded)
            {
                return NoContent();
            }
            var error = result.Errors.FirstOrDefault();
            if (error != null && error.Description.Contains("not found"))
            {
                return NotFound(error.Description);
            }
            return BadRequest(result.Errors);
        }

        // POST: api/roleadmin/users/{userId}/roles
        [HttpPost("users/{userId}/roles")]
        public async Task<IActionResult> AddUserToRole(string userId, [FromBody] UserRoleModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.RoleName))
            {
                return BadRequest("Role name is required.");
            }

            var result = await _roleService.AssignRoleToUserAsync(userId, model.RoleName);
            if (result.Succeeded)
            {
                return Ok($"User '{userId}' added to role '{model.RoleName}'.");
            }
            return BadRequest(result.Errors);
        }

        // DELETE: api/roleadmin/users/{userId}/roles/{roleName}
        // Note: RoleName in path needs to be URL encoded if it contains special characters.
        [HttpDelete("users/{userId}/roles/{roleName}")]
        public async Task<IActionResult> RemoveUserFromRole(string userId, string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name is required.");
            }
            var decodedRoleName = System.Net.WebUtility.UrlDecode(roleName);
            var result = await _roleService.RemoveRoleFromUserAsync(userId, decodedRoleName);
            if (result.Succeeded)
            {
                return Ok($"User '{userId}' removed from role '{decodedRoleName}'.");
            }
            return BadRequest(result.Errors);
        }

        // GET: api/roleadmin/users/{userId}/roles
        [HttpGet("users/{userId}/roles")]
        public async Task<ActionResult<IEnumerable<string>>> GetUserRoles(string userId)
        {
            var roles = await _roleService.GetUserRolesAsync(userId);
            if (roles == null || !roles.Any())
            {
                // Consider if user not found vs user has no roles should be different results
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return NotFound($"User with ID '{userId}' not found.");
                return Ok(new List<string>()); // User exists but has no roles
            }
            return Ok(roles);
        }

        // GET: api/roleadmin/roles/{roleName}/users
        [HttpGet("roles/{roleName}/users")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsersInRole(string roleName)
        {
            var decodedRoleName = System.Net.WebUtility.UrlDecode(roleName);
            var users = await _roleService.GetUsersInRoleAsync(decodedRoleName);
            if (users == null || !users.Any())
            {
                 var role = await _roleService.FindRoleByNameAsync(decodedRoleName);
                 if (role == null) return NotFound($"Role '{decodedRoleName}' not found.");
                 return Ok(new List<ApplicationUser>()); // Role exists but has no users
            }
            return Ok(users.Select(u => new { u.Id, u.UserName, u.Email, u.FirstName, u.LastName })); // Return selected fields
        }
    }

    // Helper models for request bodies
    public class RoleCreationModel
    {
        public string RoleName { get; set; }
    }

    public class RoleUpdateModel
    {
        public string NewName { get; set; }
    }

    public class UserRoleModel
    {
        public string RoleName { get; set; }
    }
}
