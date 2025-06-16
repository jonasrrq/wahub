# Guía de Internacionalización en WaHub

Esta guía detalla el proceso para añadir nuevas cadenas de texto traducibles a tu aplicación WaHub, utilizando el sistema de recursos (`.resx`) y la clase `ResourceKeys`.

## Estructura de Localización Actual

Tu proyecto utiliza la siguiente estructura para la internacionalización:

*   **Clase de constantes para las claves:** `src/WaHub.Shared/Models/ResourceKeys.cs`
    *   Aquí definirás una constante `public const string` para cada texto que necesites traducir.
*   **Archivos de recursos (`.resx`):** Se encuentran en `src/WaHub/Resources/`.
    *   `Resource.resx`: Contiene las cadenas de texto por defecto (Español).
    *   `Resource.en.resx`: Contiene las traducciones al Inglés.
    *   `Resource.pt.resx`: Contiene las traducciones al Portugués (Portugal).
    *   `Resource.pt-BR.resx`: Contiene las traducciones al Portugués (Brasil).
    *   `Resource.fr.resx`: Contiene las traducciones al Francés.

*   **Servicio de Localización:** `src/WaHub/Services/LocalizationService.cs`
    *   Este servicio (`ILocalizationService`) se encarga de obtener la cadena traducida basándose en la clave y el idioma actual del usuario.

## Pasos para Añadir una Nueva Traducción


### Paso 1: Identificar el Texto a Traducir

Revisa el componente y has una lista de textos a tradocur. Por ejemplo, si deseas traducir "Tus Instances" del componente `Instances.razor`.

### Paso 2: Añadir una Nueva Constante en `ResourceKeys.cs`

Abre el archivo `src/WaHub.Shared/Models/ResourceKeys.cs` y añade una nueva constante para tu cadena de texto. Se recomienda usar un nombre descriptivo en `PascalCase`.

**Ejemplo: `src/WaHub.Shared/Models/ResourceKeys.cs`**

```csharp
namespace WaHub.Shared.Models;

public static class ResourceKeys
{
    public const string Login = "Login";

    // --- Componentes de WaHub.Client/Pages/Instances.razor ---
    public const string YourInstances = "YourInstances"; 
    public const string InstancesTitle = "InstancesTitle"; // Ejemplo: "Instancias - WaHub"
    // ... más constantes para Instances.razor ...

    // --- Componentes de WaHub/Components/Pages/Home.razor ---
    public const string HomeTitle = "HomeTitle"; // "WaHub - Plataforma de Gestión de WhatsApp"
    public const string ForDevelopers = "ForDevelopers"; // "para desarrolladores"
    // ... más constantes para Home.razor ...
}
```

### Paso 3: Añadir las Traducciones en los Archivos `.resx`

Para cada idioma soportado, abre el archivo `.resx` correspondiente en `src/WaHub/Resources/` y añade una nueva entrada `<data>` con el nombre de tu constante y el valor traducido.

**Recomendación:** Para mejorar la legibilidad, puedes agregar comentarios XML para indicar a qué componente pertenecen las cadenas.

**Ejemplo: `src/WaHub/Resources/Resource.resx` (Español - por defecto)**

```xml
<!-- ... existing code ... -->
<data name="Login" xml:space="preserve">
  <value>Iniciar sesión</value>
</data>

<!-- Traducciones para src/WaHub.Client/Pages/Instances.razor -->
<data name="YourInstances" xml:space="preserve">
  <value>Tus Instancias</value>
</data>
<data name="InstancesTitle" xml:space="preserve">
  <value>Instancias</value>
</data>

<!-- Traducciones para src/WaHub/Components/Pages/Home.razor -->
<data name="HomeTitle" xml:space="preserve">
  <value>WaHub - Plataforma de Gestión de WhatsApp</value>
</data>
<data name="ForDevelopers" xml:space="preserve">
  <value>para desarrolladores</value>
</data>

<!-- Traducciones para src/WaHub/Components/Pages/EnContruccion.razor -->
<data name="UnderConstructionTitle" xml:space="preserve">
  <value>¡Página en construcción!</value>
</data>
<data name="UnderConstructionMessage1" xml:space="preserve">
  <value>Estamos trabajando para traerte esta sección muy pronto.</value>
</data>
<data name="UnderConstructionMessage2" xml:space="preserve">
  <value>Vuelve más tarde o explora otras áreas de la plataforma.</value>
</data>

<!-- Traducciones para src/WaHub/Components/Pages/Error.razor -->
<data name="Error404Title" xml:space="preserve">
  <value>Error 404</value>
</data>
<data name="PageNotFound" xml:space="preserve">
  <value>Página no encontrada</value>
</data>
<data name="PageNotFoundMessage" xml:space="preserve">
  <value>Lo sentimos, la página que buscas no existe o ha sido movida.</value>
</data>
<!-- ... añade más traducciones al español ... -->
```

**Ejemplo: `src/WaHub/Resources/Resource.en.resx` (Inglés)**

```xml
<!-- ... existing code ... -->
<data name="Login" xml:space="preserve">
  <value>Login</value>
</data>

<!-- Translations for src/WaHub.Client/Pages/Instances.razor -->
<data name="YourInstances" xml:space="preserve">
  <value>Your Instances</value>
</data>
<data name="InstancesTitle" xml:space="preserve">
  <value>Instances</value>
</data>

<!-- Translations for src/WaHub/Components/Pages/Home.razor -->
<data name="HomeTitle" xml:space="preserve">
  <value>WaHub - WhatsApp Management Platform</value>
</data>
<data name="ForDevelopers" xml:space="preserve">
  <value>for developers</value>
</data>

<!-- Translations for src/WaHub/Components/Pages/EnContruccion.razor -->
<data name="UnderConstructionTitle" xml:space="preserve">
  <value>Page under construction!</value>
</data>
<data name="UnderConstructionMessage1" xml:space="preserve">
  <value>We are working to bring you this section very soon.</value>
</data>
<data name="UnderConstructionMessage2" xml:space="preserve">
  <value>Please come back later or explore other areas of the platform.</value>
</data>

<!-- Translations for src/WaHub/Components/Pages/Error.razor -->
<data name="Error404Title" xml:space="preserve">
  <value>Error 404</value>
</data>
<data name="PageNotFound" xml:space="preserve">
  <value>Page not found</value>
</data>
<data name="PageNotFoundMessage" xml:space="preserve">
  <value>Sorry, the page you are looking for does not exist or has been moved.</value>
</data>
<!-- ... añade más traducciones al inglés ... -->
```

**(Repite para `Resource.pt.resx`, `Resource.pt-BR.resx`, `Resource.fr.resx` con las traducciones correspondientes y los comentarios.)**

### Paso 4: Actualizar el Componente Blazor (`.razor`)

En el componente Blazor donde se muestra el texto, primero asegúrate de que el servicio `ILocalizationService` esté inyectado. Si no lo está, agrégalo al inicio del archivo:

```csharp
@inject ILocalizationService Localization
```

Luego, reemplaza el texto estático con la llamada al método `GetString` del servicio `Localization`, usando la constante que definiste en `ResourceKeys`.

**Ejemplo: Antes (en `src/WaHub.Client/Pages/Instances.razor`)**

```html
<h1>Tus Instances</h1>
<PageTitle>Instancias - WaHub</PageTitle>
```

**Ejemplo: Después (en `src/WaHub.Client/Pages/Instances.razor`)**

```html
@inject ILocalizationService Localization
@* ... existing code ... *@

<h1>@Localization.GetString(ResourceKeys.YourInstances)</h1>
<PageTitle>@Localization.GetString(ResourceKeys.InstancesTitle) - WaHub</PageTitle>
```

**Ejemplo para `src/WaHub/Components/Pages/Home.razor`:**

**Antes:**

```html
<PageTitle>WaHub - Plataforma de Gestión de WhatsApp</PageTitle>
<h2 class="main-title">
    WaHub <span class="highlight">para desarrolladores</span>
</h2>
```

**Ejemplo: Después (en `src/WaHub/Components/Pages/Home.razor`)**

```html
@inject ILocalizationService Localization
@* ... existing code ... *@

<PageTitle>@Localization.GetString(ResourceKeys.HomeTitle)</PageTitle>
<h2 class="main-title">
    WaHub <span class="highlight">@Localization.GetString(ResourceKeys.ForDevelopers)</span>
</h2>
```

**Ejemplo para `src/WaHub/Components/Pages/EnContruccion.razor`:**

**Antes:**

```html
<h1>¡Página en construcción!</h1>
<p>Estamos trabajando para traerte esta sección muy pronto.<br />
Vuelve más tarde o explora otras áreas de la plataforma.</p>
```

**Ejemplo: Después (en `src/WaHub/Components/Pages/EnContruccion.razor`)**

```html
@inject ILocalizationService Localization
@* ... existing code ... *@

<h1>@Localization.GetString(ResourceKeys.UnderConstructionTitle)</h1>
<p>@Localization.GetString(ResourceKeys.UnderConstructionMessage1)<br />
@Localization.GetString(ResourceKeys.UnderConstructionMessage2)</p>
```

**Ejemplo para `src/WaHub/Components/Pages/Error.razor`:**

**Antes:**

```html
<PageTitle>Error 404</PageTitle>
<h1>Página no encontrada</h1>
<p>Lo sentimos, la página que buscas no existe o ha sido movida.</p>
```

**Ejemplo: Después (en `src/WaHub/Components/Pages/Error.razor`)**

```html
@inject ILocalizationService Localization
@* ... existing code ... *@

<PageTitle>@Localization.GetString(ResourceKeys.Error404Title)</PageTitle>
<h1>@Localization.GetString(ResourceKeys.PageNotFound)</h1>
<p>@Localization.GetString(ResourceKeys.PageNotFoundMessage)</p>
```

#### Manejo de Archivos de Código C# Separados (`.razor.cs`)

Si tu componente Blazor tiene un archivo de código C# separado (code-behind) con la extensión `.razor.cs`, debes inyectar `ILocalizationService` directamente en el constructor de tu clase parcial o utilizando el atributo `[Inject]`.

**Ejemplo: Inyección en el constructor (`TuComponente.razor.cs`)**

```csharp
using Microsoft.AspNetCore.Components;
using WaHub.Shared.Services;
using WaHub.Shared.Models;

namespace TuProyecto.Componentes;

public partial class TuComponente
{
    private readonly ILocalizationService _localization;

    public TuComponente(ILocalizationService localization)
    {
        _localization = localization;
    }

    // ... otros métodos y propiedades

    private string ObtenerTextoTraducido()
    {
        return _localization.GetString(ResourceKeys.TuClaveDeRecurso);
    }
}
```

**Ejemplo: Inyección con atributo `[Inject]` (`TuComponente.razor.cs`)**

```csharp
using Microsoft.AspNetCore.Components;
using WaHub.Shared.Services;
using WaHub.Shared.Models;

namespace TuProyecto.Componentes;

public partial class TuComponente
{
    [Inject]
    public ILocalizationService Localization { get; set; }

    // ... otros métodos y propiedades

    private string ObtenerTextoTraducido()
    {
        return Localization.GetString(ResourceKeys.TuClaveDeRecurso);
    }
}
```

Una vez inyectado, puedes usar `_localization.GetString(ResourceKeys.TuClave)` o `Localization.GetString(ResourceKeys.TuClave)` (dependiendo de cómo lo hayas inyectado) en tu lógica de C# para obtener las cadenas traducidas.

**Ejemplo práctico: `src/WaHub.Client/Layout/WaHubSidebar.razor.cs`**

**Antes:**

```csharp
// ... existing code ...
public partial class WaHubSidebar
{
    // ... existing code ...
    private MenuItem[] mainMenu = Array.Empty<MenuItem>();
    private MenuItem[] bottomMenu = Array.Empty<MenuItem>();
    // ... existing code ...

    protected override void OnInitialized()
    {
        mainMenu = new[]
        {
        new MenuItem { Label = "Tus Instancias", Icon = "bi-stack", Href = "/admin/instances", Active = true },
        new MenuItem { Label = "Prueba Gratis", Icon = "bi-lightning-fill", Href = "/admin/trial", Badge = "TRIAL" },
        new MenuItem { Label = "Suscripción", Icon = "bi-credit-card", Href = "/admin/subscription" },
        new MenuItem { Label = "Documentación", Icon = "bi-book", Href = "/admin/documentation" },
        new MenuItem { Label = "WaBulk", Icon = "bi-lightning-fill", Href = "/admin/wabulk", Badge = "BETA" },
    };

        bottomMenu = new[]
        {
        new MenuItem { Label = "API Token", Icon = "bi-key-fill", Href = "/admin/api-token" },
        new MenuItem { Label = "Webhook Settings", Icon = "bi-gear-fill", Href = "/admin/webhook-settings" },
        new MenuItem { Label = "Soporte", Icon = "bi-question-circle-fill", Href = "/admin/support" },
    };
    }
    // ... existing code ...
}
```

**Después:**

```csharp
// ... existing code ...
public partial class WaHubSidebar
{
    private readonly NavigationService _navigation;
    public readonly ILocalizationService _localization;
    private MenuItem[] mainMenu = Array.Empty<MenuItem>();
    private MenuItem[] bottomMenu = Array.Empty<MenuItem>();

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
```

---

Recuerda que cada vez que añadas una nueva constante a `ResourceKeys.cs`, deberás asegurarte de añadir la entrada `data` correspondiente en *todos* los archivos `.resx` para cada idioma que soportes. Si un idioma no tiene una traducción para una clave, el `LocalizationService` debería devolver la cadena por defecto (del `Resource.resx`).
