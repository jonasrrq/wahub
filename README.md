# 🚀 WaHub - API de WhatsApp con WebHooks

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-Server-512BD4?logo=blazor)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![API](https://img.shields.io/badge/API-WhatsApp-25D366?logo=whatsapp)](https://developers.facebook.com/docs/whatsapp/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

## 📋 Descripción
WaHub es la solución todo-en-uno para PYMEs que buscan potenciar su comunicación empresarial a través de WhatsApp. Nuestra plataforma ofrece una API RESTful robusta y una interfaz intuitiva que permite a las empresas enviar mensajes masivos, programar campañas y gestionar conversaciones de manera eficiente.

Desarrollada con .NET 9.0 y Blazor Server, WaHub está diseñada para escalar con tu negocio, ofreciendo herramientas profesionales de marketing y atención al cliente a través de la plataforma de mensajería más popular del mundo.

### 🏆 Por qué WaHub para tu PYME
- **Ahorro de tiempo**: Automatiza tus comunicaciones y campañas
- **Mayor alcance**: Llega a miles de clientes con unos pocos clics
- **Métricas en tiempo real**: Toma decisiones basadas en datos
- **Cumplimiento garantizado**: Cumple con las políticas de WhatsApp Business
- **Soporte dedicado**: Equipo experto para ayudarte a sacar el máximo provecho

## ✨ Características Principales

### 💬 API de WhatsApp
- Envío y recepción de mensajes de texto, imágenes, documentos y más
- Gestión de contactos y listas de difusión
- Plantillas de mensajes preaprobadas
- Soporte para respuestas rápidas y botones de acción

### 📢 Mensajería Masiva para PYMEs
- **Envío masivo** de mensajes a múltiples contactos
- **Segmentación avanzada** de audiencias
- **Personalización** de mensajes con variables
- **Límites controlados** para cumplir con políticas de WhatsApp
- **Informes detallados** de entregas y lecturas

### 📅 Campañas Programadas
- **Programación flexible** de envíos
- **Calendario de campañas** visual
- **Plantillas reutilizables** para campañas recurrentes
- **Optimización** de horarios de envío
- **A/B testing** de mensajes
- **Análisis de rendimiento** por campaña

### 🔄 WebHooks en Tiempo Real
- Configuración personalizada de WebHooks
- Notificaciones instantáneas de mensajes entrantes
- Estados de entrega y lectura de mensajes
- Eventos de conexión del dispositivo
- Seguimiento de interacciones por campaña

### 🔐 Autenticación y Seguridad
- Autenticación mediante API Keys y JWT
- Gestión de sesiones seguras
- Rate limiting configurable
- Registro detallado de actividades

### 📊 Panel de Control para PYMEs
- **Visión general** del rendimiento de campañas
- **Métricas clave** (tasa de apertura, clics, respuestas)
- **Gráficos comparativos** entre campañas
- **Exportación de informes** en múltiples formatos
- **Alertas personalizables** para métricas importantes

### 🏢 Soluciones por Industria

### 🛍️ Comercio Minorista
- Notificaciones de pedidos y envíos
- Campañas de descuentos personalizadas
- Encuestas de satisfacción post-compra

### 🏥 Salud y Bienestar
- Recordatorios de citas
- Seguimiento de tratamientos
- Encuestas de satisfacción

### 🏢 Servicios Profesionales
- Confirmación de citas
- Recordatorios de pagos
- Encuestas de satisfacción

### 🎓 Educación
- Notificaciones a padres y estudiantes
- Recordatorios de clases y eventos
- Encuestas de satisfacción

## 📚 Documentación Completa
- Documentación interactiva con Swagger/OpenAPI
- Ejemplos de código en múltiples lenguajes
- Guías de integración paso a paso
- SDKs oficiales disponibles
- Casos de éxito de PYMEs
- Webinars y tutoriales en video

## 🛠️ Tecnologías Utilizadas

### Backend
- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core
- SignalR para notificaciones en tiempo real
- FluentValidation para validación de datos
- AutoMapper para mapeo de objetos

### WhatsApp Integration
- WhatsApp Business API Client
- WebSockets para comunicación en tiempo real
- Sistema de colas para manejo de mensajes
- Plantillas de mensajes aprobadas por WhatsApp

### Almacenamiento
- SQL Server para datos estructurados
- Azure Blob Storage para archivos multimedia
- Redis para caché y colas

### Seguridad
- JWT para autenticación de API
- Rate limiting
- Encriptación de extremo a extremo
- Validación de firmas WebHook

### Herramientas de Desarrollo
- Visual Studio 2022
- Postman para pruebas de API
- Git para control de versiones
- Docker para contenedorización

## 🚀 Requisitos Previos

### Desarrollo
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/es/vs/) o [VS Code](https://code.visualstudio.com/)
- [SQL Server 2019+](https://www.microsoft.com/es-es/sql-server/sql-server-downloads) o [SQL Server Express](https://www.microsoft.com/es-es/sql-server/sql-server-downloads)
- [Git](https://git-scm.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (opcional, para ejecutar dependencias)
- Cuenta de desarrollador de [WhatsApp Business API](https://developers.facebook.com/docs/whatsapp/cloud-api/)

### Producción
- **Servidor**: Windows Server 2019/2022 o Linux
- **Runtime**: .NET 9.0 Runtime
- **Base de datos**: SQL Server 2019+ o Azure SQL Database
- **Almacenamiento**: Azure Blob Storage o AWS S3 para archivos multimedia
- **Caché**: Redis Server
- **Web Server**: Kestrel detrás de Nginx/Apache/IIS
- **SSL/TLS**: Certificado SSL válido (Let's Encrypt o comercial)
- **Dominio**: Dominio registrado con registros DNS configurados
- **IP Pública**: Dirección IP estática para el servidor

## 🛠️ Instalación

### 1. Clonar el Repositorio
```bash
git clone https://github.com/tu-usuario/wahub.git
cd wahub
```

### 2. Configuración Inicial
1. Copia el archivo de configuración de ejemplo y ajusta los valores:
   ```bash
   cd src/WaHub.Blazor
   copy appsettings.Example.json appsettings.Development.json
   ```

2. Configura las variables de entorno necesarias:
   ```bash
   # Configuración de WhatsApp
   set WHATSAPP_API_KEY=tu_api_key
   set WHATSAPP_API_SECRET=tu_api_secret
   
   # Configuración de base de datos
   set CONNECTION_STRINGS__DEFAULT=Server=localhost;Database=WaHub;Trusted_Connection=True;TrustServerCertificate=True;
   ```

### 3. Configuración de WhatsApp
1. Crea una aplicación en el [Panel de Desarrolladores de WhatsApp](https://developers.facebook.com/)
2. Obtén tus credenciales (API Key y API Secret)
3. Configura los WebHooks en el panel de desarrolladores

### 4. Base de Datos
```bash
# Instalar herramientas de EF Core
dotnet tool install --global dotnet-ef

# Aplicar migraciones
dotnet ef database update --project src/WaHub.Blazor

# O usar migraciones automáticas (solo desarrollo)
dotnet run --project src/WaHub.Blazor -- --migrate
```

### 5. Ejecutar la Aplicación

**Opción 1: Desarrollo con Hot Reload**
```bash
dotnet watch --project src/WaHub.Blazor
```

**Opción 2: Docker (recomendado para producción)**
```bash
docker-compose -f docker-compose.yml up -d
```

**Opción 3: Visual Studio 2022**
1. Abre `WaHub.sln`
2. Establece `WaHub.Blazor` como proyecto de inicio
3. Presiona F5 o el botón de Iniciar depuración

### 6. Verificar la instalación
Accede a la documentación de la API en:
```
https://localhost:5001/swagger
```

Verifica el estado del servicio en:
```
https://localhost:5001/health
```

## 🏗️ Estructura del Proyecto

```
WaHub/
├── src/
│   ├── WaHub.API/                  # API Principal
│   │   ├── Controllers/            # Controladores de la API
│   │   ├── Middleware/             # Middleware personalizado
│   │   ├── Models/                 # Modelos de solicitud/respuesta
│   │   ├── Services/               # Lógica de negocio
│   │   │   ├── WhatsApp/           # Servicios específicos de WhatsApp
│   │   │   └── WebHooks/           # Manejo de WebHooks
│   │   ├── appsettings.json        # Configuración
│   │   └── Program.cs              # Configuración de la aplicación
│   │
│   ├── WaHub.Core/                # Núcleo de la aplicación
│   │   ├── Entities/               # Entidades del dominio
│   │   ├── Interfaces/             # Interfaces del dominio
│   │   ├── DTOs/                   # Objetos de transferencia de datos
│   │   └── Exceptions/             # Excepciones personalizadas
│   │
│   └── WaHub.Infrastructure/      # Infraestructura
│       ├── Data/                   # Contexto de EF Core
│       ├── Repositories/           # Implementaciones de repositorios
│       ├── Services/               # Servicios de infraestructura
│       └── Extensions/             # Métodos de extensión
│
├── tests/                         # Pruebas
│   ├── WaHub.API.Tests/           # Pruebas de la API
│   └── WaHub.Core.Tests/          # Pruebas unitarias
│
├── docs/                          # Documentación
│   ├── api/                       # Documentación de la API
│   └── guides/                    # Guías de usuario
│
├── docker/                       # Configuración de Docker
├── scripts/                       # Scripts de utilidad
└── WaHub.sln                     # Solución de Visual Studio
```

## 🚀 Despliegue en Producción

### Requisitos Mínimos
- **Servidor**: 2+ vCPUs, 4GB+ RAM, 50GB+ almacenamiento SSD
- **Sistema Operativo**: Ubuntu 22.04 LTS o Windows Server 2022
- **Red**: IP Pública, puertos 80/443 abiertos, ancho de banda mínimo 10Mbps

### Pasos para Despliegue

1. **Preparar el Entorno**
   ```bash
   # Actualizar sistema
   sudo apt update && sudo apt upgrade -y
   
   # Instalar dependencias
   sudo apt install -y nginx certbot python3-certbot-nginx
   
   # Instalar .NET 9.0 Runtime
   wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
   sudo dpkg -i packages-microsoft-prod.deb
   rm packages-microsoft-prod.deb
   sudo apt update
   sudo apt install -y aspnetcore-runtime-9.0
   ```

2. **Configurar Base de Datos**
   - Instalar SQL Server o configurar conexión a Azure SQL
   - Ejecutar migraciones: `dotnet ef database update`

3. **Configurar Nginx como Proxy Inverso**
   ```nginx
   server {
       listen 80;
       server_name tu-dominio.com;
       return 301 https://$host$request_uri;
   }

   server {
       listen 443 ssl;
       server_name tu-dominio.com;

       ssl_certificate /etc/letsencrypt/live/tu-dominio.com/fullchain.pem;
       ssl_certificate_key /etc/letsencrypt/live/tu-dominio.com/privkey.pem;

       location / {
           proxy_pass http://localhost:5000;
           proxy_http_version 1.1;
           proxy_set_header Upgrade $http_upgrade;
           proxy_set_header Connection keep-alive;
           proxy_set_header Host $host;
           proxy_cache_bypass $http_upgrade;
           proxy_set_header X-Real-IP $remote_addr;
           proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
           proxy_set_header X-Forwarded-Proto $scheme;
       }
   }
   ```

4. **Obtener Certificado SSL**
   ```bash
   sudo certbot --nginx -d tu-dominio.com
   ```

5. **Configurar Servicio de Sistema**
   ```bash
   sudo nano /etc/systemd/system/wahub.service
   ```
   
   ```ini
   [Unit]
   Description=WaHub WhatsApp API
   After=network.target

   [Service]
   WorkingDirectory=/var/www/wahub
   ExecStart=/usr/bin/dotnet /var/www/wahub/WaHub.API.dll
   Restart=always
   RestartSec=10
   SyslogIdentifier=wahub
   User=www-data
   Environment=ASPNETCORE_ENVIRONMENT=Production
   Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
   Environment=ASPNETCORE_URLS=http://localhost:5000

   [Install]
   WantedBy=multi-user.target
   ```

6. **Iniciar el Servicio**
   ```bash
   sudo systemctl enable wahub.service
   sudo systemctl start wahub.service
   sudo systemctl status wahub.service
   ```

## 🧪 Ejecutando Pruebas

### Pruebas Unitarias
```bash
# Ejecutar todas las pruebas
dotnet test

# Ejecutar pruebas específicas
dotnet test --filter "FullyQualifiedName~WhatsAppServiceTests"

# Generar cobertura de código
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

### Pruebas de Integración
1. Configurar entorno de pruebas en `appsettings.Testing.json`
2. Ejecutar contenedores de prueba:
   ```bash
   docker-compose -f docker-compose.test.yml up -d
   ```
3. Ejecutar pruebas de integración:
   ```bash
   dotnet test WaHub.IntegrationTests
   ```

### Pruebas de Carga
```bash
# Instalar k6
sudo apt-get update && sudo apt-get install -y k6

# Ejecutar prueba de carga
k6 run tests/load/send-message-test.js
```

### Monitoreo
La API incluye endpoints de salud y métricas:
- `GET /health`: Estado general del servicio
- `GET /metrics`: Métricas en formato Prometheus
- `GET /swagger`: Documentación interactiva de la API

## 🤝 Contribución
¡Las contribuciones son bienvenidas! Por favor, lee nuestras [pautas de contribución](CONTRIBUTING.md) antes de enviar un pull request.

1. Haz un Fork del proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Haz commit de tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Haz push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia
Distribuido bajo la licencia MIT. Consulta el archivo `LICENSE` para más información.

## 📞 Contacto
Equipo de Desarrollo WaHub - [@WaHubApp](https://twitter.com/WaHubApp) - info@wahub.app

Enlace del Proyecto: [https://github.com/tu-usuario/wahub](https://github.com/tu-usuario/wahub)

## 🙏 Agradecimientos
- [.NET Foundation](https://dotnetfoundation.org/)
- [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
- [Bootstrap](https://getbootstrap.com/)
- Y a todos los contribuidores que han ayudado a hacer de WaHub una realidad.

## 🗄️ Migraciones Automáticas

WaHub incluye un sistema robusto de migraciones automáticas que se ejecuta en producción para mantener la base de datos actualizada sin intervención manual.

### ✨ Características

- ✅ **Automático en Producción**: Se ejecuta automáticamente al iniciar la aplicación
- ✅ **Seguro para Desarrollo**: Deshabilitado en entorno Development
- ✅ **Logs Detallados**: Registra todo el proceso de migración
- ✅ **Manejo de Errores**: Continúa la ejecución incluso si las migraciones fallan (configurable)
- ✅ **Validación de Conectividad**: Verifica la conexión antes de ejecutar migraciones

### 🛠️ Configuración

**Configurar migraciones automáticas:**
```json
{
  "Database": {
    "AutoMigrate": true,
    "FailOnMigrationError": false
  }
}
```

**Variables de entorno para Docker:**
```bash
Database__AutoMigrate=true
Database__FailOnMigrationError=false
```

### 📋 Comandos de Migración

```bash
# Crear nueva migración
dotnet ef migrations add NombreMigration --project src/WaHub

# Ver migraciones disponibles
dotnet ef migrations list --project src/WaHub

# Aplicar migraciones manualmente
dotnet ef database update --project src/WaHub

# Probar migraciones localmente
.\deploy.ps1 test-migration
```

Para más información detallada, consulta [MIGRATIONS-CONFIG.md](MIGRATIONS-CONFIG.md).

---

<div align="center">
  <sub>Hecho con ❤️ por el equipo de WaHub</sub>
</div>
