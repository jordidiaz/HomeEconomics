FROM node:14.21.3-alpine as base-spa
COPY src/HomeEconomics/spa spa
WORKDIR /spa
RUN npm install
RUN npm run build

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
EXPOSE 6001
ENV ASPNETCORE_URLS=http://*:6001

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
COPY ["HomeEconomics.sln", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/HomeEconomics/HomeEconomics.csproj", "src/HomeEconomics/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Persistence/Persistence.csproj", "src/Persistence/"]
COPY ["test/Domain.UnitTests/Domain.UnitTests.csproj", "test/Domain.UnitTests/"]
COPY ["test/HomeEconomics.IntegrationTests/HomeEconomics.IntegrationTests.csproj", "test/HomeEconomics.IntegrationTests/"]
COPY ["test/HomeEconomics.UnitTests/HomeEconomics.UnitTests.csproj", "test/HomeEconomics.UnitTests/"]
RUN dotnet restore
COPY src src
COPY test test
RUN dotnet build "HomeEconomics.sln" -c Release
COPY --from=base-spa /spa/build src/HomeEconomics/wwwroot

FROM build AS publish
RUN dotnet publish "src/HomeEconomics/HomeEconomics.csproj" --no-restore --no-build -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeEconomics.dll"]