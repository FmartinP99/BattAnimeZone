
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BattAnimeZone/BattAnimeZone/BattAnimeZone.csproj", "BattAnimeZone/BattAnimeZone/"]
COPY ["BattAnimeZone/BattAnimeZone.Shared/BattAnimeZone.Shared.csproj", "BattAnimeZone/BattAnimeZone.Shared/"]
COPY ["BattAnimeZone/BattAnimeZone.Client/BattAnimeZone.Client.csproj", "BattAnimeZone/BattAnimeZone.Client/"]
RUN dotnet restore "BattAnimeZone/BattAnimeZone/BattAnimeZone.csproj"
COPY . .
WORKDIR "/src/BattAnimeZone/BattAnimeZone"

RUN dotnet build "./BattAnimeZone.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BattAnimeZone.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BattAnimeZone.dll"]