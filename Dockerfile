# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia todo el contenido del proyecto
COPY . .

# Restaurar paquetes
RUN dotnet restore

# Publicar con configuración de Release
RUN dotnet publish -c Release -o /out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copia la aplicación compilada
COPY --from=build /out .

# Render necesita que escuche en el puerto 10000
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Ejecutar la app
ENTRYPOINT ["dotnet", "SchoolManager.dll"]
