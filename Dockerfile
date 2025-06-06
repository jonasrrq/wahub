# Consulte https://aka.ms/customizecontainer para aprender a personalizar su contenedor de depuración y cómo Visual Studio usa este Dockerfile para compilar sus imágenes para una depuración más rápida.

# Esta fase se usa cuando se ejecuta desde VS en modo rápido (valor predeterminado para la configuración de depuración)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base

# Configurar localización y timezone para evitar problemas de globalización
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV LC_ALL=en_US.UTF-8
ENV LANG=en_US.UTF-8
RUN apk add --no-cache icu-libs icu-data-full

USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase se usa para compilar el proyecto de servicio
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

# Configurar localización para build
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk add --no-cache icu-libs icu-data-full

ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WaHub/WaHub.csproj", "src/WaHub/"]
COPY ["src/WaHub.Shared/WaHub.Shared.csproj", "src/WaHub.Shared/"]
COPY ["src/WaHub.Client/WaHub.Client.csproj", "src/WaHub.Client/"]
RUN dotnet restore "./src/WaHub/WaHub.csproj"
COPY . .
WORKDIR "/src/src/WaHub"
RUN dotnet build "./WaHub.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase se usa para publicar el proyecto de servicio que se copiará en la fase final.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WaHub.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase se usa en producción o cuando se ejecuta desde VS en modo normal (valor predeterminado cuando no se usa la configuración de depuración)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WaHub.dll"]