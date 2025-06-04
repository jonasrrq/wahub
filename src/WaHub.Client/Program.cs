using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore()
    .AddCascadingAuthenticationState()
    .AddAuthenticationStateDeserialization();

//builder.Services.AddSingleton<AuthenticationStateProvider, ClientAuthStateProvider>();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
