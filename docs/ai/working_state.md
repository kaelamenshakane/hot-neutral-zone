# Working State

This file tracks the current working snapshot.
Update it only when project state materially changes.

## Current runnable flow
- Main entry scene: `res://Scenes/main_menu.tscn`
- Main menu start action opens: `res://Scenes/main_level.tscn`
- `main_level.tscn` currently instantiates only `player.tscn`

## Implemented systems in play
- main menu with animated title and buttons
- player movement
- mouse-facing aim pivot
- melee hitbox
- player hurtbox
- restart input
- enemy behavior script exists off-level
- `GameManager` reloads current scene

## Verified commands
- `powershell -ExecutionPolicy Bypass -File tools\smoke_test.ps1` -> passes
- `dotnet build` -> succeeds
- `godot --version` -> `4.6.2.stable.official.71f334935`

## Verification caveats
- Build emits CS8981 warnings from legacy `player.cs` lowercase class.
- `godot --headless --path . --import` exits 0 but logs C# script loading errors in this environment.
- The installed Godot CLI is likely not the .NET editor/runtime.
- Do not trust exit code alone for Godot CLI C# validation.

## Preferred verification order
1. `dotnet build`
2. `powershell -ExecutionPolicy Bypass -File tools\smoke_test.ps1`
3. `powershell -ExecutionPolicy Bypass -File tools\godot_run_with_log.ps1`
4. `powershell -ExecutionPolicy Bypass -File tools\godot_last_errors.ps1`

## Current risks
- Runtime not confirmed in a working Godot .NET editor session.
- Enemy scene is disconnected from main level.
- `player.cs` appears old/unused and causes warnings.
- Manual `.tscn` edits are high-risk.

## Current hot spots
- `res://Scenes/main_menu.tscn`
- `res://Scenes/main_level.tscn`
- `res://Scenes/player.tscn`
- `GameManager` autoload
- enemy scene and enemy script
- legacy `player.cs`

## Notes for agents
- Do not connect enemy into `main_level.tscn` unless the task explicitly asks.
- Do not expand `GameManager` beyond scene reload unless the task explicitly asks.
- Do not rename legacy `player.cs` unless the task is specifically about warning cleanup or depends on it.
