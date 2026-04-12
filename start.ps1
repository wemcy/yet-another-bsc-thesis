$ErrorActionPreference = 'Stop'

$composeFile = Join-Path $PSScriptRoot 'docker-compose.yaml'
$envFile = Join-Path $PSScriptRoot '.env'
$minDockerEngineVersion = [Version]'28.0'

function Get-NormalizedVersion {
	param(
		[Parameter(Mandatory = $true)]
		[string]$RawVersion
	)

	$match = [regex]::Match($RawVersion, '\d+(?:\.\d+){1,2}')
	if (-not $match.Success) {
		return $null
	}

	try {
		return [Version]$match.Value
	}
	catch {
		return $null
	}
}

if (-not (Test-Path $composeFile)) {
	Write-Error "Compose file not found: $composeFile"
	exit 1
}

if (-not (Test-Path $envFile)) {
	Write-Error "Env file not found: $envFile"
	exit 1
}

if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
	Write-Error 'Docker is not installed or not available in PATH.'
	exit 1
}

docker version *> $null
if ($LASTEXITCODE -ne 0) {
	Write-Error 'Docker is installed but not accessible. Ensure Docker is running and try again.'
	exit 1
}

$dockerEngineRawVersion = docker version --format '{{.Server.Version}}'
$dockerEngineVersion = Get-NormalizedVersion -RawVersion $dockerEngineRawVersion
if ($null -eq $dockerEngineVersion) {
	Write-Error "Could not determine Docker Engine version from: '$dockerEngineRawVersion'"
	exit 1
}

if ($dockerEngineVersion -lt $minDockerEngineVersion) {
	Write-Error "Docker Engine version $dockerEngineVersion is too old. Required: $minDockerEngineVersion or newer."
	exit 1
}

docker compose --env-file $envFile -f $composeFile up $args
