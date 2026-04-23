---
name: godot-implementation
description: Use for concrete Godot 4.6 C# implementation or bugfix tasks in this repo. Trigger for scene/script/resource wiring, local gameplay logic, UI wiring, C# fixes, and minimal refactors. Do not use for game design ideation, broad architecture, taxonomy creation, save or meta systems, or folder restructuring.
---

Read before editing:
- `AGENTS.md`
- `docs/ai/working_state.md`
- `docs/ai/project_context.md`
- `docs/ai/godot_rules.md`

If a task packet exists, read:
- `docs/ai/task_packet.md`

Project-specific guardrails:
- Current main scene is `res://Scenes/main_menu.tscn`.
- `main_level.tscn` currently instantiates only `player.tscn`.
- Enemy exists but is not connected to the main level by default.
- `GameManager` currently only reloads the current scene.
- Legacy `player.cs` may be old or unused and emits CS8981 warnings.
- Do not expand any of these unless the task explicitly asks.

Process:
1. Reduce the request to one small implementation target.
2. Identify the narrowest file set.
3. Inspect adjacent scene/script conventions.
4. Make the smallest viable change.
5. Prefer local edits over new abstraction.
6. Avoid changing `project.godot`, autoloads, input map, build targets, or scene routing unless required.
7. Verify with the narrowest useful step:
   - `dotnet build`
   - `powershell -ExecutionPolicy Bypass -File tools\smoke_test.ps1`
   - Godot log inspection when relevant
8. Report:
   - changed files
   - behavior changed
   - verification run
   - remaining risk / manual check

Special verification rule:
If Godot CLI is used, do not trust exit code alone.
Parse logs before claiming C# scripts loaded correctly.
