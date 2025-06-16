# Issue #LANGSELECT-001 - Error: Root component type 'WaHub.Components.Shared.LanguageSelector' could not be found

## TODO List

- [x] Identificar causa del error de componente no encontrado (Orden cronológico de implementación)
- [x] Verificar referencias entre proyectos WaHub, WaHub.Client y WaHub.Shared
- [x] Agregar referencia faltante a WaHub.Shared en WaHub.csproj
- [x] Crear implementación de ILocalizationService para el cliente (ClientLocalizationService)
- [x] Mover componente LanguageSelector al proyecto cliente para compatibilidad con InteractiveAuto
- [x] Actualizar imports en _Imports.razor para incluir WaHub.Client.Shared
- [x] Modificar referencias de LanguageSelector en layouts (HomeNavbar y InternalLayout)
- [x] Eliminar componente LanguageSelector duplicado del proyecto servidor
- [x] Verificar compilación exitosa
- [x] **ACTUALIZACIÓN**: Solucionar problema de funcionalidad del cambio de idioma
- [x] Corregir nombre de cookie hardcodeado en el cliente
- [x] Agregar función JavaScript getLanguageCookie al servidor
- [x] Mejorar inicialización del ClientLocalizationService
- [x] Implementar IDisposable en LanguageSelector para manejo de eventos
- [x] Verificar funcionamiento completo del selector de idioma

## Development Notes

### 16 de junio de 2025 - Solución del error de componente LanguageSelector no encontrado

- **Problema identificado**: El componente `LanguageSelector` estaba definido en el proyecto servidor (`WaHub`) pero usaba `@rendermode InteractiveAuto`, lo cual significa que puede ejecutarse tanto en el servidor como en el cliente (WebAssembly). Cuando se ejecutaba en el cliente, no podía encontrar las dependencias necesarias porque `ILocalizationService` no estaba registrado en el cliente.

- **Solución implementada**:
  1. **Agregada referencia faltante**: Se añadió `<ProjectReference Include="..\WaHub.Shared\WaHub.Shared.csproj" />` al archivo `WaHub.csproj`
  
  2. **Creado servicio de localización para cliente**: Se implementó `ClientLocalizationService` que cumple con la interfaz `ILocalizationService` pero funciona en el entorno WebAssembly usando localStorage y JSRuntime
  
  3. **Movido componente al cliente**: Se trasladó `LanguageSelector.razor` de `WaHub/Components/Shared/` a `WaHub.Client/Shared/` para que pueda ejecutarse correctamente en ambos contextos
  
  4. **Actualizado sistema de imports**: Se modificó `_Imports.razor` del servidor para incluir `@using WaHub.Client.Shared`
  
  5. **Corregidas referencias en layouts**: Se cambiaron `<LanguageSelector />` por `<WaHub.Client.Shared.LanguageSelector />` en `HomeNavbar.razor` e `InternalLayout.razor`

### 16 de junio de 2025 - Corrección de la funcionalidad del cambio de idioma

- **Problema secundario identificado**: Después de la migración del componente al cliente, el cambio de idioma dejó de funcionar debido a:
  1. Nombre de cookie hardcodeado incorrectamente como string literal
  2. Función JavaScript `getLanguageCookie` no disponible en el servidor
  3. Inicialización inadecuada del servicio de localización del cliente
  4. Falta de manejo de eventos del ciclo de vida del componente

- **Soluciones adicionales implementadas**:
  1. **Corregido nombre de cookie**: Cambiado de `"CookieRequestCultureProvider.DefaultCookieName"` a `".AspNetCore.Culture"`
  
  2. **Agregada función JavaScript**: Implementada `getLanguageCookie` en `app.js` del servidor para leer cookies de idioma
  
  3. **Mejorado ClientLocalizationService**: 
     - Agregada inicialización automática desde cookies y localStorage
     - Validación de idiomas soportados usando `SupportedCultures.All`
     - Mejor manejo de errores y valores por defecto
  
  4. **Mejorado componente LanguageSelector**:
     - Implementado `IDisposable` para limpieza de eventos
     - Agregado `OnInitializedAsync` para inicialización adecuada
     - Suscripción a eventos `LanguageChanged` para actualizaciones de UI
     - Mejor manejo de errores en el cambio de idioma

- **Cambios técnicos específicos**:
  - Eliminado `@using Microsoft.AspNetCore.Localization` del componente cliente (no disponible en WebAssembly)
  - Registrado `ClientLocalizationService` en `Program.cs` del cliente
  - Implementados métodos faltantes: `GetString()` y event `LanguageChanged`
  - Agregada función `getLanguageCookie` para leer cookies desde JavaScript
  - Validación de idiomas soportados antes de establecer el idioma

- **Resultado final**: 
  - ✅ El proyecto compila sin errores
  - ✅ La aplicación se ejecuta correctamente en `http://localhost:5014`
  - ✅ El componente `LanguageSelector` funciona correctamente en modo `@rendermode InteractiveAuto`
  - ✅ El cambio de idioma funciona tanto desde el servidor como desde el cliente WebAssembly
  - ✅ Las cookies de idioma se establecen y leen correctamente
  - ✅ El estado del idioma se persiste entre recargas de página

- **Referencias**: Este problema es común en aplicaciones Blazor híbridas donde los componentes deben funcionar tanto en Server como en WebAssembly. La clave es asegurar que todas las dependencias estén disponibles en ambos contextos y que los servicios compartan la misma interfaz pero tengan implementaciones específicas para cada entorno.
