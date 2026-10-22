# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY backend/Meridian.Api/Meridian.Api.csproj backend/Meridian.Api/
RUN dotnet restore backend/Meridian.Api/Meridian.Api.csproj
COPY backend/ backend/
RUN dotnet publish backend/Meridian.Api/Meridian.Api.csproj -c Release -o /app

# Run
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "Meridian.Api.dll"]