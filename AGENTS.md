# AGENTS.md

## Project
This repository is a Godot 4.6 C# game project at `D:\hotneutralzone`.
Project/config name: `hotline`.
Genre direction: pixel-art top-down violent action in the broad lineage of Hotline Miami.
Stage: early development / pre-architecture.

Primary rule:
Do not invent architecture before it exists.

## Stable technical facts
- Engine: Godot 4.6 C#.
- C# target: `net8.0`.
- Android target override: `net9.0`.
- Main scene: `res://Scenes/main_menu.tscn`.
- Autoload: `GameManager`.
- Input map:
  - movement: WASD
  - attack / punch / shoot: left mouse
  - pickup / throw weapon: right mouse
  - finish enemy: Space
  - look ahead: Shift
  - lock-on target: middle mouse
  - restart: R
  - pause / menu: Esc

## Current implemented state
- Main menu exists with animated title and buttons.
- Main menu starts a new campaign through `GameManager.StartNewGame()`.
- Continue opens a runtime level-select menu.
- Debug reset in the menu clears the persistent level-unlock save.
- Level unlock progress is saved to `user://save.json`.
- There are currently 3 level scenes:
  - `res://Scenes/level_1.tscn`
  - `res://Scenes/level_2.tscn`
  - `res://Scenes/level_3.tscn`
- Each level scene currently contains one player and one enemy.
- Player currently has:
  - movement
  - mouse-facing aim pivot
  - melee hitbox
  - weapon pickup/drop through nearby pickups
  - bat melee attack
  - pistol projectile attack
  - hurtbox
  - restart
- `level_1.tscn` currently contains bat and pistol pickups.
- Enemy scene exists with patrol/search/chase/attack behavior and basic collision/hurtbox/attack shapes.
- Enemy visual currently reuses the player sprite with a red outline.
- Hitbox/Hurtbox model is minimal:
  - enemy dies via `QueueFree`
  - player hit reloads scene
- Level victory currently means killing the single enemy in the level.
- `GameManager` owns level loading, level unlock progress, save reset, and current-scene reload.

## Not implemented / not decided
Do not fill these gaps with architecture unless explicitly requested:
- final combat model
- level architecture
- enemy taxonomy
- weapon taxonomy
- downed enemy / finish enemy behavior
- look-ahead camera behavior
- lock-on targeting behavior
- pause/menu behavior
- progression
- save/load beyond level unlock progress
- meta-game
- content pipeline
- automated test framework

## Governance
- Work must be small, local, and task-bound.
- No broad systems, registries, taxonomies, save systems, progression systems, or folder restructuring unless explicitly requested.
- Do not rename scenes, scripts, resources, nodes, signals, input actions, autoloads, resource paths, or target frameworks unless required by the task.
- Do not create tracker files, planning frameworks, PLANS.md, or backlog systems unless explicitly requested.
- Do not spawn subagents unless explicitly requested.
- Do not clean up unrelated files.
- Do not invent framework code to prepare for hypothetical future features.
- Preserve current repo shape unless the task explicitly asks for structural change.

## Source-of-truth order
1. `docs/ai/working_state.md`
2. `docs/ai/project_context.md`
3. `docs/ai/godot_rules.md`
4. `docs/ai/decisions.md`
5. existing Godot project files
6. code comments

## Required reading before editing
Read these before coding:
- `AGENTS.md`
- `docs/ai/working_state.md`
- `docs/ai/project_context.md`
- `docs/ai/godot_rules.md`

If the task packet exists, also read:
- `docs/ai/task_packet.md`

## Skill usage
Use:
- `$godot-implementation` for concrete code or scene changes
- `$godot-review` for review-only work

## Working protocol
For each task:
1. Convert the request into one small implementation target.
2. Identify the likely touched files.
3. Inspect nearby scene/script conventions before changing anything.
4. Make the smallest viable change.
5. Run the narrowest useful verification.
6. Report:
   - changed files
   - what was verified
   - remaining gaps
   - risks

Do not emit long preambles.
If the task is simple and low-risk, act.
If the task is ambiguous or high-risk, give a short plan first.

## Verification
Default verification order:
1. `dotnet build`
2. `powershell -ExecutionPolicy Bypass -File tools\smoke_test.ps1`
3. inspect logs when Godot CLI is involved:
   - `powershell -ExecutionPolicy Bypass -File tools\godot_last_errors.ps1`

Important:
- Do not trust `godot --headless --path . --import` exit code alone in this environment.
- PATH `godot` may not be the .NET editor/runtime.
- Prefer the local Godot .NET binary at `D:\godot-4.6.2-dotnet\Godot_v4.6.2-stable_mono_win64\Godot_v4.6.2-stable_mono_win64_console.exe`.
- Parse logs before claiming success for C# script loading.
- Never claim the project runs unless it was actually run.

## Current known risks
- Runtime level flow still needs manual playthrough verification in the Godot editor.
- `player.cs` appears old/unused.
- Manual `.tscn` edits are high-risk; keep diffs minimal.

## High-risk changes
Stop and plan before changing:
- `project.godot`
- autoload setup
- input map
- scene ownership / scene root structure
- resource paths
- physics layers / masks
- import settings
- export settings
- build target configuration
- main scene routing

## Existing tooling
- `tools/smoke_test.ps1`
- `tools/godot_run_with_log.ps1`
- `tools/godot_last_errors.ps1`

## Git discipline
- One task, one coherent diff.
- No unrelated cleanup in the same change.
- Do not commit unless explicitly asked.
