& "$PSScriptRoot\generate_api.ps1"
Set-Location -Path "$PSScriptRoot\..\src\frontend\recipe-api-client\"
pnpm run build 
Set-Location -Path "$PSScriptRoot\.."