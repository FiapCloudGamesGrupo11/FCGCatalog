# Dockerfile
# Multi-stage build for the whole solution: API + Application + Domain + Infrastructure + Tests
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything first to ensure Directory.Build.props / NuGet.Config and other root files are present
COPY . .

# Restore the API project (will resolve project references)
RUN dotnet restore "FiapCloudGames.API/FiapCloudGames.API.csproj"

# Build the API project (no --no-restore to ensure restore effects are used)
RUN dotnet build "FiapCloudGames.API/FiapCloudGames.API.csproj" -c Release

# (Optional) Run tests during image build — uncomment to enable test execution
# RUN dotnet test FiapCloudGames.Testes/FiapCloudGames.Testes.csproj -c Release --no-build --logger "trx;LogFileName=test-result.trx"

# Publish the API project (runtime artifact)
RUN dotnet publish "FiapCloudGames.API/FiapCloudGames.API.csproj" -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "FiapCloudGames.API.dll"]