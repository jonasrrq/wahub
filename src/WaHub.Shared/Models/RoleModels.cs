namespace WaHub.Shared.Models;

public class RoleDto
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public int UserCount { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class UserDto
{
    public string Id { get; set; } = "";
    public string Email { get; set; } = "";
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string FullName { get; set; } = "";
    public string? CompanyName { get; set; }
    public string? Avatar { get; set; }
    public IList<string> Roles { get; set; } = new List<string>();
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; } = true;
}

public class CreateRoleRequest
{
    public string Name { get; set; } = "";
    public string? Description { get; set; }
}

public class UpdateRoleRequest
{
    public string Name { get; set; } = "";
    public string? Description { get; set; }
}

public class AssignRoleRequest
{
    public string UserId { get; set; } = "";
    public string RoleName { get; set; } = "";
}