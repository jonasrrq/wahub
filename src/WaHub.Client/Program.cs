using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using WaHub.Client.Services;
using WaHub.Shared.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore()
    .AddCascadingAuthenticationState();
    //.AddAuthenticationStateDeserialization(); // Removed as it's not standard for client app and caused error in .NET 8

//builder.Services.AddSingleton<AuthenticationStateProvider, ClientAuthStateProvider>();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//builder.Services.AddScoped<IApiAdminService, ApiAdminService>();


await builder.Build().RunAsync();
