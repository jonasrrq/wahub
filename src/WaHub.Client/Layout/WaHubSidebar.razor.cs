using Microsoft.AspNetCore.Components;

using WaHub.Client.Services;


namespace WaHub.Client.Layout;

public partial class WaHubSidebar
{
    [Inject]
    public NavigationService Navigation { get; set; } = default!;

    private MenuItem[] mainMenu = Array.Empty<MenuItem>();
    private MenuItem[] bottomMenu = Array.Empty<MenuItem>();
    private bool isUserMenuOpen = false;



    protected override void OnInitialized()
    {
        mainMenu = new[]
        {
            new MenuItem { Label = "Tus Instancias", Icon = "bi-stack", Href = "/admin/instances", Active = true },
            new MenuItem { Label = "Prueba Gratis", Icon = "bi-lightning-fill", Href = "/admin/trial", Badge = "TRIAL" },
            new MenuItem { Label = "Suscripción", Icon = "bi-credit-card", Href = "/admin/subscription" },
            new MenuItem { Label = "Documentación", Icon = "bi-book", Href = "/admin/documentation" },
            new MenuItem { Label = "WaBulk", Icon = "bi-lightning-fill", Href = "/admin/wabulk", Badge = "BETA" },
        };

        bottomMenu = new[]
        {
            new MenuItem { Label = "API Token", Icon = "bi-key-fill", Href = "/admin/api-token" },
            new MenuItem { Label = "Webhook Settings", Icon = "bi-gear-fill", Href = "/admin/webhook-settings" },
            new MenuItem { Label = "Soporte", Icon = "bi-question-circle-fill", Href = "/admin/support" },
        };
    }

    private void NavigateToDashboard()
    {
        Navigation.Push("/admin/dashboard");
    }

    private void NavigateToPage(string href)
    {
        Navigation.Push(href);
        isUserMenuOpen = false; // Cerrar menú al navegar
    }

    private void ToggleUserMenu()
    {
        isUserMenuOpen = !isUserMenuOpen;
    }

    // Cerrar menú al hacer clic fuera (se podría implementar con JavaScript)
    private void CloseUserMenu()
    {
        isUserMenuOpen = false;
    }

    private class MenuItem
    {
        public string Label { get; set; } = "";
        public string Icon { get; set; } = "";
        public string Href { get; set; } = "";
        public bool Active { get; set; }
        public string Badge { get; set; } = "";
    }
}

