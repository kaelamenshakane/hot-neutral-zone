# Working State

## Engine
Godot version:
- unknown / fill in after project creation

Godot binary:
- use `godot` if available on PATH
- otherwise set local shell variable or env var `GODOT_BIN`

## Current phase
Early project setup.
No final architecture yet.

## Main runnable scene
Main menu:
- `res://Scenes/main_menu.tscn`

## Current focus
Set up the repository so Codex can safely help with Godot programming tasks.

## Known working commands
Open editor:

```bash
godot -e --path .
```

Run project:

```bash
godot --path .
```

Import check:

```bash
godot --headless --path . --import
```

Run specific scene:

```bash
godot --path . path/to/scene.tscn
```

Run project with persistent log capture:

```bash
powershell -ExecutionPolicy Bypass -File tools/godot_run_with_log.ps1
```

Extract error-like lines from captured log:

```bash
powershell -ExecutionPolicy Bypass -File tools/godot_last_errors.ps1
```

## Known broken things
- none recorded yet

## Current active task
- none

## Notes for Codex
Do not infer game architecture.
Use existing files as the source of truth.
If no relevant file exists yet, create the smallest file required by the task.
