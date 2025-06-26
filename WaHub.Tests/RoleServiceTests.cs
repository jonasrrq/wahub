using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WaHub.Data;
using WaHub.Services;
using Xunit;

namespace WaHub.Tests;

public class RoleServiceTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly RoleService _roleService;

    public RoleServiceTests()
    {
        // Create in-memory database
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);

        // Create stores and managers
        var userStore = new UserStore<ApplicationUser, IdentityRole, ApplicationDbContext>(_context);
        var roleStore = new RoleStore<IdentityRole>(_context);
        
        _userManager = new UserManager<ApplicationUser>(userStore, null!, null!, null!, null!, null!, null!, null!, null!);
        _roleManager = new RoleManager<IdentityRole>(roleStore, null!, null!, null!, null!);
        
        _roleService = new RoleService(_roleManager, _userManager);

        // Ensure database is created
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task CreateRoleAsync_ValidRole_ReturnsTrue()
    {
        // Arrange
        var roleName = "TestRole";

        // Act
        var result = await _roleService.CreateRoleAsync(roleName);

        // Assert
        Assert.True(result);
        var role = await _roleManager.FindByNameAsync(roleName);
        Assert.NotNull(role);
        Assert.Equal(roleName, role.Name);
    }

    [Fact]
    public async Task CreateRoleAsync_DuplicateRole_ReturnsFalse()
    {
        // Arrange
        var roleName = "TestRole";
        await _roleService.CreateRoleAsync(roleName);

        // Act
        var result = await _roleService.CreateRoleAsync(roleName);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetAllRolesAsync_ReturnsAllRoles()
    {
        // Arrange
        await _roleService.CreateRoleAsync("Role1");
        await _roleService.CreateRoleAsync("Role2");

        // Act
        var roles = await _roleService.GetAllRolesAsync();

        // Assert
        Assert.Equal(2, roles.Count());
        Assert.Contains(roles, r => r.Name == "Role1");
        Assert.Contains(roles, r => r.Name == "Role2");
    }

    [Fact]
    public async Task DeleteRoleAsync_ExistingRoleWithoutUsers_ReturnsTrue()
    {
        // Arrange
        await _roleService.CreateRoleAsync("TestRole");
        var role = await _roleManager.FindByNameAsync("TestRole");

        // Act
        var result = await _roleService.DeleteRoleAsync(role!.Id);

        // Assert
        Assert.True(result);
        var deletedRole = await _roleManager.FindByNameAsync("TestRole");
        Assert.Null(deletedRole);
    }

    [Fact]
    public async Task AssignRoleToUserAsync_ValidUserAndRole_ReturnsTrue()
    {
        // Arrange
        var user = new ApplicationUser 
        { 
            UserName = "testuser@example.com", 
            Email = "testuser@example.com",
            FirstName = "Test",
            LastName = "User",
            FullName = "Test User"
        };
        await _userManager.CreateAsync(user);
        await _roleService.CreateRoleAsync("TestRole");

        // Act
        var result = await _roleService.AssignRoleToUserAsync(user.Id, "TestRole");

        // Assert
        Assert.True(result);
        var isInRole = await _userManager.IsInRoleAsync(user, "TestRole");
        Assert.True(isInRole);
    }

    [Fact]
    public async Task GetUserRolesAsync_ReturnsCorrectRoles()
    {
        // Arrange
        var user = new ApplicationUser 
        { 
            UserName = "testuser@example.com", 
            Email = "testuser@example.com",
            FirstName = "Test",
            LastName = "User",
            FullName = "Test User"
        };
        await _userManager.CreateAsync(user);
        await _roleService.CreateRoleAsync("Role1");
        await _roleService.CreateRoleAsync("Role2");
        await _roleService.AssignRoleToUserAsync(user.Id, "Role1");
        await _roleService.AssignRoleToUserAsync(user.Id, "Role2");

        // Act
        var roles = await _roleService.GetUserRolesAsync(user.Id);

        // Assert
        Assert.Equal(2, roles.Count());
        Assert.Contains("Role1", roles);
        Assert.Contains("Role2", roles);
    }

    [Fact]
    public async Task UserIsInRoleAsync_UserInRole_ReturnsTrue()
    {
        // Arrange
        var user = new ApplicationUser 
        { 
            UserName = "testuser@example.com", 
            Email = "testuser@example.com",
            FirstName = "Test",
            LastName = "User",
            FullName = "Test User"
        };
        await _userManager.CreateAsync(user);
        await _roleService.CreateRoleAsync("TestRole");
        await _roleService.AssignRoleToUserAsync(user.Id, "TestRole");

        // Act
        var result = await _roleService.UserIsInRoleAsync(user.Id, "TestRole");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task UserIsInRoleAsync_UserNotInRole_ReturnsFalse()
    {
        // Arrange
        var user = new ApplicationUser 
        { 
            UserName = "testuser@example.com", 
            Email = "testuser@example.com",
            FirstName = "Test",
            LastName = "User",
            FullName = "Test User"
        };
        await _userManager.CreateAsync(user);
        await _roleService.CreateRoleAsync("TestRole");

        // Act
        var result = await _roleService.UserIsInRoleAsync(user.Id, "TestRole");

        // Assert
        Assert.False(result);
    }

    public void Dispose()
    {
        _context.Dispose();
        _userManager.Dispose();
        _roleManager.Dispose();
    }
}