# Configuración de Migraciones Automáticas en Producción

## Descripción
Este documento describe cómo está configurado el proyecto WaHub para ejecutar automáticamente las migraciones de Entity Framework cuando se inicia en producción.

## Configuración Actual

### 1. Archivos Modificados

#### Program.cs
- Se agregó la lógica para ejecutar migraciones automáticamente en producción
- Solo se ejecutan cuando `Database:AutoMigrate` es `true` y el entorno no es Development

#### ServiceRegistration.cs
- Se implementó el método `ApplyMigrationsAsync` que:
  - Verifica si la base de datos existe
  - Obtiene migraciones pendientes
  - Aplica las migraciones de forma segura
  - Registra logs detallados del proceso

### 2. Configuración de appsettings

#### appsettings.json
```json
{
  "Database": {
    "AutoMigrate": true,
    "FailOnMigrationError": false
  }
}
```

#### appsettings.Production.json
- Configuración específica para producción
- Usa variables de entorno para la cadena de conexión
- Logs optimizados para producción

### 3. Variables de Configuración

| Variable | Descripción | Valor por Defecto |
|----------|-------------|-------------------|
| `Database:AutoMigrate` | Habilita/deshabilita migraciones automáticas | `true` |
| `Database:FailOnMigrationError` | Si `true`, la aplicación no iniciará si fallan las migraciones | `false` |

### 4. Variables de Entorno para Producción

Para producción, configura estas variables de entorno:

```bash
DB_SERVER=tu-servidor-sql
DB_NAME=WahubManager
DB_USER=tu-usuario
DB_PASSWORD=tu-contraseña
ASPNETCORE_ENVIRONMENT=Production
```

## Comportamiento en Diferentes Entornos

### Development
- Las migraciones NO se ejecutan automáticamente
- Se espera que el desarrollador ejecute `dotnet ef database update` manualmente

### Production
- Las migraciones se ejecutan automáticamente al iniciar la aplicación
- Si fallan las migraciones, la aplicación continúa ejecutándose (configurable)
- Se registran logs detallados del proceso

## Logs de Migraciones

El sistema registra los siguientes eventos:
- Inicio del proceso de migración
- Creación de base de datos si no existe
- Lista de migraciones pendientes
- Éxito o fallo de las migraciones

## Desactivar Migraciones Automáticas

Para desactivar las migraciones automáticas en producción:

1. **Variable de entorno:**
   ```bash
   Database__AutoMigrate=false
   ```

2. **En appsettings.Production.json:**
   ```json
   {
     "Database": {
       "AutoMigrate": false
     }
   }
   ```

## Consideraciones de Seguridad

1. **Respaldos:** Siempre realiza respaldos antes de desplegar en producción
2. **Monitoreo:** Revisa los logs después del despliegue para verificar que las migraciones se ejecutaron correctamente
3. **Rollback:** Ten un plan de rollback en caso de que las migraciones fallen

## Comandos Útiles

### Crear una nueva migración:
```bash
dotnet ef migrations add NombreDeLaMigration --project src/WaHub
```

### Ver migraciones pendientes:
```bash
dotnet ef migrations list --project src/WaHub
```

### Aplicar migraciones manualmente:
```bash
dotnet ef database update --project src/WaHub
```

### Revertir a una migración específica:
```bash
dotnet ef database update NombreDeLaMigration --project src/WaHub
```

## Docker Deployment

El Dockerfile actual ya está configurado para soportar migraciones automáticas. Al ejecutar el contenedor:

```bash
docker run -e ASPNETCORE_ENVIRONMENT=Production \
           -e DB_SERVER=tu-servidor \
           -e DB_NAME=WahubManager \
           -e DB_USER=tu-usuario \
           -e DB_PASSWORD=tu-contraseña \
           wahub:latest
```

## Troubleshooting

### La aplicación no inicia
- Verifica la cadena de conexión
- Revisa los logs para errores de migración
- Considera establecer `Database:FailOnMigrationError=false`

### Migraciones no se ejecutan
- Verifica que `Database:AutoMigrate=true`
- Confirma que el entorno es Production
- Revisa los logs de aplicación

### Error de permisos de base de datos
- Asegúrate de que el usuario tenga permisos para crear/modificar la base de datos
- Verifica la cadena de conexión
