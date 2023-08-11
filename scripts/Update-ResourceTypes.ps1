$resourceTypesJsonPath = "../src/repository/resourcetypes.json"

$resourceTypes = Get-Content -Path $resourceTypesJsonPath | ConvertFrom-Json -Depth 100

foreach ($resourceType in $resourceTypes) {
    Write-Host $($resourceType.id) -NoNewline
    $resourceType.id += 1000
    Write-Host " --> $($resourceType.id)"
}

$resourceTypes | ConvertTo-Json | Out-File $resourceTypesJsonPath