# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copia todo el proyecto
COPY . .

# Restaura paquetes y publica en modo Release
RUN dotnet restore
RUN dotnet publish -c Release -o /out

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app

# Copia los archivos publicados
COPY --from=build /out .

# Render requiere que escuchemos en el puerto 10000
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Ejecuta el proyecto principal
ENTRYPOINT ["dotnet", "SchoolManager.dll"]
