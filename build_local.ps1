$ErrorActionPreference = 'Stop'
./scripts/generate_api.ps1
./scripts/generate_versions.ps1
Push-Location
Set-Location .\src\frontend
pnpm install 
pnpm recursive run --sort build
Pop-Location
docker compose --env-file gitversion.env --env-file .env -f docker-compose.yaml -f docker-compose-build.yaml build --platform linux/amd64