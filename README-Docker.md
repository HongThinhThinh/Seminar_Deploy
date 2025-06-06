# Demo2 API - Docker Deployment

## Prerequisites

- Docker Desktop installed and running
- Docker Compose (included with Docker Desktop)

## Deployment Instructions

### 1. Build and Run with Docker Compose

From the project root directory, run:

```bash
docker-compose up --build
```

This will:

- Build the .NET API container
- Start SQL Server container
- Automatically create the database and apply migrations
- Expose the API at http://localhost:5000

### 2. Access the Application

- **API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger
- **SQL Server**: localhost:1433 (from host machine)

### 3. Database Connection Details

- **Server**: localhost,1433 (from host) or sqlserver,1433 (from container)
- **Database**: demo
- **Username**: sa
- **Password**: YourStrong@Password123

### 4. Stopping the Application

```bash
docker-compose down
```

To remove volumes as well:

```bash
docker-compose down -v
```

### 5. Logs and Troubleshooting

View logs:

```bash
# All services
docker-compose logs

# Specific service
docker-compose logs demo2-api
docker-compose logs sqlserver
```

### 6. Development Mode

For development with local SQL Server, use:

```bash
dotnet run --project Controller
```

The application will use the connection string from `appsettings.Development.json`.

## Features

- ✅ Automatic database creation and migration
- ✅ SQL Server in Docker container
- ✅ Health checks for dependencies
- ✅ Persistent data volumes
- ✅ Swagger documentation
- ✅ Production-ready configuration
