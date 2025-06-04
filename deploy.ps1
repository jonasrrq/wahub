# Script para despliegue y gestión de migraciones de WaHub
# Uso: .\deploy.ps1 [comando]
# Comandos disponibles: build, up, down, logs, migrate, test-migration

param(
    [Parameter(Position=0)]
    [string]$Command = "help",
    
    [Parameter()]
    [string]$Environment = "Production"
)

function Show-Help {
    Write-Host "WaHub Deployment Script" -ForegroundColor Green
    Write-Host "======================" -ForegroundColor Green
    Write-Host ""
    Write-Host "Comandos disponibles:" -ForegroundColor Yellow
    Write-Host "  build          - Construir las imágenes Docker"
    Write-Host "  up             - Levantar los servicios"
    Write-Host "  down           - Detener los servicios"
    Write-Host "  logs           - Ver logs de la aplicación"
    Write-Host "  migrate        - Ejecutar migraciones manualmente"
    Write-Host "  test-migration - Probar migraciones localmente"
    Write-Host "  clean          - Limpiar contenedores y volúmenes"
    Write-Host ""
    Write-Host "Ejemplos:" -ForegroundColor Cyan
    Write-Host "  .\deploy.ps1 build"
    Write-Host "  .\deploy.ps1 up"
    Write-Host "  .\deploy.ps1 logs"
}

function Test-EnvFile {
    if (!(Test-Path ".env")) {
        Write-Host "Archivo .env no encontrado. Creando desde .env.example..." -ForegroundColor Yellow
        if (Test-Path ".env.example") {
            Copy-Item ".env.example" ".env"
            Write-Host "Archivo .env creado. Por favor, actualiza las variables de entorno antes de continuar." -ForegroundColor Red
            notepad .env
            exit 1
        } else {
            Write-Host "Archivo .env.example no encontrado. Por favor, créalo manualmente." -ForegroundColor Red
            exit 1
        }
    }
}

function Build-Images {
    Write-Host "Construyendo imágenes Docker..." -ForegroundColor Green
    docker-compose build
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Imágenes construidas exitosamente." -ForegroundColor Green
    } else {
        Write-Host "Error al construir las imágenes." -ForegroundColor Red
        exit 1
    }
}

function Start-Services {
    Test-EnvFile
    Write-Host "Iniciando servicios..." -ForegroundColor Green
    docker-compose up -d
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Servicios iniciados exitosamente." -ForegroundColor Green
        Write-Host "La aplicación estará disponible en: http://localhost:8080" -ForegroundColor Cyan
        Write-Host "Para ver los logs: .\deploy.ps1 logs" -ForegroundColor Yellow
    } else {
        Write-Host "Error al iniciar los servicios." -ForegroundColor Red
        exit 1
    }
}

function Stop-Services {
    Write-Host "Deteniendo servicios..." -ForegroundColor Yellow
    docker-compose down
    Write-Host "Servicios detenidos." -ForegroundColor Green
}

function Show-Logs {
    Write-Host "Mostrando logs de la aplicación..." -ForegroundColor Green
    docker-compose logs -f wahub-app
}

function Run-Migrations {
    Write-Host "Ejecutando migraciones manualmente..." -ForegroundColor Green
    dotnet ef database update --project src/WaHub
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Migraciones ejecutadas exitosamente." -ForegroundColor Green
    } else {
        Write-Host "Error al ejecutar las migraciones." -ForegroundColor Red
        exit 1
    }
}

function Test-Migrations {
    Write-Host "Probando migraciones localmente..." -ForegroundColor Green
    
    # Verificar que Entity Framework Tools esté instalado
    $efVersion = dotnet ef --version 2>$null
    if (!$efVersion) {
        Write-Host "Instalando Entity Framework Tools..." -ForegroundColor Yellow
        dotnet tool install --global dotnet-ef
    }
    
    # Mostrar migraciones pendientes
    Write-Host "Migraciones disponibles:" -ForegroundColor Cyan
    dotnet ef migrations list --project src/WaHub
    
    Write-Host ""
    Write-Host "¿Deseas aplicar las migraciones? (Y/N): " -ForegroundColor Yellow -NoNewline
    $response = Read-Host
    
    if ($response -eq "Y" -or $response -eq "y") {
        Run-Migrations
    }
}

function Clean-Environment {
    Write-Host "Limpiando contenedores y volúmenes..." -ForegroundColor Yellow
    docker-compose down -v
    docker system prune -f
    Write-Host "Entorno limpiado." -ForegroundColor Green
}

# Ejecutar comando
switch ($Command.ToLower()) {
    "build" { Build-Images }
    "up" { Start-Services }
    "down" { Stop-Services }
    "logs" { Show-Logs }
    "migrate" { Run-Migrations }
    "test-migration" { Test-Migrations }
    "clean" { Clean-Environment }
    "help" { Show-Help }
    default { 
        Write-Host "Comando no reconocido: $Command" -ForegroundColor Red
        Show-Help 
    }
}
