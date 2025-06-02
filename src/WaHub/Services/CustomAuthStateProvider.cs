using System.Security.Claims;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;

using WaHub.Shared.Models;

namespace WaHub.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ICrossPlatformStorageService _storageService;

    public CustomAuthStateProvider(ICrossPlatformStorageService storageService)
    {
        _storageService = storageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Si no estamos en navegador (es prerenderizado), no intentes acceder a LocalStorage
        if (!OperatingSystem.IsBrowser())
        {
            // Usuario no autenticado durante prerenderizado
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        try
        {
            var tokenResult = await _storageService.GetAsync<string>("token");
            var userInfoResult = await _storageService.GetAsync<UserInfo>("userInfo");

            if (tokenResult.Success && !string.IsNullOrEmpty(tokenResult.Value) &&
                userInfoResult.Success && userInfoResult.Value != null)
            {
                var userInfo = userInfoResult.Value;

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, userInfo.Name),
                    new Claim(ClaimTypes.Email, userInfo.Email),
                    new Claim(ClaimTypes.Role, userInfo.Role),
                    new Claim("avatar", userInfo.Avatar),
                    new Claim(ClaimTypes.Authentication, "true")
                };

                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
        }
        catch
        {
            // Error al acceder al almacenamiento
        }

        // Usuario no autenticado por defecto
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public Task NotifyAuthenticationStateChangedAsync()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return Task.CompletedTask;
    }
}
