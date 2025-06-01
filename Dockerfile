# Dockerfile para Blazor WebAssembly (Standalone) ultra-minimizado
# Basado en la documentación oficial de Microsoft (.NET 9)
# https://learn.microsoft.com/en-us/aspnet/core/blazor/host-and-deploy/webassembly/?view=aspnetcore-9.0#standalone-with-docker
# https://learn.microsoft.com/en-us/aspnet/core/blazor/host-and-deploy/webassembly/nginx?view=aspnetcore-9.0

# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /src
COPY . .
RUN dotnet publish src/WaHub.Blazor/WaHub.Blazor.csproj -c Release -o /app/publish --nologo

# Etapa 2: Imagen final ultra-minimalista con Nginx
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
RUN rm -rf ./*
COPY --from=build /app/publish/wwwroot .

# Configuración Nginx para SPA (Blazor WebAssembly)
COPY nginx.conf /etc/nginx/nginx.conf

# Exponer el puerto por defecto
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
