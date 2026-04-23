# Godot Rules

This file defines local rules for safe work in this Godot 4.6 C# repo.

## Naming and file rules
- Existing names are source of truth.
- For new scenes, resources, and folders use `snake_case`.
- For new C# script files use `PascalCase` matching the class name.
- Use `PascalCase` for node names.
- For new C# code prefer:
  - `PascalCase`: classes, methods, properties, exported members, events
  - `_camelCase`: private fields
  - `camelCase`: locals and parameters
- Do not rename existing lowercase legacy files/classes unless the task explicitly targets them.

## Scene/resource safety
- Avoid manual edits to `.tscn`, `.tres`, and `.res` unless they are the smallest safe change.
- If a manual edit is necessary, keep the diff minimal and verify resource paths and node references.
- Do not move or rename scenes/resources casually.
- Path stability matters.

## Project-level danger zone
Stop and plan before changing:
- `project.godot`
- main scene routing
- input map
- autoloads
- physics layers / masks
- import settings
- export settings
- `.csproj` / `.sln`
- target frameworks
- Android target override

## C# project rules
- Preserve existing namespace style nearby.
- Do not introduce helper frameworks, service layers, registries, or generalized systems unless explicitly requested.
- Prefer direct local fixes over abstraction.
- Do not touch build target configuration unless the task is explicitly about build/runtime setup.

## Verification rules
- Prefer `dotnet build` and `tools\smoke_test.ps1` as baseline verification.
- When Godot CLI is involved, use `tools\godot_run_with_log.ps1` and inspect logs with `tools\godot_last_errors.ps1`.
- Do not trust `godot --headless --path . --import` exit code alone in this environment.

## Out of scope by default
- save systems
- progression systems
- meta-game systems
- content registries
- taxonomies
- folder restructuring
- broad cleanup passes
- speculative refactors
