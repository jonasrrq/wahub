using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

using WaHub.Authentication;
using WaHub.Client.Services;
using WaHub.Components;
using WaHub.Data;
using WaHub.Handlers;
using WaHub.Shared.Models;
using WaHub.Shared.Services;

namespace WaHub.Services;

public static class ServiceRegistration
{
    public static void AddServices(WebApplicationBuilder builder)
    {        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents()
            .AddAuthenticationStateSerialization();

        builder.Services.AddControllers(); // Habilitar controladores para API

        //builder.Services.AddScoped<AuthService>();        builder.Services.AddScoped<IApiService, ApiService>();
        builder.Services.AddScoped<IApiAdminService, ApiAdminService>();
        builder.Services.AddScoped<NavigationService>();
        builder.Services.AddScoped<ILocalizationService, LocalizationService>();
        builder.Services.AddScoped<NotificationService>();
        builder.Services.AddScoped<IThemeService, ThemeService>();
        builder.Services.AddHttpContextAccessor(); // Agregar para acceder al HttpContext
        //builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
        //builder.Services.AddScoped<ICrossPlatformStorageService, CrossPlatformStorageService>();
        builder.Services.AddScoped<AuthenticationStateProvider, PersistAuthStateProvider>();

        // Add localization services
        builder.Services.AddLocalization();

        // Add HTTP client with auth handler
        builder.Services.AddScoped<AuthAdminHeaderHandler>();
        builder.Services.AddHttpClient("ApiAdminHttpClient", client =>
                {
                    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UriApi") ?? "");
                })
                .AddHttpMessageHandler<AuthAdminHeaderHandler>();

        //.AddHttpClient("ApiHttpClient")//

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        }).AddApplicationCookie();

        builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        builder.Services.AddCascadingAuthenticationState();

    }

    public static void UseConfigurations(WebApplication app)
    {
        var supportedCultures = SupportedCultures.All;

        var localizationOptions = new RequestLocalizationOptions()
        {
            DefaultRequestCulture = new RequestCulture(SupportedCultures.Default),
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures,
            FallBackToParentCultures = true,
            FallBackToParentUICultures = true,
            RequestCultureProviders = new List<IRequestCultureProvider>
            {
                new AcceptLanguageHeaderRequestCultureProvider(),
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider()
            }
        };
        app.UseRequestLocalization(localizationOptions);
     

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }        //app.MapControllers();
        app.MapControllers(); // Habilitar controladores para API

        //app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();

        // Middleware de autenticaci�n y autorizaci�n para APIs protegidas
        //app.UseAuthentication();
        //app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(WaHub.Client._Imports).Assembly);

        app.MapSingOutEndpoint();

        app.UseStatusCodePagesWithRedirects("/404");
        //app.UseStatusCodePagesWithReExecute("/error/{0}");

    }

    public static async Task ApplyMigrationsAsync(WebApplication webApp)
    {
        using var scope = webApp.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = webApp.Services.GetRequiredService<ILogger<ApplicationDbContext>>();
        var configuration = webApp.Services.GetRequiredService<IConfiguration>();

        var failOnError = configuration.GetValue<bool>("Database:FailOnMigrationError", false);

        try
        {
            logger.LogInformation("Starting database migration process...");

            // Apply migrations (this will create the database if it doesn't exist)
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                logger.LogInformation($"Found {pendingMigrations.Count()} pending migrations. Applying...");
                foreach (var migration in pendingMigrations)
                {
                    logger.LogInformation($"Pending migration: {migration}");
                }

                await context.Database.MigrateAsync();
                logger.LogInformation("All database migrations applied successfully.");
            }
            else
            {
                logger.LogInformation("No pending migrations found. Database is up to date.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while applying database migrations.");

            if (failOnError)
            {
                logger.LogCritical("Application startup will be halted due to migration errors and FailOnMigrationError=true");
                throw; // This will prevent the application from starting
            }

            logger.LogWarning("Application will continue to start despite migration errors. Please check the database manually.");
        }
    }
}
