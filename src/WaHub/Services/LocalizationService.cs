using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Localization;

namespace WaHub.Services;

public class LocalizationService
{
    private readonly IStringLocalizer<LocalizationService> _localizer;
    private readonly ProtectedLocalStorage _localStorage;
    private string _currentLanguage = "es";

    public LocalizationService(IStringLocalizer<LocalizationService> localizer, ProtectedLocalStorage localStorage)
    {
        _localizer = localizer;
        _localStorage = localStorage;
        _ = InitializeLanguageAsync();
    }

    public event Action? LanguageChanged;

    public string CurrentLanguage => _currentLanguage;

    public string GetString(string key, string defaultValue = "")
    {
        var localizedString = _localizer[key];
        return localizedString.ResourceNotFound ? defaultValue : localizedString.Value;
    }

    public async Task SetLanguageAsync(string language)
    {
        _currentLanguage = language;
        await _localStorage.SetAsync("language", language);
        LanguageChanged?.Invoke();
    }

    private async Task InitializeLanguageAsync()
    {
        try
        {
            var result = await _localStorage.GetAsync<string>("language");
            if (result.Success && !string.IsNullOrEmpty(result.Value))
            {
                _currentLanguage = result.Value;
            }
        }
        catch
        {
            _currentLanguage = "es"; // idioma por defecto
        }
    }

    // Métodos de conveniencia para las traducciones específicas
    public string T(string key, string defaultValue = "") => GetString(key, defaultValue);

    // Traducciones específicas que encontramos en el código Next.js
    public string UserName => T("user_name", "Jonas Requena");
    public string YourInstances => T("your_instances", "Tus Instancias");
    public string FreeTrial => T("free_trial", "Prueba Gratis");
    public string Subscription => T("subscription", "Suscripción");
    public string Documentation => T("documentation", "Documentación");
    public string Wabulk => T("wabulk", "WaBulk");
    public string ApiToken => T("api_token", "Token API");
    public string WebhookSettings => T("webhook_settings", "Configuración Webhook");
    public string Support => T("support", "Soporte");
    public string Profile => T("profile", "Perfil");
    public string Logout => T("logout", "Cerrar Sesión");
    public string Login => T("login", "Iniciar Sesión");
    public string HomeTitle => T("home_title", "WhatsApp API");
    public string ForDevelopers => T("for_developers", "para desarrolladores");
    public string HomeDescription => T("home_description", "Envía y recibe mensajes, gestiona chats, grupos y canales fácilmente con nuestra API de nivel empresarial. Paga por cuenta de WhatsApp, sin tarifas por mensaje.");
    public string StartFreeTrial => T("start_free_trial", "Comenzar prueba gratis");
    public string TrialInfo => T("trial_info", "3 días de prueba gratis • Sin tarjeta de crédito • Cancela cuando quieras");
}
