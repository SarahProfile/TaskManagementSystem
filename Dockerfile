# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY backend/src/TaskManagement.Domain/TaskManagement.Domain.csproj backend/src/TaskManagement.Domain/
COPY backend/src/TaskManagement.Application/TaskManagement.Application.csproj backend/src/TaskManagement.Application/
COPY backend/src/TaskManagement.Infrastructure/TaskManagement.Infrastructure.csproj backend/src/TaskManagement.Infrastructure/
COPY backend/src/TaskManagement.API/TaskManagement.API.csproj backend/src/TaskManagement.API/

RUN dotnet restore backend/src/TaskManagement.API/TaskManagement.API.csproj

# Copy everything else and build
COPY backend/src/ backend/src/
WORKDIR /src/backend/src/TaskManagement.API
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Railway provides PORT environment variable
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-5000}

EXPOSE ${PORT:-5000}

ENTRYPOINT ["dotnet", "TaskManagement.API.dll"]
