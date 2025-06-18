using Xunit;
using Moq;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaHub.Services; // Para RoleService
using WaHub.Data;    // Para ApplicationRole, ApplicationUser
using WaHub.Shared.Services; // Para IRoleService
using WaHub.Shared.Models; // Para Response
using MockQueryable.Moq; // Asegurar que este using esté presente

// Helper para mockear UserManager y RoleManager
public static class IdentityMockHelpers
{
    public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : ApplicationUser // Ajustado a ApplicationUser para acceder a FirstName/LastName
    {
        var store = new Mock<IUserStore<TUser>>();
        var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        mgr.Object.UserValidators.Add(new UserValidator<TUser>());
        mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

        mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
        mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
        mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
        // Usar BuildMock() para Users también si se usa ToListAsync() sobre él en el servicio, aunque no en RoleService directamente.
        mgr.Setup(x => x.Users).Returns(ls.AsQueryable().BuildMock());
        mgr.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((string id) => ls.FirstOrDefault(u => u.Id == id));
        mgr.Setup(x => x.GetRolesAsync(It.IsAny<TUser>())).ReturnsAsync((TUser user) => {
            // Simular la obtención de roles. Esto necesitaría una lógica más compleja si se prueba GetUserRolesAsync aquí.
            // Por ahora, para RoleService, UserManager se usa principalmente para GetUsersInRoleAsync y AddToRoleAsync/RemoveFromRoleAsync.
            return new List<string>();
        });
        mgr.Setup(x => x.IsInRoleAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync((TUser user, string role) => {
            // Lógica simple de simulación
            var appUser = user as ApplicationUser;
            // Esto es muy simplificado, necesitaría una forma de rastrear roles asignados a usuarios en el mock.
            // Para las pruebas actuales de RoleService, esto podría ser suficiente si no se prueba profundamente la asignación.
            return _userRoles.TryGetValue(appUser.Id, out var roles) && roles.Contains(role);
        });
         mgr.Setup(x => x.AddToRoleAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync((TUser user, string role) => {
            var appUser = user as ApplicationUser;
            if (!_userRoles.ContainsKey(appUser.Id)) _userRoles[appUser.Id] = new List<string>();
            if (!_userRoles[appUser.Id].Contains(role)) _userRoles[appUser.Id].Add(role);
            return IdentityResult.Success;
        });
        mgr.Setup(x => x.RemoveFromRoleAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync((TUser user, string role) => {
            var appUser = user as ApplicationUser;
            if (_userRoles.TryGetValue(appUser.Id, out var roles) && roles.Contains(role))
            {
                roles.Remove(role);
            }
            return IdentityResult.Success;
        });
        mgr.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>())).ReturnsAsync((string roleName) => {
            return ls.Where(u => {
                var appUser = u as ApplicationUser;
                return _userRoles.TryGetValue(appUser.Id, out var roles) && roles.Contains(roleName);
            }).ToList();
        });

        return mgr;
    }

    // Necesitamos una forma de mockear la asignación de roles a usuarios para GetUsersInRoleAsync
    private static Dictionary<string, List<string>> _userRoles = new Dictionary<string, List<string>>();


    public static Mock<RoleManager<TRole>> MockRoleManager<TRole>(List<TRole> ls) where TRole : ApplicationRole // Ajustado a ApplicationRole
    {
        var store = new Mock<IRoleStore<TRole>>();
        var mgr = new Mock<RoleManager<TRole>>(store.Object, null, null, null, null);
        mgr.Object.RoleValidators.Add(new RoleValidator<TRole>());

        mgr.Setup(x => x.Roles).Returns(ls.AsQueryable().BuildMock()); // Usar BuildMock aquí
        mgr.Setup(x => x.CreateAsync(It.IsAny<TRole>())).ReturnsAsync(IdentityResult.Success).Callback<TRole>(r => { if (ls.All(existingRole => existingRole.Name != r.Name)) ls.Add(r); });
        mgr.Setup(x => x.DeleteAsync(It.IsAny<TRole>())).ReturnsAsync(IdentityResult.Success).Callback<TRole>(r => ls.RemoveAll(role => role.Name == r.Name));
        mgr.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync((string roleName) => ls.Any(r => r.Name == roleName));
        mgr.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((string roleName) => ls.FirstOrDefault(r => r.Name == roleName));

        return mgr;
    }
     public static void ClearUserRoles() {_userRoles.Clear();}
}

public class RoleServiceTests
{
    private readonly Mock<RoleManager<ApplicationRole>> _mockRoleManager;
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly IRoleService _roleService;
    private List<ApplicationRole> _rolesStore;
    private List<ApplicationUser> _usersStore;

    public RoleServiceTests()
    {
        // Resetear stores y mocks para cada prueba para evitar interferencias
        _rolesStore = new List<ApplicationRole>();
        _usersStore = new List<ApplicationUser>();
        IdentityMockHelpers.ClearUserRoles(); // Limpiar los roles de usuario mockeados

        _mockRoleManager = IdentityMockHelpers.MockRoleManager(_rolesStore);
        _mockUserManager = IdentityMockHelpers.MockUserManager(_usersStore);

        _roleService = new RoleService(_mockRoleManager.Object, _mockUserManager.Object);
    }

    private void ResetMocksAndStores() // Método para asegurar estado limpio si es necesario entre arranges
    {
        _rolesStore.Clear();
        _usersStore.Clear();
        IdentityMockHelpers.ClearUserRoles();

        // Re-configurar mocks para que apunten a las listas limpias y actualizadas con BuildMock()
        _mockRoleManager.Setup(x => x.Roles).Returns(_rolesStore.AsQueryable().BuildMock());
        _mockUserManager.Setup(x => x.Users).Returns(_usersStore.AsQueryable().BuildMock());
    }


    [Fact]
    public async Task GetAllRolesAsync_ShouldReturnAllRoleNames()
    {
        // Arrange
        ResetMocksAndStores(); // Asegurar estado limpio
        _rolesStore.Add(new ApplicationRole("Admin"));
        _rolesStore.Add(new ApplicationRole("User"));
        // No es necesario re-mockear _mockRoleManager.Roles aquí si el constructor ya lo hace con la lista correcta
        // o si ResetMocksAndStores lo hace.

        // Act
        var response = await _roleService.GetAllRolesAsync();

        // Assert
        Assert.True(response.Success, response.Message);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data.Count);
        Assert.Contains("Admin", response.Data);
        Assert.Contains("User", response.Data);
    }

    [Fact]
    public async Task GetAllRolesAsync_ShouldReturnEmptyList_WhenNoRolesExist()
    {
        // Arrange
        ResetMocksAndStores(); // Asegurar estado limpio, _rolesStore estará vacía

        // Act
        var response = await _roleService.GetAllRolesAsync();

        // Assert
        Assert.True(response.Success, response.Message);
        Assert.NotNull(response.Data);
        Assert.Empty(response.Data);
    }

    [Fact]
    public async Task CreateRoleAsync_ShouldCreateRole_WhenRoleDoesNotExist()
    {
        // Arrange
        ResetMocksAndStores();
        var roleName = "Tester";
        // RoleExistsAsync ya está mockeado por IdentityMockHelpers para devolver false si no está en _rolesStore
        // CreateAsync también está mockeado para añadir a _rolesStore

        // Act
        var response = await _roleService.CreateRoleAsync(roleName);

        // Assert
        Assert.True(response.Success, response.Message);
        Assert.Equal(roleName, response.Data);
        Assert.Contains(_rolesStore, r => r.Name == roleName);
        _mockRoleManager.Verify(x => x.CreateAsync(It.Is<ApplicationRole>(r => r.Name == roleName)), Times.Once);
    }

    [Fact]
    public async Task CreateRoleAsync_ShouldReturnFalse_WhenRoleAlreadyExists()
    {
        // Arrange
        ResetMocksAndStores();
        var roleName = "Admin";
        _rolesStore.Add(new ApplicationRole(roleName)); // Pre-existente
        // RoleExistsAsync mockeado por IdentityMockHelpers reflejará esto

        // Act
        var response = await _roleService.CreateRoleAsync(roleName);

        // Assert
        Assert.False(response.Success);
        Assert.Contains("ya existe", response.Message);
        // CreateAsync no debería ser llamado si el rol ya existe.
        // El mock de CreateAsync en IdentityMockHelpers añade al store, así que verificamos que no se llame una segunda vez.
        // Times.Never podría ser muy estricto si el setup inicial lo llama. Mejor verificar el contenido del store o el número de llamadas.
        _mockRoleManager.Verify(x => x.CreateAsync(It.Is<ApplicationRole>(r => r.Name == roleName)), Times.Never);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task CreateRoleAsync_ShouldReturnFalse_WhenRoleNameIsInvalid(string invalidRoleName)
    {
        // Arrange
        ResetMocksAndStores();

        // Act
        var response = await _roleService.CreateRoleAsync(invalidRoleName);

        // Assert
        Assert.False(response.Success);
        Assert.Contains("no puede estar vacío", response.Message);
        _mockRoleManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationRole>()), Times.Never);
    }

    // Aquí se podrían añadir más pruebas para DeleteRoleAsync, GetUserRolesAsync, AddUserToRoleAsync, RemoveUserFromRoleAsync
    // Por ejemplo, para DeleteRoleAsync:
    [Fact]
    public async Task DeleteRoleAsync_ShouldDeleteRole_WhenRoleExistsAndNoUsers()
    {
        // Arrange
        ResetMocksAndStores();
        var roleName = "ToDelete";
        _rolesStore.Add(new ApplicationRole(roleName) { NormalizedName = roleName.ToUpper() });
        // GetUsersInRoleAsync debe devolver lista vacía para este rol
        _mockUserManager.Setup(um => um.GetUsersInRoleAsync(roleName)).ReturnsAsync(new List<ApplicationUser>());

        // Act
        var response = await _roleService.DeleteRoleAsync(roleName);

        // Assert
        Assert.True(response.Success, response.Message);
        Assert.DoesNotContain(_rolesStore, r => r.Name == roleName);
        _mockRoleManager.Verify(x => x.DeleteAsync(It.Is<ApplicationRole>(r => r.Name == roleName)), Times.Once);
    }

    [Fact]
    public async Task DeleteRoleAsync_ShouldReturnFalse_WhenRoleDoesNotExist()
    {
        // Arrange
        ResetMocksAndStores();
        var roleName = "NonExistent";

        // Act
        var response = await _roleService.DeleteRoleAsync(roleName);

        // Assert
        Assert.False(response.Success);
        Assert.Contains("no fue encontrado", response.Message);
        _mockRoleManager.Verify(x => x.DeleteAsync(It.IsAny<ApplicationRole>()), Times.Never);
    }

    [Fact]
    public async Task DeleteRoleAsync_ShouldReturnFalse_WhenRoleHasUsersAssigned()
    {
        // Arrange
        ResetMocksAndStores();
        var roleName = "RoleWithUsers";
         _rolesStore.Add(new ApplicationRole(roleName) { NormalizedName = roleName.ToUpper() });
        var user = new ApplicationUser { Id = "testuser", UserName = "test", FirstName = "Test", LastName = "User" };
        _usersStore.Add(user);
        // Simular que el usuario está en el rol
        IdentityMockHelpers._userRoles[user.Id] = new List<string> { roleName };


        // Act
        var response = await _roleService.DeleteRoleAsync(roleName);

        // Assert
        Assert.False(response.Success);
        Assert.Contains("tiene usuarios asignados", response.Message);
        _mockRoleManager.Verify(x => x.DeleteAsync(It.IsAny<ApplicationRole>()), Times.Never);
    }
}
EOL
