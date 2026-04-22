param(
    [switch]$VerboseOutput
)

$ErrorActionPreference = 'Stop'

function Assert-Path {
    param([string]$Path)
    if (-not (Test-Path -LiteralPath $Path)) {
        throw "Missing required path: $Path"
    }
}

function Assert-Pattern {
    param(
        [string]$Path,
        [string]$Pattern,
        [string]$Reason
    )

    $hit = Select-String -LiteralPath $Path -Pattern $Pattern -SimpleMatch -Quiet
    if (-not $hit) {
        throw "Check failed in ${Path}: $Reason"
    }

    if ($VerboseOutput) {
        Write-Host "OK: $Reason"
    }
}

$root = Split-Path -Parent $PSScriptRoot
Set-Location $root

Write-Host '[1/3] Build check...'
dotnet build hotline.sln | Out-Host
if ($LASTEXITCODE -ne 0) {
    throw 'dotnet build failed'
}

Write-Host '[2/3] Scene checks...'
$menuScene = Join-Path $root 'Scenes/main_menu.tscn'
Assert-Path $menuScene
Assert-Pattern -Path $menuScene -Pattern '[node name="NewGame" type="Button"' -Reason 'NewGame button exists'
Assert-Pattern -Path $menuScene -Pattern '[node name="ContinueGame" type="Button"' -Reason 'ContinueGame button exists'
Assert-Pattern -Path $menuScene -Pattern '[node name="Exit" type="Button"' -Reason 'Exit button exists'
Assert-Pattern -Path $menuScene -Pattern 'mouse_filter = 2' -Reason 'Background controls ignore mouse'

Write-Host '[3/3] Script wiring checks...'
$menuScript = Join-Path $root 'MainMenu.cs'
Assert-Path $menuScript
Assert-Pattern -Path $menuScript -Pattern 'GetNode<Button>("NewGame")' -Reason 'MainMenu wires NewGame'
Assert-Pattern -Path $menuScript -Pattern 'GetNode<Button>("ContinueGame")' -Reason 'MainMenu wires ContinueGame'
Assert-Pattern -Path $menuScript -Pattern 'GetNode<Button>("Exit")' -Reason 'MainMenu wires Exit'
Assert-Pattern -Path $menuScript -Pattern 'OnStartPressed' -Reason 'Start handler exists'
Assert-Pattern -Path $menuScript -Pattern 'OnQuitPressed' -Reason 'Quit handler exists'

Write-Host 'SMOKE TEST PASSED'
exit 0
