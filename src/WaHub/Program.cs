using Microsoft.AspNetCore.Components.Authorization;

using WaHub.Blazor.Handlers;
using WaHub.Components;
using WaHub.Services;
using WaHub.Shared.Models;
using WaHub.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<NavigationService>();
builder.Services.AddScoped<LocalizationService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<ICrossPlatformStorageService, CrossPlatformStorageService>();

// Add localization services
builder.Services.AddLocalization();

// Add HTTP client with auth handler
builder.Services.AddScoped<AuthHeaderHandler>();
builder.Services.AddHttpClient("AuthHttpClient")
    .AddHttpMessageHandler<AuthHeaderHandler>();

// Register a named client without auth for public endpoints if needed
builder.Services.AddHttpClient("PublicHttpClient");

var app = builder.Build();

// Configure localization
//var supportedCultures = new[] { "es", "en" };
//var localizationOptions = new RequestLocalizationOptions()
//    .SetDefaultCulture(supportedCultures[0])
//    .AddSupportedCultures(supportedCultures)
//    .AddSupportedUICultures(supportedCultures);

//app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(WaHub.Client._Imports).Assembly);

app.Run();
