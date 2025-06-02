using System.Security.Claims;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;

using WaHub.Models;

namespace WaHub.Services
{
    public class AuthService
    {
        private readonly ProtectedLocalStorage _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(ProtectedLocalStorage localStorage, AuthenticationStateProvider authStateProvider)
        {
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            try
            {
                var result = await _localStorage.GetAsync<string>("token");
                return result.Success && !string.IsNullOrEmpty(result.Value);
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
                var result = await _localStorage.GetAsync<string>("token");
                return result.Success ? result.Value : null;
            }
            catch
            {
                return null;
            }
        }

        public async Task SetTokenAsync(string token)
        {
            await _localStorage.SetAsync("token", token);
            // Extraer información del usuario del token (simulado)
            var userInfo = ExtractUserInfoFromToken(token);
            await _localStorage.SetAsync("userInfo", userInfo);

            // Notificar cambio de estado de autenticación
            if (_authStateProvider is CustomAuthStateProvider customProvider)
            {
                await customProvider.NotifyAuthenticationStateChangedAsync();
            }
        }

        public async Task<UserInfo?> GetUserInfoAsync()
        {
            try
            {
                var result = await _localStorage.GetAsync<UserInfo>("userInfo");
                return result.Success ? result.Value : null;
            }
            catch
            {
                return null;
            }
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
            await _localStorage.DeleteAsync("token");
            await _localStorage.DeleteAsync("userInfo");
            // Notificar cambio de estado de autenticación
            if (_authStateProvider is CustomAuthStateProvider customProvider)
            {
                await customProvider.NotifyAuthenticationStateChangedAsync();
            }
        }
    }

    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _localStorage;
        private readonly NavigationManager _navigationManager;
        private readonly IJSRuntime _jsRuntime;

        public CustomAuthStateProvider(
            ProtectedLocalStorage localStorage,
            NavigationManager navigationManager,
            IJSRuntime jsRuntime)
        {
            _localStorage = localStorage;
            _navigationManager = navigationManager;
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Detectar prerenderizado
            if (_jsRuntime is IJSInProcessRuntime)
            {
                // Estamos en el lado cliente, es seguro usar JS interop
                try
                {
                    var tokenResult = await _localStorage.GetAsync<string>("token");
                    var userInfoResult = await _localStorage.GetAsync<UserInfo>("userInfo");

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
                    // Error al acceder al localStorage
                }
            }

            // Si estamos prerenderizando, devolvemos un usuario no autenticado
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public Task NotifyAuthenticationStateChangedAsync()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return Task.CompletedTask;
        }
    }
}
