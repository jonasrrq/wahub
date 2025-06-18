using Microsoft.AspNetCore.Identity;
using WaHub.Shared.Models;
using System.Collections.Generic;

namespace WaHub.Data;

public class ApplicationUser : IdentityUser
{
    public string? Avatar { get; set; }
    public string FullName { get; set; } = "";
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? CompanyName { get; set; }
    
    // Roles relationship
    public virtual ICollection<AppRole> Roles { get; set; } = new List<AppRole>();
}
