# Yet Another BSC Thesis

Recipe management web application and thesis project workspace.

## Project status

This repository contains:

- a running full-stack web application (frontend + backend + proxy + PostgreSQL),
- API specification and generated client/server artifacts,
- project documentation (DocFX, use cases, wireframes, Bruno collections),
- thesis LaTeX sources.

## Architecture

Runtime services (via Docker Compose):

- proxy: Nginx reverse proxy on port `9393`
- frontend: Vue 3 + Vite app served via Nginx
- backend: ASP.NET Core Web API (`net10.0`)
- recipe_database: PostgreSQL 17
- adminer: optional database UI in dev profile on port `8082`

Routing through proxy:

- `http://localhost:9393/` -> frontend
- `http://localhost:9393/api/` -> backend

Optional HTTPS:

- TLS terminates at the proxy only.
- Frontend and backend traffic inside Docker stays on HTTP.
- Set `PROXY_TLS_ENABLED=true` in `.env` and place cert files in `./certs/`.

## Tech stack

- Frontend: Vue 3, Vite, TypeScript, Pinia, Tailwind CSS, Vitest
- Backend: ASP.NET Core 10, Entity Framework Core, Npgsql, Swagger
- API tooling: OpenAPI Generator CLI
- DevOps: Docker Compose / Buildx, GitVersion
- Docs: DocFX, Bruno collections, LaTeX thesis sources

## Quick start (recommended)

Use Docker for the fastest setup:

```powershell
./dev.ps1
```

This starts the app stack with development overrides.

Useful URLs:

- App: `http://localhost:9393`
- API (via proxy): `http://localhost:9393/api`
- Swagger (through proxy): `http://localhost:9393/api/swagger`
- Adminer: `http://localhost:8082`

## Common commands

### Run development stack

```powershell
./dev.ps1
```

### Run production-like stack locally

```powershell
./prod.ps1
```

### Enable HTTPS at the proxy

1. Put your certificate and key into `./certs/`.
2. Set these values in `.env`:

```dotenv
PROXY_TLS_ENABLED=true
PROXY_TLS_CERTIFICATE_FILE=tls.crt
PROXY_TLS_CERTIFICATE_KEY_FILE=tls.key
```

3. Start the stack as usual with `./start.ps1`, `./dev.ps1`, or `./prod.ps1`.

Once enabled, use `https://localhost:9393` or `https://<APP_HOST>:<APP_PORT>`.

### Refresh generated assets and print build plan

```powershell
./build.ps1
```

### Build and push images

```powershell
./publish.ps1
```

### Regenerate API server/client code

```powershell
./scripts/generate_api.ps1
```

### Regenerate API and build frontend client package

```powershell
./scripts/update_api.ps1
```

### Regenerate versions and update project versions

```powershell
./scripts/generate_versions.ps1
```

## Development setup

See full prerequisite and setup guide in:

- [DEVELOPMENT_SETUP.md](DEVELOPMENT_SETUP.md)

## Repository structure

- `src/backend`: backend solution and API projects
- `src/frontend`: frontend app and generated API client workspace
- `src/proxy`: reverse proxy (Nginx)
- `docs/api`: OpenAPI spec and generator configs
- `docs/bruno`: API call collections
- `docs/usecases`: use case documentation
- `docs/wireframes`: UI wireframes
- `docs/thesis-latex`: thesis sources and build script

## Notes

- Backend startup runs EF Core migration at app boot.
- Backend expects a `ConnectionStrings__Default` value (provided in compose configs).
- `scripts/generate_api.ps1` installs OpenAPI Generator CLI globally with pnpm.
- `build.ps1` currently prints Docker buildx bake output, it does not execute the image build.
