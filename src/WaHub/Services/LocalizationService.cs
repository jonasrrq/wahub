using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

using WaHub.Resources;
using WaHub.Shared.Models;
using WaHub.Shared.Services;

namespace WaHub.Services;

public class LocalizationService : ILocalizationService
{
    private readonly IStringLocalizer<Resource> _localizer;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string _currentLanguage = "es";

    public LocalizationService(IStringLocalizer<Resource> localizer, IHttpContextAccessor httpContextAccessor)
    {
        _localizer = localizer;
        _httpContextAccessor = httpContextAccessor;
        _ = InitializeLanguageAsync();
    }

    public event Action? LanguageChanged;

    public string CurrentLanguage => _currentLanguage;

    public string GetString(string key, string defaultValue = "")
    {
        var culture = new CultureInfo(_currentLanguage);
        CultureInfo.CurrentUICulture = culture;
        CultureInfo.CurrentCulture = culture;
        var localizedString = _localizer.GetString(key);
        return localizedString.ResourceNotFound ? defaultValue : localizedString.Value;
    }

    public async Task SetLanguageAsync(string language)
    {
        _currentLanguage = language;

        if (_httpContextAccessor.HttpContext != null)
        {
            var culture = new RequestCulture(language);
            _httpContextAccessor.HttpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(culture),
                new CookieOptions { IsEssential = true, Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
        }

        var newCulture = new CultureInfo(language);
        CultureInfo.DefaultThreadCurrentUICulture = newCulture;
        CultureInfo.DefaultThreadCurrentCulture = newCulture;
        await Task.CompletedTask;
        LanguageChanged?.Invoke();
    }

    private async Task InitializeLanguageAsync()
    {
        try
        {
            string? preferredLanguage = null;

            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(CookieRequestCultureProvider.DefaultCookieName, out var cookieValue))
            {
                var requestCulture = CookieRequestCultureProvider.ParseCookieValue(cookieValue);
                preferredLanguage = requestCulture?.UICultures[0].Value;
                //preferredLanguage = requestCulture?.UICulture?.TwoLetterISOLanguageName;
            }

            if (!string.IsNullOrEmpty(preferredLanguage) && SupportedCultures.All.Any(x => x.TwoLetterISOLanguageName.Equals(preferredLanguage, StringComparison.OrdinalIgnoreCase)))
            {
                _currentLanguage = preferredLanguage;
            }
            else
            {
                var browserLang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                if (SupportedCultures.All.Any(x => x.TwoLetterISOLanguageName.Equals(browserLang, StringComparison.OrdinalIgnoreCase)))
                {
                    _currentLanguage = browserLang;
                }
                else
                {
                    _currentLanguage = "es";
                }
            }

            await SetLanguageAsync(_currentLanguage);
        }
        catch
        {
            _currentLanguage = "es";
            await SetLanguageAsync(_currentLanguage);
        }
    }   
}
