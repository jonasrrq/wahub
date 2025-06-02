using System.Security.Claims;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace WaHub.Blazor.Services
{
    public class BlazorAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
    }

    public class BlazorAuthenticationSchemeHandler : AuthenticationHandler<BlazorAuthenticationSchemeOptions>
    {
        public BlazorAuthenticationSchemeHandler(IOptionsMonitor<BlazorAuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Siempre retorna éxito - la autenticación real se maneja en el AuthenticationStateProvider
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "BlazorUser"),
                new Claim(ClaimTypes.Authentication, "true")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            // Redirigir al login si no está autenticado
            Response.Redirect("/login");
            return Task.CompletedTask;
        }
    }
}
