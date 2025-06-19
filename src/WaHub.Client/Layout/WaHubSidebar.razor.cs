using Microsoft.AspNetCore.Components.Authorization;
using WaHub.Client.Services;
using WaHub.Shared.Models.Resouces;
using WaHub.Shared.Services;


namespace WaHub.Client.Layout;

public partial class WaHubSidebar
{
    private readonly NavigationService _navigation;
    public readonly ILocalizationService _localization;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private MenuItem[] mainMenu = Array.Empty<MenuItem>();
    private MenuItem[] bottomMenu = Array.Empty<MenuItem>();
    private bool isUserMenuOpen = false;



    public WaHubSidebar(NavigationService Navigation,
        ILocalizationService Localization,
        AuthenticationStateProvider authenticationStateProvider)
    {
        _navigation = Navigation;
        _localization = Localization;
        _authenticationStateProvider = authenticationStateProvider;
        // Constructor vacío, la inicialización se realiza en OnInitialized
    }

    protected override async Task OnInitializedAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        var menuItems = new List<MenuItem>
        {
            new MenuItem { Label = _localization.GetString(ResourceKeys.WaHubSidebar_YourInstances), Icon = "bi-stack", Href = "/admin/instances", Active = true }, // Assuming Active logic might need review
            new MenuItem { Label = _localization.GetString(ResourceKeys.WaHubSidebar_FreeTrial), Icon = "bi-lightning-fill", Href = "/admin/trial", Badge = "TRIAL" },
            new MenuItem { Label = _localization.GetString(ResourceKeys.WaHubSidebar_Subscription), Icon = "bi-credit-card", Href = "/admin/subscription" },
        };

        // Conditionally add Role Management link
        // TODO: Replace "Admin" with a constant or configuration if available for role names
        if (user.IsInRole("Admin"))
        {
            menuItems.Add(new MenuItem { Label = _localization.GetString("Role Management"), Icon = "bi-people-fill", Href = "/admin/roles" });
        }

        // Add other existing main menu items
        menuItems.Add(new MenuItem { Label = _localization.GetString(ResourceKeys.WaHubSidebar_Documentation), Icon = "bi-book", Href = "/admin/documentation" });
        menuItems.Add(new MenuItem { Label = _localization.GetString(ResourceKeys.WaHubSidebar_WaBulk), Icon = "bi-lightning-fill", Href = "/admin/wabulk", Badge = "BETA" });

        mainMenu = menuItems.ToArray();

        // Bottom menu remains unchanged for now
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
