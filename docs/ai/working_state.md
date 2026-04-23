# Working State

This file tracks the current working snapshot.
Update it only when project state materially changes.

## Current runnable flow
- Main entry scene: `res://Scenes/main_menu.tscn`
- New Game calls `GameManager.StartNewGame()` and opens `res://Scenes/level_1.tscn`
- Continue opens a runtime level-select menu
- Level-select buttons can load only unlocked levels
- Debug reset clears the persistent level-unlock save
- Level progression currently runs through `res://Scenes/level_1.tscn`, `res://Scenes/level_2.tscn`, and `res://Scenes/level_3.tscn`

## Implemented systems in play
- main menu with animated title and buttons
- runtime level-select menu
- persistent level-unlock save at `user://save.json`
- player movement
- mouse-facing aim pivot
- melee hitbox
- weapon pickup/drop with right mouse
- bat melee attack
- pistol projectile attack
- player hurtbox
- restart input
- full requested Hotline-like input actions are registered:
  - attack / punch / shoot
  - pickup / throw weapon
  - finish enemy
  - look ahead
  - lock-on target
  - restart level
  - pause / menu
- enemy behavior script
- enemy visual reuses the player sprite with a red outline
- one enemy per current level scene
- bat and pistol pickups on `res://Scenes/level_1.tscn`
- level victory when the single enemy is killed
- player defeat reloads current scene
- `GameManager` handles level loading, unlock progress, save reset, and current-scene reload

## Verified commands
- `powershell -ExecutionPolicy Bypass -File tools\smoke_test.ps1` -> passes
- `dotnet build` -> succeeds
- local Godot .NET headless import -> exits 0 with 0 error-like log lines
- `powershell -ExecutionPolicy Bypass -File tools\godot_last_errors.ps1 -LogFile logs\godot_dotnet_import.log` -> finds no error-like lines
- local Godot .NET binary `D:\godot-4.6.2-dotnet\Godot_v4.6.2-stable_mono_win64\Godot_v4.6.2-stable_mono_win64_console.exe` -> `4.6.2.stable.mono.official.71f334935`

## Verification caveats
- PATH `godot` may still point to a non-.NET binary.
- Do not trust exit code alone for Godot CLI C# validation.
- Local Godot .NET headless import currently exits 0 but can emit GodotTools editor timer error-like lines unrelated to project script loading.

## Preferred verification order
1. `dotnet build`
2. `powershell -ExecutionPolicy Bypass -File tools\smoke_test.ps1`
3. run Godot through local .NET binary or set `GODOT_BIN` to `D:\godot-4.6.2-dotnet\Godot_v4.6.2-stable_mono_win64\Godot_v4.6.2-stable_mono_win64_console.exe`
4. `powershell -ExecutionPolicy Bypass -File tools\godot_last_errors.ps1`

## Current risks
- Runtime flow still needs manual playthrough verification in the Godot editor.
- Bat/pistol pickup, attack, throw, and enemy kill need manual playthrough verification.
- `finish_enemy`, `look_ahead`, `lock_on`, and `pause_menu` are registered input actions but do not have gameplay behavior yet.
- `player.cs` appears old/unused.
- Manual `.tscn` edits are high-risk.
- `hotline.csproj.old` may be a local Godot-generated backup and should not be committed unless intentionally needed.

## Current hot spots
- `res://Scenes/main_menu.tscn`
- `res://Scenes/level_1.tscn`
- `res://Scenes/level_2.tscn`
- `res://Scenes/level_3.tscn`
- `res://Scenes/player.tscn`
- `res://Scenes/weapon_pickup.tscn`
- `res://Scenes/projectile.tscn`
- `GameManager` autoload
- enemy scene and enemy script
- weapon pickup/projectile scripts
- `Scripts/LevelController.cs`
- legacy `player.cs`

## Notes for agents
- Do not expand save data beyond level unlock progress unless the task explicitly asks.
- Do not add more levels unless the task explicitly asks.
- Do not expand weapon architecture beyond the current minimal Bat/Pistol layer unless the task explicitly asks.
- Do not implement downed-state, finishing, lock-on, look-ahead camera, or pause UI without an explicit task.
- Do not rename legacy `player.cs` unless the task is specifically about warning cleanup or depends on it.
