# Despliegue con Docker Compose - WaHub

## Configuración de Variables de Entorno

### Archivo .env
El archivo `.env` contiene las variables de entorno necesarias para el despliegue:

```bash
# Configuración de Base de Datos
SA_PASSWORD=jr123456789JR#
CONNECTION_STRING=Server=sqlserver,1433;Database=WahubManager;User Id=sa;Password=jr123456789JR#;TrustServerCertificate=true;Encrypt=false

# Configuración de la aplicación
ASPNETCORE_ENVIRONMENT=Production
```

### Variables de Entorno para ASP.NET Core

En ASP.NET Core, las variables de entorno para ConnectionStrings siguen el patrón:
- `ConnectionStrings__NombreDeLaConexion` (doble guión bajo)
- En el docker-compose.yml se define como: `ConnectionStrings__DefaultConnection`

## Comandos de Despliegue

### 1. Construir y ejecutar los servicios
```bash
docker-compose up --build
```

### 2. Ejecutar en modo detached (segundo plano)
```bash
docker-compose up -d --build
```

### 3. Ver logs
```bash
# Todos los servicios
docker-compose logs -f

# Solo la aplicación
docker-compose logs -f wahub-app

# Solo SQL Server
docker-compose logs -f sqlserver
```

### 4. Detener los servicios
```bash
docker-compose down
```

### 5. Detener y eliminar volúmenes
```bash
docker-compose down -v
```

## Estructura de Servicios

- **wahub-app**: Aplicación ASP.NET Core en puertos 8080/8081
- **sqlserver**: SQL Server Express 2022 en puerto 1433
- **Volumen**: `sqlserver_data` para persistencia de datos

## Notas Importantes

1. **Seguridad**: En producción, usar contraseñas más seguras y considerar el uso de secretos
2. **Migraciónes**: Asegúrate de que las migraciones se ejecuten al iniciar la aplicación
3. **Red**: Ambos servicios están en la misma red (`wahub-network`) para comunicación interna
4. **Certificados**: `TrustServerCertificate=true` para desarrollo, revisar para producción
