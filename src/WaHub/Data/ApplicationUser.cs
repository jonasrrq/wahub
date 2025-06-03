using Microsoft.AspNetCore.Identity;

namespace WaHub.Data;

public class ApplicationUser : IdentityUser
{
    public string? Avatar { get; set; }
    public string FullName { get; set; } = "";
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? CompanyName { get; set; }
}
