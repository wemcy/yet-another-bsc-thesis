$ErrorActionPreference = 'Stop'
./scripts/generate_api.ps1
./scripts/generate_versions.ps1
docker compose --env-file gitversion.env -f docker-compose.yaml -f docker-compose-build.yaml build