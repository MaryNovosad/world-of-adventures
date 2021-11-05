#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["WorldOfAdventures.Api.csproj", "WorldOfAdventures.Api/"]
RUN dotnet restore "WorldOfAdventures.Api/WorldOfAdventures.Api.csproj"
COPY . "WorldOfAdventures.Api/"
WORKDIR "/src/WorldOfAdventures.Api"
RUN dotnet build "WorldOfAdventures.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorldOfAdventures.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorldOfAdventures.Api.dll"]