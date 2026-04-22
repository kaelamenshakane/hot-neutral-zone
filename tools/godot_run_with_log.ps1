param(
    [string]$LogFile = "logs/godot_live.log"
)

$ErrorActionPreference = 'Stop'

$root = Split-Path -Parent $PSScriptRoot
Set-Location $root

if (-not (Test-Path -LiteralPath "logs")) {
    New-Item -ItemType Directory -Path "logs" | Out-Null
}

$godotBin = if ($env:GODOT_BIN -and $env:GODOT_BIN.Trim() -ne "") { $env:GODOT_BIN } else { "godot" }

Write-Host "Launching Godot with log capture..."
Write-Host "Binary: $godotBin"
Write-Host "Log: $LogFile"

# Stdout + stderr are captured into one live log stream.
& $godotBin --path . 2>&1 | Tee-Object -FilePath $LogFile

exit $LASTEXITCODE
