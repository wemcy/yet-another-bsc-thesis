# Development setup

This guide lists everything needed to work with all parts of this repository:

- runtime app stack (frontend/backend/proxy/database)
- API generation
- versioning scripts
- documentation
- thesis LaTeX build

## 1. Required software

## Core

1. Git
2. PowerShell 7+
3. Docker Desktop (with Docker Compose v2 and Buildx)
4. Node.js 20+ (Node 22 recommended)
5. pnpm 10+
6. .NET SDK 10
7. Java 17+ (required by OpenAPI Generator CLI)

## Optional but recommended

1. VS Code
2. Docker extension(s) for VS Code
3. LaTeX Workshop extension for VS Code

## Docs and thesis extras

1. DocFX (for docs site generation)
2. TeX Live (or MiKTeX) with `pdflatex` and `biber`

## Quick install commands (Windows, winget)

You can install most prerequisites with:

```powershell
winget install --id Git.Git -e
winget install --id Microsoft.PowerShell -e
winget install --id Docker.DockerDesktop -e
winget install --id OpenJS.NodeJS.LTS -e
winget install --id Microsoft.DotNet.SDK.10 -e
winget install --id EclipseAdoptium.Temurin.17.JDK -e
```

Install pnpm:

```powershell
npm install -g pnpm
```

For LaTeX on Windows (choose one):

```powershell
winget install --id MiKTeX.MiKTeX -e
```

or TeX Live from the official installer.

Install DocFX (global .NET tool):

```powershell
dotnet tool install -g docfx
```

## 2. Verify installation

Run from a terminal:

```powershell
git --version
pwsh --version
docker --version
docker compose version
docker buildx version
node --version
pnpm --version
dotnet --version
java --version
```

For thesis build:

```powershell
pdflatex --version
biber --version
```

## 3. Clone and bootstrap

```powershell
git clone https://github.com/wemcy/yet-another-bsc-thesis.git
cd yet-another-bsc-thesis
```

Install .NET local tools (GitVersion):

```powershell
dotnet tool restore
```

Install frontend workspace deps:

```powershell
cd src/frontend
pnpm install
cd ../..
```

## 4. Run the app (Docker-first workflow)

Start development stack:

```powershell
./dev.ps1
```

Open:

- App: http://localhost:9393
- Adminer: http://localhost:8082

Optional HTTPS at the proxy:

1. Put your certificate and key files into the repository root `certs/` directory.
2. Set `PROXY_TLS_ENABLED=true` in `.env`.
3. If needed, change `PROXY_TLS_CERTIFICATE_FILE` and `PROXY_TLS_CERTIFICATE_KEY_FILE` to match your filenames.
4. Re-run the stack and open `https://localhost:9393`.

Stop stack:

```powershell
docker compose --env-file gitversion.env -f docker-compose.yaml -f docker-compose-build.yaml -f docker-compose-dev.yaml down
```

## 5. Local (non-Docker) development

Use this if you want to run backend/frontend directly on host.

## Backend local

Backend project path:

- `src/backend/src/Wemcy.RecipeApp.Backend`

Backend requires a connection string because migrations run at startup.

Example (PostgreSQL):

```powershell
cd src/backend/src/Wemcy.RecipeApp.Backend
dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Port=5432;Database=recipe-app;Username=admin;Password=admin"
dotnet run
```

Default dev profile URL from launch settings:

- http://localhost:5008

## Frontend local

Frontend project path:

- `src/frontend/recipe-web-frontend`

Run:

```powershell
cd src/frontend/recipe-web-frontend
pnpm dev
```

By default Vite serves on localhost:5173.

## 6. API generation workflow

Generate backend/server stubs and frontend API client from OpenAPI:

```powershell
./scripts/generate_api.ps1
```

This script installs OpenAPI Generator CLI globally if needed.

Regenerate API and build frontend API client package:

```powershell
./scripts/update_api.ps1
```

## 7. Versioning workflow

Generate `gitversion.env` and update project/package versions:

```powershell
./scripts/generate_versions.ps1
```

## 8. Build and publish workflows

Refresh generated assets and print Docker buildx bake plan:

```powershell
./build.ps1
```

Note: this script currently does not execute the image build, it prints the bake plan.

Build and push images:

```powershell
./publish.ps1
```

Run production-like compose locally:

```powershell
./prod.ps1
```

## 9. Documentation workflow

## API docs source

- OpenAPI source: `docs/api/recipe_api.yaml`

## DocFX site

DocFX config:

- `docs/docfx.json`

If `docfx` is installed:

```powershell
cd docs
docfx docfx.json --serve
```

## Bruno API collections

- `docs/bruno/api-calls`

Optional recipe call generators:

- `scripts/generate_recipes.ps1`
- `scripts/create_100_recipe_calls.ps1`

## 10. Thesis LaTeX workflow

Thesis root:

- `docs/thesis-latex`

Build thesis PDF:

```powershell
./docs/thesis-latex/build.ps1
```

Output:

- `docs/thesis-latex/build/elteikthesis_hu.pdf`

Clean thesis build artifacts:

```powershell
./docs/thesis-latex/build.ps1 -Clean
```

## 11. Troubleshooting

## `openapi-generator-cli` fails

- Ensure Java 17+ is installed and available on PATH.
- Re-run `pnpm add -g @openapitools/openapi-generator-cli`.

## Backend fails at startup with DB error

- Check `ConnectionStrings__Default` (Docker) or user-secrets (local run).
- Ensure PostgreSQL is reachable.

## Frontend cannot call API

- If using Docker stack, call through proxy on `http://localhost:9393/api`.
- Check proxy container is up and healthy.

## HTTPS proxy fails to start

- Ensure `certs/` exists in the repository root.
- Ensure the certificate and key filenames in `.env` match files inside `certs/`.
- If `PROXY_TLS_ENABLED=true`, the proxy will fail fast when either file is missing.

## LaTeX build fails

- Verify both `pdflatex` and `biber` are installed and on PATH.
- Re-run build after fixing any `.tex` compile errors.
