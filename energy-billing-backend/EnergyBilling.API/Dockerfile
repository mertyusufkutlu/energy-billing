# Uygulama çalıştırma için base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# SDK ile build katmanı
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Proje dosyalarının kopyalanması (her katmanın bağımsız olması için)
COPY ["EnergyBilling.API/EnergyBilling.API.csproj", "EnergyBilling.API/"]
COPY ["EnergyBilling.Application/EnergyBilling.Application.csproj", "EnergyBilling.Application/"]
COPY ["EnergyBilling.Domain/EnergyBilling.Domain.csproj", "EnergyBilling.Domain/"]
COPY ["EnergyBilling.Infrastructure/EnergyBilling.Infrastructure.csproj", "EnergyBilling.Infrastructure/"]
COPY ["EnergyBilling.Shared/EnergyBilling.Shared.csproj", "EnergyBilling.Shared/"]

# Nuget paketlerinin restore edilmesi
RUN dotnet restore "EnergyBilling.API/EnergyBilling.API.csproj"

# Tüm kaynak kodları kopyala
COPY . .

# API projesine geç
WORKDIR "/src/EnergyBilling.API"

# Build işlemi
RUN dotnet build "EnergyBilling.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Yayın (publish) işlemi
FROM build AS publish
RUN dotnet publish "EnergyBilling.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image: çalıştırılabilir hal
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Uygulamanın başlatılması
ENTRYPOINT ["dotnet", "EnergyBilling.API.dll"]
