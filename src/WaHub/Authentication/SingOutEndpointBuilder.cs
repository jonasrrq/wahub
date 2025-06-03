using System.Security.Claims;

using Microsoft.AspNetCore.Identity;

using WaHub.Data;

namespace WaHub.Authentication;

internal static class SingOutEndpointBuilder
{
    public static IEndpointConventionBuilder MapSingOutEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var accountGroup = endpoints.MapGroup("/Account");
        accountGroup.MapPost("/Logount", async (
            ClaimsPrincipal User,
            SignInManager<ApplicationUser> singInManager) =>
        {
            await singInManager.SignOutAsync();
            return TypedResults.LocalRedirect("/");
        });

        return accountGroup;
    }
}
