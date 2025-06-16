using Microsoft.AspNetCore.Components.Authorization;
using WaHub.Shared.Models;

namespace WaHub.Services;

public class AuthService
{
    
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthService(AuthenticationStateProvider authStateProvider)
    {
       
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        try
        {
            //var result = await _storageService.GetAsync<string>("token");
            //return result.Success && !string.IsNullOrEmpty(result.Value);
            return await Task.FromResult(true);
        }
        catch
        {
            return false;
        }
    }

    public async Task<string?> GetTokenAsync()
    {
        try
        {
            //var result = await _storageService.GetAsync<string>("token");
            //return result.Success ? result.Value : null;
            return await Task.FromResult("true");
        }
        catch
        {
            return null;
        }
    }

    public async Task SetTokenAsync(string token)
    {
        //await _storageService.SetAsync("token", token);
        //// Extraer información del usuario del token (simulado)
        //var userInfo = ExtractUserInfoFromToken(token);
        //await _storageService.SetAsync("userInfo", userInfo);

        //// Notificar cambio de estado de autenticación
        //if (_authStateProvider is CustomAuthStateProvider customProvider)
        //{
        //    await customProvider.NotifyAuthenticationStateChangedAsync();
        //}
        await Task.CompletedTask;
    }

    public async Task<UserInfo?> GetUserInfoAsync()
    {
       return await Task.FromResult<UserInfo>(new UserInfo());
        //try
        //{
        //    var result = await _storageService.GetAsync<UserInfo>("userInfo");
        //    return result.Success ? result.Value : null;
        //}
        //catch
        //{
        //    return null;
        //}
    }

    private UserInfo ExtractUserInfoFromToken(string token)
    {
        // Simulación de extracción de información del token
        var isAdmin = token.Contains("admin");
        return new UserInfo
        {
            Name = isAdmin ? "Administrador" : "Usuario",
            Email = isAdmin ? "admin@wahub.com" : "user@wahub.com",
            Role = isAdmin ? "Admin" : "User",
            Avatar = isAdmin ? "👑" : "👤"
        };
    }

    public async Task LogoutAsync()
    {
        await Task.Delay(1);
        //await _storageService.DeleteAsync("token");
        //await _storageService.DeleteAsync("userInfo");
        //// Notificar cambio de estado de autenticación
        //if (_authStateProvider is CustomAuthStateProvider customProvider)
        //{
        //    await customProvider.NotifyAuthenticationStateChangedAsync();
        //}
    }
}
