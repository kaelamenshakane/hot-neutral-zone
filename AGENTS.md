# AGENTS.md

## Project
This is a Godot game project developed by two people in Git.

Stable known facts:
- Engine: Godot.
- Visual direction: pixel art.
- Broad reference: fast top-down violent action in the lineage of Hotline Miami.
- Current phase: early development / pre-architecture.
- Codex model target: gpt-5.4 with xhigh reasoning.

## Core instruction
Do not invent the internal game architecture before it exists.

Codex must help implement requested Godot tasks while preserving the current repo shape.
When a task lacks design detail, ask for the smallest missing implementation fact or implement a neutral placeholder only if explicitly allowed.

## What not to do
Do not create large design systems unless requested.
Do not create a combat model, progression model, content registry, enemy taxonomy, weapon taxonomy, save system, inventory system, or level architecture unless the task explicitly asks for it.
Do not turn a small implementation request into a full framework.
Do not rename scenes, scripts, resources, nodes, signals, input actions, or autoloads unless required.
Do not reorganize folders proactively.

## Source of truth order
When facts conflict, use this order:
1. docs/ai/working_state.md
2. docs/ai/project_context.md
3. docs/ai/godot_rules.md
4. docs/ai/decisions.md
5. the existing Godot project files
6. comments in code

## Required reading before coding
Before editing code or scenes, read:
- docs/ai/working_state.md
- docs/ai/project_context.md
- docs/ai/godot_rules.md

If the user gives a task packet, read:
- docs/ai/task_packet.md

## Godot behavior
Prefer small, local edits.
Prefer current project conventions over external opinions.
When creating files, use snake_case for file and folder names.
When creating Godot nodes, use PascalCase names.
When adding GDScript, keep it readable, typed where the surrounding code is typed, and consistent with nearby scripts.
Do not hand-edit generated/cache folders.
Avoid broad manual rewrites of .tscn and .tres files.
If a .tscn or .tres edit is necessary, explain why and keep the diff minimal.

## Dangerous changes
Stop and plan first before changing:
- project.godot
- input map
- autoloads/singletons
- physics layers/masks
- import settings
- export settings
- scene ownership structure
- resource paths
- anything that renames or moves scenes/resources

## Implementation protocol
For each coding task:
1. Restate the requested change as a small implementation target.
2. Identify likely touched files.
3. Check current conventions in nearby files.
4. Make the smallest viable change.
5. Run the narrowest available verification command.
6. Report changed files, verification, and remaining risks.

## Verification
Use the commands documented in docs/ai/working_state.md.
If no Godot binary or no verification command is available, say exactly what was not run.
Never claim the project runs unless it was actually run.

## Git discipline
One task, one branch, one coherent diff.
Do not bundle unrelated cleanup.
Do not format unrelated files.
Do not commit unless explicitly asked.
