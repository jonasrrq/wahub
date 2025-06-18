using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaHub.Services;
using WaHub.Shared.Models;

namespace WaHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize(Policy = AppPermissions.ManageRoles)]
        public async Task<ActionResult<List<AppRole>>> GetRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = AppPermissions.ManageRoles)]
        public async Task<ActionResult<AppRole>> GetRole(Guid id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpPost]
        [Authorize(Policy = AppPermissions.ManageRoles)]
        public async Task<ActionResult<AppRole>> CreateRole(AppRole role)
        {
            var createdRole = await _roleService.CreateRoleAsync(role);
            return CreatedAtAction(nameof(GetRole), new { id = createdRole.Id }, createdRole);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = AppPermissions.ManageRoles)]
        public async Task<ActionResult<AppRole>> UpdateRole(Guid id, AppRole role)
        {
            if (id != role.Id)
                return BadRequest();

            try
            {
                var updatedRole = await _roleService.UpdateRoleAsync(role);
                return Ok(updatedRole);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = AppPermissions.ManageRoles)]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            try
            {
                await _roleService.DeleteRoleAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("user-permissions")]
        [Authorize]
        public async Task<ActionResult<List<string>>> GetUserPermissions()
        {
            var userId = User.Identity.Name;
            var permissions = await _roleService.GetUserPermissionsAsync(userId);
            return Ok(permissions);
        }
    }
}
