# Backend API Generation Script
# This script generates the backend API code using OpenAPI Generator CLI.
$apiPath = Join-Path -Path $PSScriptRoot -ChildPath "..\docs\api\recipe_api.yaml"
$configPath = Join-Path -Path $PSScriptRoot -ChildPath "..\docs\api\server_generator_config.json"
$outputPath = Join-Path -Path $PSScriptRoot -ChildPath "..\src\backend\gen"

openapi-generator-cli generate -g aspnetcore -c $configPath -i $apiPath -o $outputPath

# Frontend Client Generation Script
# This script generates the frontend API client code using OpenAPI Generator CLI.
$apiPath = Join-Path -Path $PSScriptRoot -ChildPath "..\docs\api\recipe_api.yaml"
$configPath = Join-Path -Path $PSScriptRoot -ChildPath "..\docs\api\client_generator_config.json"
$outputPath = Join-Path -Path $PSScriptRoot -ChildPath "..\src\frontend\recipe-api-client\gen"

openapi-generator-cli generate -g typescript-fetch -c $configPath -i $apiPath -o $outputPath