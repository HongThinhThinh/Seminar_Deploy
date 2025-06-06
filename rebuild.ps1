Write-Host "Stopping and removing existing containers..." -ForegroundColor Yellow
docker-compose down -v

Write-Host "Building and starting containers..." -ForegroundColor Yellow
docker-compose up --build
