using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using WaHub.Authentication;
using WaHub.Data;
using WaHub.Shared.Models;
using WaHub.Shared.Services;

namespace WaHub.Services;

public static class ServiceRegistration
{
    public static void AddServices(WebApplicationBuilder builder)
    {
         builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<ApiService>();
        builder.Services.AddScoped<NavigationService>();
        builder.Services.AddScoped<LocalizationService>();
        builder.Services.AddScoped<NotificationService>();
        builder.Services.AddHttpContextAccessor(); // Agregar para acceder al HttpContext
        //builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        //builder.Services.AddScoped<ICrossPlatformStorageService, CrossPlatformStorageService>();

        // Add localization services
        builder.Services.AddLocalization();

        // Add HTTP client with auth handler
        //builder.Services.AddScoped<AuthHeaderHandler>();
        //builder.Services.AddHttpClient("AuthHttpClient")
        //    .AddHttpMessageHandler<AuthHeaderHandler>();

        // Register a named client without auth for public endpoints if needed
        builder.Services.AddHttpClient("PublicHttpClient");

        // Configuraci�n de autenticaci�n JWT para APIs
        //builder.Services.AddJwtAuthentication(builder.Configuration);

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        }).AddApplicationCookie();

        builder.Services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<AuthenticationStateProvider, PersistAuthStateProvider>();
    }
}
