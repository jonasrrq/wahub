# Issue #10 - Implantar Notificaciones

## Descripción
Resumen: Se debe activar y asegurar el correcto funcionamiento del sistema de notificaciones en WaHub, permitiendo mostrar mensajes de éxito, error, advertencia e información en toda la aplicación.

Contexto: WaHub ya cuenta con un servicio de notificaciones (`NotificationService`) y un componente visual (`NotificationContainer.razor`) en el cliente Blazor. Sin embargo, actualmente las notificaciones no se muestran correctamente en todos los flujos o no se disparan en los lugares esperados. El sistema de notificaciones es clave para la experiencia de usuario, ya que informa sobre operaciones exitosas, errores y advertencias en tiempo real.

Qué se debe modificar:
- Revisar e integrar correctamente el `NotificationService` en los servicios y páginas relevantes de la aplicación (`src/WaHub.Client/Services/`, `src/WaHub.Client/Pages/`, `src/WaHub.Client/Layout/NotificationContainer.razor`).
- Asegurarse de que los métodos `ShowSuccess`, `ShowError`, `ShowWarning` y `ShowInfo` se llamen en los puntos adecuados (por ejemplo, tras operaciones CRUD, autenticación, errores de red, etc.).
- Verificar que el componente `NotificationContainer.razor` esté presente en el layout principal y reciba correctamente los eventos.
- Revisar la documentación y ejemplos de uso en el código (`src/WaHub.Shared/Models/NotificationService.cs`, `src/WaHub.Client/Layout/NotificationContainer.razor`).
- Realizar pruebas en los principales flujos de usuario para validar la visualización de notificaciones.
- Implementar y documentar tests automáticos (unitarios y de integración) para asegurar el correcto funcionamiento del sistema de notificaciones.

Criterios de aceptación:
- Las notificaciones se muestran correctamente en toda la aplicación para los diferentes tipos de mensajes.
- El usuario recibe feedback inmediato tras acciones importantes (éxito, error, advertencia, info).
- El sistema es consistente y no genera errores en consola.
- El código está documentado y, si es necesario, se actualiza la guía de uso de notificaciones.
- Existen tests automáticos que validan los principales escenarios de notificaciones.
- 
