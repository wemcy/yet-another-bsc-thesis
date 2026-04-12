#!/usr/bin/env sh
set -eu

SCRIPT_DIR=$(CDPATH= cd -- "$(dirname -- "$0")" && pwd)
COMPOSE_FILE="$SCRIPT_DIR/docker-compose.yaml"
ENV_FILE="$SCRIPT_DIR/.env"
MIN_DOCKER_ENGINE_MAJOR=28

if [ ! -f "$COMPOSE_FILE" ]; then
  echo "Compose file not found: $COMPOSE_FILE" >&2
  exit 1
fi

if [ ! -f "$ENV_FILE" ]; then
  echo "Env file not found: $ENV_FILE" >&2
  exit 1
fi

if ! command -v docker >/dev/null 2>&1; then
  echo "Docker is not installed or not available in PATH." >&2
  exit 1
fi

if ! docker version >/dev/null 2>&1; then
  echo "Docker is installed but not accessible. Ensure Docker is running and try again." >&2
  exit 1
fi

DOCKER_ENGINE_RAW_VERSION=$(docker version --format '{{.Server.Version}}' 2>/dev/null || true)
if [ -z "$DOCKER_ENGINE_RAW_VERSION" ]; then
  echo "Could not determine Docker Engine version." >&2
  exit 1
fi

case "$DOCKER_ENGINE_RAW_VERSION" in
  [0-9]*) ;;
  *)
    echo "Could not parse Docker Engine version from: $DOCKER_ENGINE_RAW_VERSION" >&2
    exit 1
    ;;
esac

DOCKER_ENGINE_MAJOR=$(printf '%s' "$DOCKER_ENGINE_RAW_VERSION" | cut -d. -f1)
case "$DOCKER_ENGINE_MAJOR" in
  ''|*[!0-9]*)
    echo "Could not parse Docker Engine version from: $DOCKER_ENGINE_RAW_VERSION" >&2
    exit 1
    ;;
esac

if [ "$DOCKER_ENGINE_MAJOR" -lt "$MIN_DOCKER_ENGINE_MAJOR" ]; then
  echo "Docker Engine version $DOCKER_ENGINE_RAW_VERSION is too old. Required: $MIN_DOCKER_ENGINE_MAJOR or newer." >&2
  exit 1
fi

docker compose --env-file "$ENV_FILE" -f "$COMPOSE_FILE" up "$@"
