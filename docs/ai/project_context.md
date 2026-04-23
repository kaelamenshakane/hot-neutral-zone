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
  - attack: left mouse
  - restart: R

## Current implemented shape
- Main menu exists with animated title and buttons.
- Main menu starts `res://Scenes/main_level.tscn`.
- `main_level.tscn` currently instantiates only `player.tscn`.
- Player currently has movement, mouse-facing aim pivot, melee hitbox, hurtbox, and restart.
- Enemy scene exists with patrol/search/chase/attack behavior, but is not placed in `main_level.tscn`.
- Hitbox/Hurtbox model is minimal:
  - enemy dies via `QueueFree`
  - player hit reloads scene
- `GameManager` currently only reloads the current scene.

## Explicitly undecided areas
Do not invent architecture for these unless explicitly asked:
- final combat model
- level architecture
- enemy taxonomy
- weapon taxonomy
- progression
- save/load
- meta-game
- content pipeline
- automated test framework

## Agent policy summary
- Keep changes small, local, and task-bound.
- Prefer current repo truth over abstract best practice.
- Avoid speculative architecture.
- Preserve names and paths unless the task requires a change.
