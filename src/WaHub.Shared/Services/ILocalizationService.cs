namespace WaHub.Shared.Services;

public interface ILocalizationService
{
    string CurrentLanguage { get; }
    event Action LanguageChanged;
    string GetString(string key, string defaultValue = "");
    Task SetLanguageAsync(string language);
   
}
