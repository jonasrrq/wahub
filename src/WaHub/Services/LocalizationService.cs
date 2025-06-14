using Microsoft.Extensions.Localization;

using WaHub.Resources;
using WaHub.Shared.Models;
using WaHub.Shared.Services;

namespace WaHub.Services;

public class LocalizationService : ILocalizationService
{
    private readonly IStringLocalizer<Resource> _localizer;
    private string _currentLanguage = "es";

    public LocalizationService(IStringLocalizer<Resource> localizer)
    {
        _localizer = localizer;
        _ = InitializeLanguageAsync();
    }

    public event Action? LanguageChanged;

    public string CurrentLanguage => _currentLanguage;

    public string GetString(string key, string defaultValue = "")
    {
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
        await Task.CompletedTask; // Simular una tarea asíncrona si es necesario
        LanguageChanged?.Invoke();
    }

    private async Task InitializeLanguageAsync()
    {
        try
        {
            // Obtener el idioma del navegador
            var browserLang = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

            // Verificar si el idioma está soportado           
            if (SupportedCultures.All.Any(x => x.TwoLetterISOLanguageName.Equals(browserLang, StringComparison.OrdinalIgnoreCase)))
            {
                _currentLanguage = browserLang;
            }
            else
            {
                _currentLanguage = "es"; // idioma por defecto
            }

            // Aplicar el idioma inmediatamente
            await SetLanguageAsync(_currentLanguage);
        }
        catch
        {
            _currentLanguage = "es"; // idioma por defecto
            await SetLanguageAsync(_currentLanguage);
        }
    }   
}
