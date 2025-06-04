using WaHub.Authentication;
using WaHub.Components;
using WaHub.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


ServiceRegistration.AddServices(builder);

var app = builder.Build();

// Apply database migrations based on configuration
var autoMigrate = builder.Configuration.GetValue<bool>("Database:AutoMigrate", true);
if (autoMigrate && !app.Environment.IsDevelopment())
{
    await ServiceRegistration.ApplyMigrationsAsync(app);
}


ServiceRegistration.UseConfigurations(app);

app.Run();