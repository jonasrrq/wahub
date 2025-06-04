# Script de Verificación de Docker Compose
# ========================================

Write-Host "=== WaHub Docker Compose - Verificación de Configuración ===" -ForegroundColor Green
Write-Host ""

# Verificar que existe el archivo .env
if (Test-Path ".env") {
    Write-Host "✅ Archivo .env encontrado" -ForegroundColor Green
    
    # Mostrar variables principales
    Write-Host ""
    Write-Host "📋 Variables principales cargadas:" -ForegroundColor Yellow
    
    # Leer y mostrar variables del .env (sin mostrar contraseñas completas)
    $envContent = Get-Content ".env" | Where-Object { $_ -match "^[^#].*=" }
    foreach ($line in $envContent) {
        if ($line -match "PASSWORD") {
            $parts = $line.Split("=", 2)
            $value = if ($parts[1].Length -gt 4) { "*" * ($parts[1].Length - 4) + $parts[1].Substring($parts[1].Length - 4) } else { "****" }
            Write-Host "  $($parts[0])=$value" -ForegroundColor Cyan
        } else {
            Write-Host "  $line" -ForegroundColor Cyan
        }
    }
} else {
    Write-Host "❌ Archivo .env no encontrado" -ForegroundColor Red
    Write-Host "   Ejecuta: Copy-Item .env.example .env" -ForegroundColor Yellow
    return
}

Write-Host ""
Write-Host "🔍 Verificando configuración de Docker Compose..." -ForegroundColor Yellow

# Verificar sintaxis del docker-compose.yml
try {
    $output = docker-compose config 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Sintaxis de docker-compose.yml correcta" -ForegroundColor Green
    } else {
        Write-Host "❌ Error en docker-compose.yml:" -ForegroundColor Red
        Write-Host $output -ForegroundColor Red
        return
    }
} catch {
    Write-Host "❌ Docker Compose no está disponible" -ForegroundColor Red
    Write-Host "   Instala Docker Desktop: https://www.docker.com/products/docker-desktop" -ForegroundColor Yellow
    return
}

Write-Host ""
Write-Host "🚀 Comandos disponibles:" -ForegroundColor Yellow
Write-Host "  Construir y ejecutar:  docker-compose up --build" -ForegroundColor Cyan
Write-Host "  Ejecutar en segundo plano: docker-compose up -d --build" -ForegroundColor Cyan
Write-Host "  Ver logs: docker-compose logs -f wahub-app" -ForegroundColor Cyan
Write-Host "  Detener: docker-compose down" -ForegroundColor Cyan
Write-Host ""

# Verificar puertos
Write-Host "🌐 Puertos configurados:" -ForegroundColor Yellow
$appHttpPort = (Get-Content ".env" | Select-String "APP_HTTP_PORT=").ToString().Split("=")[1]
$appHttpsPort = (Get-Content ".env" | Select-String "APP_HTTPS_PORT=").ToString().Split("=")[1]
$dbPort = (Get-Content ".env" | Select-String "DB_PORT=").ToString().Split("=")[1]

Write-Host "  Aplicación HTTP:  http://localhost:$appHttpPort" -ForegroundColor Cyan
Write-Host "  Aplicación HTTPS: https://localhost:$appHttpsPort" -ForegroundColor Cyan
Write-Host "  SQL Server:       localhost:$dbPort" -ForegroundColor Cyan
Write-Host ""

Write-Host "✨ Configuración lista para usar!" -ForegroundColor Green
Write-Host "   Las migraciones se ejecutarán automáticamente en el primer inicio." -ForegroundColor Gray
