$ErrorActionPreference = 'Stop'

$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
$env:DOTNET_NOLOGO = '1'

$target = [System.IO.Path]::GetFullPath((Join-Path -Path $PSScriptRoot -ChildPath "..\gitversion.env"))

dotnet tool restore
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Output $target
dotnet tool run dotnet-gitversion /verbosity Verbose /output dotenv > $target
dotnet tool run dotnet-gitversion /verbosity Verbose /updateprojectfiles

$version = dotnet tool run dotnet-gitversion /output json /showvariable FullSemVer

pnpm recursive exec -- pnpm version $version


if ($env:CI -eq 'true') {
    git tag $version
    git push origin tag $version
}