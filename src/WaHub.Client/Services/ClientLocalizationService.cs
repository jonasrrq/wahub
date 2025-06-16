using WaHub.Shared.Services;
using Microsoft.JSInterop;
using WaHub.Shared.Models;

namespace WaHub.Client.Services
{
    public class ClientLocalizationService : ILocalizationService
    {
        private readonly IJSRuntime _jsRuntime;
        private string _currentLanguage = SupportedCultures.Default; // Default language

        public ClientLocalizationService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            _ = InitializeAsync(); // Initialize language from cookies/localStorage
        }

        public string CurrentLanguage => _currentLanguage;
        
        public event Action? LanguageChanged;

        public async Task SetLanguageAsync(string language)
        {
            if (SupportedCultures.All.Any(c => c.Name == language || c.TwoLetterISOLanguageName == language))
            {
                _currentLanguage = language;
                
                // Store in localStorage for persistence
                try
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "selectedLanguage", language);
                }
                catch
                {
                    // Ignore errors if localStorage is not available
                }
                
                // Notify subscribers
                LanguageChanged?.Invoke();
            }
        }

        public string GetString(string key, string defaultValue = "")
        {
            // For client-side, we'll return the defaultValue or key as fallback
            // In a real implementation, you'd load resources based on current language
            return string.IsNullOrEmpty(defaultValue) ? key : defaultValue;
        }

        public async Task InitializeAsync()
        {
            try
            {
                // First try to get language from cookie
                var cookieLanguage = await _jsRuntime.InvokeAsync<string>("getLanguageCookie", ".AspNetCore.Culture");
                if (!string.IsNullOrEmpty(cookieLanguage) && 
                    SupportedCultures.All.Any(c => c.Name == cookieLanguage || c.TwoLetterISOLanguageName == cookieLanguage))
                {
                    _currentLanguage = cookieLanguage;
                    return;
                }
                
                // Fallback to localStorage
                var storedLanguage = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "selectedLanguage");
                if (!string.IsNullOrEmpty(storedLanguage) && 
                    SupportedCultures.All.Any(c => c.Name == storedLanguage || c.TwoLetterISOLanguageName == storedLanguage))
                {
                    _currentLanguage = storedLanguage;
                }
            }
            catch
            {
                // If there's any error, use default language
                _currentLanguage = SupportedCultures.Default;
            }
        }
    }
}
