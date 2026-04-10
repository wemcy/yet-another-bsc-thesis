param(
    [switch]$Clean
)

$ErrorActionPreference = 'Stop'

function Invoke-Step {
    param(
        [Parameter(Mandatory = $true)][string]$Command,
        [string[]]$Arguments = @()
    )

    & $Command @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "$Command failed with exit code $LASTEXITCODE."
    }
}

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
Push-Location $scriptDir

try {
    if ($Clean) {
        if (Test-Path build) {
            Remove-Item build -Recurse -Force
        }

        Write-Host 'Removed thesis build artifacts.'
        return
    }

    $missingTools = @(
        'pdflatex',
        'biber'
    ) | Where-Object { -not (Get-Command $_ -ErrorAction SilentlyContinue) }

    if ($missingTools.Count -gt 0) {
        $toolList = $missingTools -join ', '
        throw "Missing LaTeX tools: $toolList. Install TeX Live or MiKTeX with biber support, then rerun this script."
    }

    New-Item -ItemType Directory -Path build -Force | Out-Null

    $pdfLatexArgs = @('-interaction=nonstopmode', '-file-line-error', '-output-directory=build', 'elteikthesis_hu.tex')

    Invoke-Step -Command 'pdflatex' -Arguments $pdfLatexArgs
    Invoke-Step -Command 'biber' -Arguments @('--input-directory=build', '--output-directory=build', 'elteikthesis_hu')
    Invoke-Step -Command 'pdflatex' -Arguments $pdfLatexArgs
    Invoke-Step -Command 'pdflatex' -Arguments $pdfLatexArgs

    Write-Host "Built PDF: $scriptDir\build\elteikthesis_hu.pdf"
}
finally {
    Pop-Location
}