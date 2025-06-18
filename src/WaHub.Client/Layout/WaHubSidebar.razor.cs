using WaHub.Client.Services;
using WaHub.Shared.Models.Resouces;
using WaHub.Shared.Services;


namespace WaHub.Client.Layout;

public partial class WaHubSidebar
{
    private readonly NavigationService _navigation;
    public readonly ILocalizationService _localization;
    private MenuItem[] mainMenu = Array.Empty<MenuItem>();
    private MenuItem[] bottomMenu = Array.Empty<MenuItem>();
    private bool isUserMenuOpen = false;



    public WaHubSidebar(NavigationService Navigation,
        ILocalizationService Localization)
    {
        _navigation = Navigation;
        _localization = Localization;
        // Constructor vacío, la inicialización se realiza en OnInitialized
    }

    protected override void OnInitialized()
    {
        mainMenu = new[]
        {
        new MenuItem { Label = _localization.GetString(ResourceKeys.WaHubSidebar_YourInstances), Icon = "bi-stack", Href = "/admin/instances", Active = true },
        new MenuItem { Label = _localization.GetString(ResourceKeys.WaHubSidebar_FreeTrial), Icon = "bi-lightning-fill", Href = "/admin/trial", Badge = "TRIAL" },
        new MenuItem { Label = _localization.GetString(ResourceKeys.WaHubSidebar_Subscription), Icon = "bi-credit-card", Href = "/admin/subscription" },
        new MenuItem { Label = _localization.GetString(ResourceKeys.WaHubSidebar_Documentation), Icon = "bi-book", Href = "/admin/documentation" },
        new MenuItem { Label = _localization.GetString(ResourceKeys.WaHubSidebar_WaBulk), Icon = "bi-lightning-fill", Href = "/admin/wabulk", Badge = "BETA" },
    };

        bottomMenu = new[]
        {
        new MenuItem { Label = _localization.GetString(ResourceKeys.WaHubSidebar_APIToken), Icon = "bi-key-fill", Href = "/admin/api-token" },
        new MenuItem { Label = _localization.GetString(ResourceKeys.WaHubSidebar_WebhookSettings), Icon = "bi-gear-fill", Href = "/admin/webhook-settings" },
        new MenuItem { Label = _localization.GetString(ResourceKeys.WaHubSidebar_Support), Icon = "bi-question-circle-fill", Href = "/admin/support" },
        };
    }

    private void NavigateToDashboard()
    {
        _navigation.Push("/admin/dashboard");
    }

    private void NavigateToPage(string href)
    {
        _navigation.Push(href);
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
}

public class MenuItem
{
    public string Label { get; set; } = "";
    public string Icon { get; set; } = "";
    public string Href { get; set; } = "";
    public bool Active { get; set; }
    public string Badge { get; set; } = "";
}
