$ErrorActionPreference = 'Stop'
./scripts/generate_api.ps1
./scripts/generate_versions.ps1
Push-Location
Set-Location .\src\frontend
pnpm install 
pnpm recursive run --sort build
Pop-Location
