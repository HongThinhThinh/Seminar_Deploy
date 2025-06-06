# Use the ASP.NET Core runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution file
COPY ["Demo2.sln", "."]

# Copy project files
COPY ["Controller/Demo2.csproj", "Controller/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Repositories/Repositories.csproj", "Repositories/"]

# Restore dependencies
RUN dotnet restore "Controller/Demo2.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/Controller"
RUN dotnet build "Demo2.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Demo2.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Wait for SQL Server to be ready and then start the application
ENTRYPOINT ["dotnet", "Demo2.dll"]
