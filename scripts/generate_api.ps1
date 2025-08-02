$apiPath = Join-Path -Path $PSScriptRoot -ChildPath "..\docs\api\recipe_api.yaml"
$configPath = Join-Path -Path $PSScriptRoot -ChildPath "..\docs\api\server_generator_config.json"
$outputPath = Join-Path -Path $PSScriptRoot -ChildPath "..\src\backend\gen"

openapi-generator-cli generate -g aspnetcore -c $configPath -i $apiPath -o $outputPath
