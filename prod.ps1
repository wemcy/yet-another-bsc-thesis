$ErrorActionPreference = 'Stop'
docker compose --env-file gitversion.env --env-file .env -f docker-compose.yaml -f docker-compose-build.yaml up --build -d
