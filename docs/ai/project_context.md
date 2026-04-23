# Project Context

This file records stable project truth.
It should change rarely.

## Identity
- Repo path: `D:\hotneutralzone`
- Project/config name: `hotline`
- Engine: Godot 4.6 C#
- Genre direction: pixel-art top-down violent action in the broad Hotline Miami lineage
- Stage: early development / pre-architecture

## Technical baseline
- C# target: `net8.0`
- Android target override: `net9.0`
- Main scene: `res://Scenes/main_menu.tscn`
- Autoload: `GameManager`
- Input map:
  - movement: WASD
  - attack / punch / shoot: left mouse
  - pickup / throw weapon: right mouse
  - finish enemy: Space
  - look ahead: Shift
  - lock-on target: middle mouse
  - restart: R
  - pause / menu: Esc

## Current implemented shape
- Main menu exists with animated title and buttons.
- Main menu starts a new campaign through `GameManager.StartNewGame()`.
- Continue opens a runtime level-select menu.
- Debug reset in the menu clears the persistent level-unlock save.
- Level unlock progress is saved to `user://save.json`.
- There are currently 3 level scenes: `res://Scenes/level_1.tscn`, `res://Scenes/level_2.tscn`, and `res://Scenes/level_3.tscn`.
- Each level scene currently contains one player and one enemy.
- Player currently has movement, mouse-facing aim pivot, melee hitbox, hurtbox, restart, weapon pickup/drop, bat melee attack, and pistol projectile attack.
- Current level scenes include `LevelHud` with weapon status, death panel, R-to-restart after death, ESC pause, and pause-menu return to main menu.
- Each current level currently contains bat and pistol pickups.
- Enemy scene exists with patrol/search/chase/attack behavior and basic collision/hurtbox/attack shapes.
- Enemy visual currently reuses the player sprite with a red outline.
- Hitbox/Hurtbox model is minimal:
  - enemy dies via `QueueFree`
  - player hit emits death and the level HUD handles restart
- Level victory currently means killing the single enemy in the level.
- `GameManager` owns level loading, level unlock progress, save reset, and current-scene reload.

## Explicitly undecided areas
Do not invent architecture for these unless explicitly asked:
- final combat model
- level architecture
- enemy taxonomy
- weapon taxonomy
- downed enemy / finish enemy behavior
- look-ahead camera behavior
- lock-on targeting behavior
- progression
- save/load beyond level unlock progress
- meta-game
- content pipeline
- automated test framework

## Agent policy summary
- Keep changes small, local, and task-bound.
- Prefer current repo truth over abstract best practice.
- Avoid speculative architecture.
- Preserve names and paths unless the task requires a change.
