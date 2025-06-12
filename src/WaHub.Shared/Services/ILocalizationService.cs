using WaHub.Shared.Models;

namespace WaHub.Shared.Services;

public interface ILocalizationService
{
    string CurrentLanguage { get; }
    ExternalPagesLocalization ExternalPages { get; }
    event Action LanguageChanged;
    string GetString(string key, string defaultValue = "");
    Task SetLanguageAsync(string language);
    string T(string key, string defaultValue = "");
}
