using System.Dynamic;

using WaHub.Shared.Services;

namespace WaHub.Shared.Models;

public class ExternalPagesLocalization : DynamicObject
{
    private readonly Dictionary<string, string> _values = new();
    private readonly ILocalizationService _localizationService;

    public ExternalPagesLocalization(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    // Implementación para acceso dinámico a propiedades
    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        string propertyName = binder.Name;

        //Intentar obtener la clave desde el diccionario
        if (!_values.TryGetValue(propertyName, out string value))
        {
            // Si no existe, intentar cargarlo usando el servicio de localización
            value = _localizationService.GetString(propertyName.ToLower());
            _values[propertyName] = value;
        }

        result = value;
        return true;
    }

    //Método para limpiar el caché cuando cambia el idioma
    public void ClearCache()
    {
        _values.Clear();
    }

    // Propiedad explícita para Login para compatibilidad con código existente
    public string Login => _localizationService.GetString("login");
    public string UserName => _localizationService.GetString("username");
    public string YourInstances => _localizationService.GetString("your_instances");
    public string FreeTrial => _localizationService.GetString("free_trial");
    public string Subscription => _localizationService.GetString("subscription");
    public string Documentation => _localizationService.GetString("documentation");
    public string Wabulk => _localizationService.GetString("wabulk");
    public string ApiToken => _localizationService.GetString("api_token");
    public string WebhookSettings => _localizationService.GetString("webhook_settings");
    public string Support => _localizationService.GetString("support");
    public string Profile => _localizationService.GetString("profile");
    public string Logout => _localizationService.GetString("logout");
    public string HomeTitle => _localizationService.GetString("home_title");
    public string ForDevelopers => _localizationService.GetString("for_developers");
    public string HomeDescription => _localizationService.GetString("home_description");
    public string StartFreeTrial => _localizationService.GetString("start_free_trial");
    public string TrialInfo => _localizationService.GetString("trial_info");

}

//public class LocalizationModel
//{
   
//}
