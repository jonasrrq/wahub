using System.Diagnostics;
using System.Security.Claims;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using WaHub.Data;
using WaHub.Shared.Models;

namespace WaHub.Authentication;

public class PersistAuthStateProvider : RevalidatingServerAuthenticationStateProvider
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly PersistentComponentState _state;
    private readonly IdentityOptions _options;
    private readonly PersistingComponentStateSubscription _subscription;

    private Task<AuthenticationState>? _authenticationStateTask;

    public PersistAuthStateProvider(ILoggerFactory loggerFactory,
        IServiceScopeFactory serviceScopeFactory,
        PersistentComponentState persistentComponent,
        IOptions<IdentityOptions> options)
        : base(loggerFactory)
    {
        _scopeFactory = serviceScopeFactory;
        _state = persistentComponent;
        _options = options.Value;

        AuthenticationStateChanged += OnAuthenticationStateChanged;
        _subscription = _state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
    }



    protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState,
        CancellationToken cancellationToken)
    {
        // Obtenga el administrador de usuarios de un nuevo ámbito para garantizar que obtenga datos nuevos
        await using var scope = _scopeFactory.CreateAsyncScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        return await ValidateAuthenticationStampAsync(userManager, authenticationState.User);
    }

    private async Task<bool> ValidateAuthenticationStampAsync(UserManager<ApplicationUser> userManager,
        ClaimsPrincipal principal)
    {
        var user = await userManager.GetUserAsync(principal);

        if (user is null)
            return false;
        else if (!userManager.SupportsUserSecurityStamp)
            return true;
        else
        {
            var principalStamp = principal.FindFirstValue(_options.ClaimsIdentity.SecurityStampClaimType);
            var userStamp = await userManager.GetSecurityStampAsync(user);
            return principalStamp == userStamp;
        }
    }

    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

    private async Task OnPersistingAsync()
    {
        if (_authenticationStateTask is null)
            throw new UnreachableException($"Authentication state not set in {nameof(OnPersistingAsync)}().");

        var authenticationState = await _authenticationStateTask;
        var principal = authenticationState.User;

        if (principal.Identity?.IsAuthenticated == true)
        {
            // Obtenga lo que desea mostrar en la interfaz de usuario del cliente
            var fullName = principal.Claims.First(_ => _.Type == "Fullname").Value;
            var email = principal.FindFirst(_options.ClaimsIdentity.EmailClaimType)?.Value;

            if (fullName != null && email != null)
            {
                _state.PersistAsJson(nameof(UserSession), new UserSession
                {
                    FullName = fullName,
                    Email = email
                });
            }
        }
    }

    private void OnAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        _authenticationStateTask = task;
    }

    protected override void Dispose(bool disposing)
    {
        _subscription.Dispose();
        AuthenticationStateChanged -= OnAuthenticationStateChanged;
        base.Dispose(disposing);
    }
}
