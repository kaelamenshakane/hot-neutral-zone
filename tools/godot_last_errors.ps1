param(
    [string]$LogFile = "logs/godot_live.log"
)

$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot
Set-Location $root

if (-not (Test-Path -LiteralPath $LogFile)) {
    throw "Log file not found: $LogFile"
}

$patterns = @(
    "E 0:",
    "ERROR",
    "SCRIPT ERROR",
    "Exception",
    "Unhandled",
    "Condition \""
)

$regex = ($patterns -join "|")
$hits = Select-String -Path $LogFile -Pattern $regex

if (-not $hits) {
    Write-Host "No error-like lines found in $LogFile"
    exit 0
}

Write-Host "Error-like lines from $LogFile:"
$hits | ForEach-Object { $_.Line }
