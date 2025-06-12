using Microsoft.Extensions.Localization;

using WaHub.Services;
using WaHub.Shared.Models;
using WaHub.Shared.Resources;
using WaHub.Shared.Services;

namespace WaHub.Services;

public class LocalizationService : ILocalizationService
{
    private readonly IStringLocalizer<Resource> _localizer;
    private string _currentLanguage = "es";
    public ExternalPagesLocalization ExternalPages { get; private set; } 

    public LocalizationService(IStringLocalizer<Resource> localizer)
    {
        _localizer = localizer;
        ExternalPages = new ExternalPagesLocalization(this);
        //LoadExternalPages();
        _ = InitializeLanguageAsync();
    }

    public event Action? LanguageChanged;

    public string CurrentLanguage => _currentLanguage;

    public string GetString(string key, string defaultValue = "")
    {
        // Forzar la cultura actual antes de buscar el recurso
        var culture = new System.Globalization.CultureInfo(_currentLanguage);
        System.Globalization.CultureInfo.CurrentUICulture = culture;
        System.Globalization.CultureInfo.CurrentCulture = culture;
        //var localizedString = _localizer[key];
        var localizedString = _localizer.GetString(key);
        return localizedString.ResourceNotFound ? defaultValue : localizedString.Value;
    }

    public async Task SetLanguageAsync(string language)
    {
        _currentLanguage = language;
        // Cambiar la cultura global para que los recursos .resx se recarguen correctamente
        var culture = new System.Globalization.CultureInfo(language);
        System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = culture;
        System.Globalization.CultureInfo.DefaultThreadCurrentCulture = culture;
        //LoadExternalPages(); // Recargar textos localizados al cambiar idioma

        ExternalPages.ClearCache(); // Limpiar caché para refrescar los valores

        await Task.CompletedTask; // Simular una tarea asíncrona si es necesario
        LanguageChanged?.Invoke();
    }


    // Métodos de conveniencia para las traducciones específicas
    public string T(string key, string defaultValue = "") => GetString(key, defaultValue);


    private async Task InitializeLanguageAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(_currentLanguage))
            {
                var browserLang = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                if (browserLang == "es" || browserLang == "en")
                    _currentLanguage = browserLang;
                else
                    _currentLanguage = "es"; // fallback

                await Task.CompletedTask; // Simular una tarea asíncrona si es necesario
            }
            //LoadExternalPages(); // Inicializar textos localizados
        }
        catch
        {
            _currentLanguage = "es"; // idioma por defecto
            //LoadExternalPages();
        }
    }


    //private void LoadExternalPages()
    //{
    //    ExternalPages = new ExternalPagesLocalization
    //    {
    //        UserName = GetString("user_name"),
    //        YourInstances = GetString("your_instances"),
    //        FreeTrial = GetString("free_trial"),
    //        Subscription = GetString("subscription"),
    //        Documentation = GetString("documentation"),
    //        Wabulk = GetString("wabulk"),
    //        ApiToken = GetString("api_token"),
    //        WebhookSettings = GetString("webhook_settings"),
    //        Support = GetString("support"),
    //        Profile = GetString("profile"),
    //        Logout = GetString("logout"),
    //        Login = GetString("login"),
    //        HomeTitle = GetString("home_title"),
    //        ForDevelopers = GetString("for_developers"),
    //        HomeDescription = GetString("home_description"),
    //        StartFreeTrial = GetString("start_free_trial"),
    //        TrialInfo = GetString("trial_info")
    //    };
    //}


    //// Traducciones específicas que encontramos en el código Next.js
    //public string UserName => T("user_name", "Jonas Requena");
    //public string YourInstances => T("your_instances", "Tus Instancias");
    //public string FreeTrial => T("free_trial", "Prueba Gratis");
    //public string Subscription => T("subscription", "Suscripción");
    //public string Documentation => T("documentation", "Documentación");
    //public string Wabulk => T("wabulk", "WaBulk");
    //public string ApiToken => T("api_token", "Token API");
    //public string WebhookSettings => T("webhook_settings", "Configuración Webhook");
    //public string Support => T("support", "Soporte");
    //public string Profile => T("profile", "Perfil");
    //public string Logout => T("logout", "Cerrar Sesión");
    //public string Login => T("Auth_Login", "Iniciar Sesión");
    //public string HomeTitle => T("home_title", "WhatsApp API");
    //public string ForDevelopers => T("for_developers", "para desarrolladores");
    //public string HomeDescription => T("home_description", "Envía y recibe mensajes, gestiona chats, grupos y canales fácilmente con nuestra API de nivel empresarial. Paga por cuenta de WhatsApp, sin tarifas por mensaje.");
    //public string StartFreeTrial => T("start_free_trial", "Comenzar prueba gratis");
    //public string TrialInfo => T("trial_info", "3 días de prueba gratis • Sin tarjeta de crédito • Cancela cuando quieras");
}
