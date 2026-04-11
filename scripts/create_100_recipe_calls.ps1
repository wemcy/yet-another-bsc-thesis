param(
    [string]$BaseUrl = "http://localhost:9393/api/recipes/",
    [string]$SessionCookie = "",
    [string]$CookieName = ".AspNetCore.Identity.Application",
    [int]$DelayMs = 0
)

# One-file bulk recipe creator: sends 100 POST calls to the create recipe endpoint.

$adjectives = @(
    "Smoky", "Crispy", "Creamy", "Zesty", "Roasted", "Herby", "Spicy", "Savory", "Tangy", "Golden"
)

$mains = @(
    "Chicken", "Beef", "Tofu", "Salmon", "Mushroom", "Lentil", "Pork", "Turkey", "Chickpea", "Vegetable"
)

$dishes = @(
    "Skillet", "Pasta", "Stew", "Bowl", "Curry", "Bake", "Wrap", "Salad", "Soup", "Stir Fry"
)

$units = @("g", "ml", "pcs", "tbsp")
$allergenPool = @("GLUTEN", "CRUSTACEANS", "EGGS", "FISH", "PEANUTS", "SOYBEANS", "MILK", "NUTS", "CELERY", "MUSTARD", "SESAMESEEDS", "SULPHURDIOXIDE", "LUPIN", "MOLLUSCS")

function New-RecipePayload {
    param([int]$Index)

    $adj = $adjectives[$Index % $adjectives.Count]
    $main = $mains[$Index % $mains.Count]
    $dish = $dishes[$Index % $dishes.Count]

    $allergens = @()
    if ($Index % 3 -eq 0) { $allergens += $allergenPool[$Index % $allergenPool.Count] }
    if ($Index % 5 -eq 0) { $allergens += $allergenPool[($Index + 3) % $allergenPool.Count] }
    $allergens = @($allergens | Select-Object -Unique)

    $ingredients = @(
        @{
            name = "$main base"
            quantity = 200 + ($Index % 5) * 50
            unitOfMeasurement = $units[$Index % $units.Count]
        },
        @{
            name = "Onion"
            quantity = 1 + ($Index % 3)
            unitOfMeasurement = "pcs"
        },
        @{
            name = "Olive oil"
            quantity = 1 + ($Index % 2)
            unitOfMeasurement = "tbsp"
        }
    )

    return @{
        title = "$adj $main $dish #$Index"
        description = "Auto-generated test recipe $Index for API load/data seeding"
        allergens = $allergens
        steps = @(
            "Prep ingredients",
            "Cook main component",
            "Combine and season",
            "Plate and serve"
        )
        ingredients = $ingredients
    }
}

$headers = @{
    "Accept" = "application/json"
}

$webSession = New-Object Microsoft.PowerShell.Commands.WebRequestSession
if (-not [string]::IsNullOrWhiteSpace($SessionCookie)) {
    $uri = [System.Uri]$BaseUrl
    $cookie = New-Object System.Net.Cookie($CookieName, $SessionCookie, "/", $uri.Host)
    $webSession.Cookies.Add($uri, $cookie)
}

$ok = 0
$failed = 0

for ($i = 1; $i -le 100; $i++) {
    $payload = New-RecipePayload -Index $i
    $json = $payload | ConvertTo-Json -Depth 10

    try {
        $response = Invoke-RestMethod -Method POST -Uri $BaseUrl -Headers $headers -WebSession $webSession -ContentType "application/json" -Body $json
        $ok++
        Write-Host "[$i/100] OK - $($payload.title)"
    }
    catch {
        $failed++
        $statusCode = "n/a"
        if ($_.Exception.Response -and $_.Exception.Response.StatusCode) {
            $statusCode = [int]$_.Exception.Response.StatusCode
        }
        $msg = $_.Exception.Message
        Write-Host "[$i/100] FAIL - $($payload.title) - $msg"
        if ($i -eq 1) {
            Write-Host "First failure status code: $statusCode"
            try {
                $stream = $_.Exception.Response.GetResponseStream()
                if ($stream) {
                    $reader = New-Object System.IO.StreamReader($stream)
                    $body = $reader.ReadToEnd()
                    if (-not [string]::IsNullOrWhiteSpace($body)) {
                        Write-Host "First failure response body: $body"
                    }
                    $reader.Close()
                }
            }
            catch {
                Write-Host "Could not read first failure response body."
            }
        }
    }

    if ($DelayMs -gt 0) {
        Start-Sleep -Milliseconds $DelayMs
    }
}

Write-Host ""
Write-Host "Finished. Success: $ok, Failed: $failed"
if ($failed -gt 0) {
    exit 1
}
