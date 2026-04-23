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

Write-Host '[1/4] Build check...'
dotnet build hotline.sln | Out-Host
if ($LASTEXITCODE -ne 0) {
    throw 'dotnet build failed'
}

Write-Host '[2/4] Scene checks...'
$menuScene = Join-Path $root 'Scenes/main_menu.tscn'
Assert-Path $menuScene
Assert-Pattern -Path $menuScene -Pattern '[node name="NewGame" type="Button"' -Reason 'NewGame button exists'
Assert-Pattern -Path $menuScene -Pattern '[node name="ContinueGame" type="Button"' -Reason 'ContinueGame button exists'
Assert-Pattern -Path $menuScene -Pattern '[node name="Exit" type="Button"' -Reason 'Exit button exists'
Assert-Pattern -Path $menuScene -Pattern 'mouse_filter = 2' -Reason 'Background controls ignore mouse'

$level1Scene = Join-Path $root 'Scenes/level_1.tscn'
$level2Scene = Join-Path $root 'Scenes/level_2.tscn'
$level3Scene = Join-Path $root 'Scenes/level_3.tscn'
$levelHudScene = Join-Path $root 'Scenes/level_hud.tscn'
$weaponPickupScene = Join-Path $root 'Scenes/weapon_pickup.tscn'
$projectileScene = Join-Path $root 'Scenes/projectile.tscn'
Assert-Path $level1Scene
Assert-Path $level2Scene
Assert-Path $level3Scene
Assert-Path $levelHudScene
Assert-Path $weaponPickupScene
Assert-Path $projectileScene
Assert-Pattern -Path $level1Scene -Pattern 'LevelNumber = 1' -Reason 'Level 1 controller index exists'
Assert-Pattern -Path $level2Scene -Pattern 'LevelNumber = 2' -Reason 'Level 2 controller index exists'
Assert-Pattern -Path $level3Scene -Pattern 'LevelNumber = 3' -Reason 'Level 3 controller index exists'
Assert-Pattern -Path $level1Scene -Pattern 'path="res://Scenes/enemy.tscn"' -Reason 'Level 1 has enemy scene'
Assert-Pattern -Path $level2Scene -Pattern 'path="res://Scenes/enemy.tscn"' -Reason 'Level 2 has enemy scene'
Assert-Pattern -Path $level3Scene -Pattern 'path="res://Scenes/enemy.tscn"' -Reason 'Level 3 has enemy scene'
Assert-Pattern -Path $level1Scene -Pattern 'path="res://Scenes/level_hud.tscn"' -Reason 'Level 1 has level HUD scene'
Assert-Pattern -Path $level2Scene -Pattern 'path="res://Scenes/level_hud.tscn"' -Reason 'Level 2 has level HUD scene'
Assert-Pattern -Path $level3Scene -Pattern 'path="res://Scenes/level_hud.tscn"' -Reason 'Level 3 has level HUD scene'
Assert-Pattern -Path $level1Scene -Pattern '[node name="BatPickup" parent="." instance=ExtResource("4_weapon")]' -Reason 'Level 1 has bat pickup'
Assert-Pattern -Path $level1Scene -Pattern '[node name="PistolPickup" parent="." instance=ExtResource("4_weapon")]' -Reason 'Level 1 has pistol pickup'
Assert-Pattern -Path $level2Scene -Pattern '[node name="BatPickup" parent="." instance=ExtResource("5_weapon")]' -Reason 'Level 2 has bat pickup'
Assert-Pattern -Path $level2Scene -Pattern '[node name="PistolPickup" parent="." instance=ExtResource("5_weapon")]' -Reason 'Level 2 has pistol pickup'
Assert-Pattern -Path $level3Scene -Pattern '[node name="BatPickup" parent="." instance=ExtResource("5_weapon")]' -Reason 'Level 3 has bat pickup'
Assert-Pattern -Path $level3Scene -Pattern '[node name="PistolPickup" parent="." instance=ExtResource("5_weapon")]' -Reason 'Level 3 has pistol pickup'
Assert-Pattern -Path $level1Scene -Pattern 'Kind = 1' -Reason 'Bat pickup exports bat kind'
Assert-Pattern -Path $level1Scene -Pattern 'Kind = 2' -Reason 'Pistol pickup exports pistol kind'
Assert-Pattern -Path $level2Scene -Pattern 'Kind = 1' -Reason 'Level 2 bat pickup exports bat kind'
Assert-Pattern -Path $level2Scene -Pattern 'Kind = 2' -Reason 'Level 2 pistol pickup exports pistol kind'
Assert-Pattern -Path $level3Scene -Pattern 'Kind = 1' -Reason 'Level 3 bat pickup exports bat kind'
Assert-Pattern -Path $level3Scene -Pattern 'Kind = 2' -Reason 'Level 3 pistol pickup exports pistol kind'
Assert-Pattern -Path $levelHudScene -Pattern '[node name="DeathPanel" type="PanelContainer" parent="."]' -Reason 'Level HUD has death panel'
Assert-Pattern -Path $levelHudScene -Pattern '[node name="PausePanel" type="PanelContainer" parent="."]' -Reason 'Level HUD has pause panel'
Assert-Pattern -Path $levelHudScene -Pattern '[node name="WeaponStatus" type="Label" parent="."]' -Reason 'Level HUD has weapon status'

Write-Host '[3/4] Script wiring checks...'
$menuScript = Join-Path $root 'MainMenu.cs'
Assert-Path $menuScript
Assert-Pattern -Path $menuScript -Pattern 'GetNode<Button>("NewGame")' -Reason 'MainMenu wires NewGame'
Assert-Pattern -Path $menuScript -Pattern 'GetNode<Button>("ContinueGame")' -Reason 'MainMenu wires ContinueGame'
Assert-Pattern -Path $menuScript -Pattern 'GetNode<Button>("Exit")' -Reason 'MainMenu wires Exit'
Assert-Pattern -Path $menuScript -Pattern 'OnNewGamePressed' -Reason 'New game handler exists'
Assert-Pattern -Path $menuScript -Pattern 'OnContinuePressed' -Reason 'Continue handler exists'
Assert-Pattern -Path $menuScript -Pattern 'OnResetSavePressed' -Reason 'Reset save handler exists'
Assert-Pattern -Path $menuScript -Pattern 'LoadLevel(levelNumber)' -Reason 'Level select loads levels'
Assert-Pattern -Path $menuScript -Pattern 'OnQuitPressed' -Reason 'Quit handler exists'

$gameManagerScript = Join-Path $root 'Scripts/GameManager.cs'
$levelControllerScript = Join-Path $root 'Scripts/LevelController.cs'
$playerScript = Join-Path $root 'Scripts/PlayerController.cs'
$hurtboxScript = Join-Path $root 'Scripts/Hurtbox.cs'
$levelHudScript = Join-Path $root 'Scripts/LevelHud.cs'
$weaponKindScript = Join-Path $root 'Scripts/WeaponKind.cs'
$weaponPickupScript = Join-Path $root 'Scripts/WeaponPickup.cs'
$projectileScript = Join-Path $root 'Scripts/Projectile.cs'
$playerScene = Join-Path $root 'Scenes/player.tscn'
$projectConfig = Join-Path $root 'project.godot'
Assert-Path $gameManagerScript
Assert-Path $levelControllerScript
Assert-Path $playerScript
Assert-Path $hurtboxScript
Assert-Path $levelHudScript
Assert-Path $weaponKindScript
Assert-Path $weaponPickupScript
Assert-Path $projectileScript
Assert-Path $playerScene
Assert-Path $projectConfig
Assert-Pattern -Path $projectConfig -Pattern 'GameManager="*res://Scripts/GameManager.cs"' -Reason 'GameManager autoload uses script path'
Assert-Pattern -Path $projectConfig -Pattern 'pickup_throw={' -Reason 'Pickup/throw input action exists'
Assert-Pattern -Path $projectConfig -Pattern 'finish_enemy={' -Reason 'Finish enemy input action exists'
Assert-Pattern -Path $projectConfig -Pattern 'look_ahead={' -Reason 'Look ahead input action exists'
Assert-Pattern -Path $projectConfig -Pattern 'lock_on={' -Reason 'Lock-on input action exists'
Assert-Pattern -Path $projectConfig -Pattern 'pause_menu={' -Reason 'Pause/menu input action exists'
Assert-Pattern -Path $gameManagerScript -Pattern 'user://save.json' -Reason 'GameManager uses persistent save path'
Assert-Pattern -Path $gameManagerScript -Pattern 'StartNewGame' -Reason 'GameManager can start new game'
Assert-Pattern -Path $gameManagerScript -Pattern 'LoadLevel' -Reason 'GameManager can load levels'
Assert-Pattern -Path $gameManagerScript -Pattern 'CompleteLevel' -Reason 'GameManager can complete levels'
Assert-Pattern -Path $gameManagerScript -Pattern 'ResetSave' -Reason 'GameManager can reset save'
Assert-Pattern -Path $gameManagerScript -Pattern 'CallDeferred(MethodName.RestartCurrentSceneDeferred)' -Reason 'Restart is deferred out of physics callbacks'
Assert-Pattern -Path $gameManagerScript -Pattern 'ReturnToMainMenu' -Reason 'GameManager can return to main menu'
Assert-Pattern -Path $levelControllerScript -Pattern 'CompleteLevel(LevelNumber)' -Reason 'LevelController completes level'
Assert-Pattern -Path $hurtboxScript -Pattern 'EmitSignal(SignalName.Killed, GetParent())' -Reason 'Player hurtbox emits death instead of immediate reload'
Assert-Pattern -Path $levelHudScript -Pattern 'Input.IsActionJustPressed("restart")' -Reason 'Level HUD handles death restart'
Assert-Pattern -Path $levelHudScript -Pattern 'Input.IsActionJustPressed("pause_menu")' -Reason 'Level HUD handles pause menu action'
Assert-Pattern -Path $levelHudScript -Pattern 'ReturnToMainMenu' -Reason 'Level HUD can return to main menu'
Assert-Pattern -Path $levelHudScript -Pattern 'UpdateWeaponStatus' -Reason 'Level HUD updates weapon status'
Assert-Pattern -Path $playerScript -Pattern 'Input.IsActionJustPressed("pickup_throw")' -Reason 'Player handles pickup/throw action'
Assert-Pattern -Path $playerScript -Pattern 'ShootPistol' -Reason 'Player can shoot pistol'
Assert-Pattern -Path $playerScript -Pattern 'WeaponKind.Bat' -Reason 'Player can use bat'
Assert-Pattern -Path $playerScript -Pattern 'WeaponChangedEventHandler' -Reason 'Player emits weapon changed signal'
Assert-Pattern -Path $projectileScript -Pattern 'hurtbox.ApplyHit()' -Reason 'Projectile can damage hurtbox'
Assert-Pattern -Path $weaponPickupScript -Pattern 'public WeaponKind Kind' -Reason 'Weapon pickup exports weapon kind'
Assert-Pattern -Path $playerScene -Pattern '[node name="PickupArea" type="Area2D" parent="."]' -Reason 'Player has pickup area'
Assert-Pattern -Path $playerScene -Pattern '[node name="HeldWeaponVisual" type="Polygon2D" parent="AimPivot"]' -Reason 'Player has held weapon visual'

Write-Host '[4/4] Enemy checks...'
$enemyScene = Join-Path $root 'Scenes/enemy.tscn'
Assert-Path $enemyScene
Assert-Pattern -Path $enemyScene -Pattern '[node name="RedOutline" type="Sprite2D" parent="."]' -Reason 'Enemy has red outline node'
Assert-Pattern -Path $enemyScene -Pattern 'path="res://player.png"' -Reason 'Enemy uses player sprite texture'
Assert-Pattern -Path $enemyScene -Pattern 'path="res://Scripts/Hurtbox.cs"' -Reason 'Enemy hurtbox has Hurtbox script resource'
Assert-Pattern -Path $enemyScene -Pattern 'script = ExtResource("3_hurtbox")' -Reason 'Enemy Hurtbox node uses Hurtbox script'
Assert-Pattern -Path $enemyScene -Pattern 'modulate = Color(1, 0, 0, 1)' -Reason 'Enemy outline is red'
Assert-Pattern -Path $enemyScene -Pattern 'shape = SubResource("CircleShape2D_vision")' -Reason 'Enemy vision shape exists'
Assert-Pattern -Path $enemyScene -Pattern 'shape = SubResource("RectangleShape2D_hurtbox")' -Reason 'Enemy hurtbox shape exists'
Assert-Pattern -Path $enemyScene -Pattern 'shape = SubResource("CircleShape2D_attack")' -Reason 'Enemy attack shape exists'

Write-Host 'SMOKE TEST PASSED'
exit 0
