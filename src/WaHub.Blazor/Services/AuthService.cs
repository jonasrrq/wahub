using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;

namespace WaHub.Blazor.Services
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
        }

        public async Task LogoutAsync()
        {
            await _localStorage.DeleteAsync("token");
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

        public CustomAuthStateProvider(ProtectedLocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var result = await _localStorage.GetAsync<string>("token");
                
                if (result.Success && !string.IsNullOrEmpty(result.Value))
                {
                    // Crear claims del usuario autenticado
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name, "Jonas Requena"),
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
            }            // Usuario no autenticado
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public Task NotifyAuthenticationStateChangedAsync()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return Task.CompletedTask;
        }
    }
}
