using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaHub.Shared.Models;
using WaHub.Shared.Services;

namespace WaHub.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllRoles()
    {
        var roles = await _roleService.GetAllRolesAsync();
        return Ok(roles);
    }

    [HttpGet("{roleId}")]
    public async Task<ActionResult<RoleDto>> GetRole(string roleId)
    {
        var role = await _roleService.GetRoleByIdAsync(roleId);
        if (role == null)
            return NotFound();
        
        return Ok(role);
    }

    [HttpPost]
    public async Task<ActionResult> CreateRole([FromBody] CreateRoleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Role name is required");

        var success = await _roleService.CreateRoleAsync(request.Name, request.Description);
        if (!success)
            return BadRequest("Role already exists or could not be created");

        return CreatedAtAction(nameof(GetRole), new { roleId = request.Name }, request);
    }

    [HttpPut("{roleId}")]
    public async Task<ActionResult> UpdateRole(string roleId, [FromBody] UpdateRoleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest("Role name is required");

        var success = await _roleService.UpdateRoleAsync(roleId, request.Name, request.Description);
        if (!success)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{roleId}")]
    public async Task<ActionResult> DeleteRole(string roleId)
    {
        var success = await _roleService.DeleteRoleAsync(roleId);
        if (!success)
            return BadRequest("Role not found or cannot be deleted (has users assigned)");

        return NoContent();
    }

    [HttpPost("assign")]
    public async Task<ActionResult> AssignRole([FromBody] AssignRoleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.RoleName))
            return BadRequest("UserId and RoleName are required");

        var success = await _roleService.AssignRoleToUserAsync(request.UserId, request.RoleName);
        if (!success)
            return BadRequest("User not found, role not found, or user already has this role");

        return NoContent();
    }

    [HttpDelete("assign")]
    public async Task<ActionResult> RemoveRole([FromBody] AssignRoleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.RoleName))
            return BadRequest("UserId and RoleName are required");

        var success = await _roleService.RemoveRoleFromUserAsync(request.UserId, request.RoleName);
        if (!success)
            return BadRequest("User not found, role not found, or user doesn't have this role");

        return NoContent();
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<string>>> GetUserRoles(string userId)
    {
        var roles = await _roleService.GetUserRolesAsync(userId);
        return Ok(roles);
    }

    [HttpGet("{roleName}/users")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersInRole(string roleName)
    {
        var users = await _roleService.GetUsersInRoleAsync(roleName);
        return Ok(users);
    }
}