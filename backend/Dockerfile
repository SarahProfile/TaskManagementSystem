# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY TaskManagementSystem.sln ./
COPY src/TaskManagement.Domain/TaskManagement.Domain.csproj ./src/TaskManagement.Domain/
COPY src/TaskManagement.Application/TaskManagement.Application.csproj ./src/TaskManagement.Application/
COPY src/TaskManagement.Infrastructure/TaskManagement.Infrastructure.csproj ./src/TaskManagement.Infrastructure/
COPY src/TaskManagement.API/TaskManagement.API.csproj ./src/TaskManagement.API/

# Restore dependencies
RUN dotnet restore

# Copy everything else
COPY . .

# Build
WORKDIR /src/src/TaskManagement.API
RUN dotnet build -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "TaskManagement.API.dll"]
