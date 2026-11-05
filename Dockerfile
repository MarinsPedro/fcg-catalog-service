# syntax=docker/dockerfile:1.7

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080
USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /workspace
COPY --from=buildingblocks / /workspace/fcg-dotnet-building-blocks
COPY . /workspace/fcg-catalog-service
WORKDIR /workspace/fcg-catalog-service

RUN --mount=type=secret,id=nuget_config,target=/root/.nuget/NuGet/NuGet.Config,required=true \
    dotnet restore FiapCloudGames.Catalog.slnx -p:UseLocalBuildingBlocks=true --configfile /root/.nuget/NuGet/NuGet.Config

RUN dotnet publish src/FiapCloudGames.Catalog.Api/FiapCloudGames.Catalog.Api.csproj \
    -c Release -p:UseLocalBuildingBlocks=true --no-restore -o /app/api

RUN dotnet publish src/FiapCloudGames.Catalog.Migrations/FiapCloudGames.Catalog.Migrations.csproj \
    -c Release -p:UseLocalBuildingBlocks=true --no-restore -o /app/migrations

FROM runtime AS migrations
WORKDIR /app
COPY --from=build /app/migrations .
ENTRYPOINT ["dotnet", "FiapCloudGames.Catalog.Migrations.dll"]

FROM runtime AS final
WORKDIR /app
COPY --from=build /app/api .
ENTRYPOINT ["dotnet", "FiapCloudGames.Catalog.Api.dll"]
