# Godot Rules for Codex

## General
This is a Godot project. Preserve current Godot conventions.

Godot uses scenes, nodes, resources, scripts, and filesystem paths heavily.
Path stability matters.

## File and naming rules
- Use snake_case for file and folder names.
- Use PascalCase for node names.
- Keep scene names and script names consistent with nearby project style.
- Do not rename resources unless the task explicitly requires it.
- Do not move files unless the task explicitly requires it.

## Generated files
Do not edit:
- .godot/
- imported/generated cache files
- temporary editor files

## Scenes and resources
Be careful with:
- .tscn
- .tres
- .res
- project.godot

Manual edits to .tscn/.tres are allowed only when they are the smallest safe change.
Prefer editor-generated structure when possible.
If editing them manually, keep diffs minimal and check references.

## Scripts
Before changing a script:
- inspect nearby scripts for style
- preserve signal names
- preserve exported variable names unless change is required
- preserve node paths unless change is required
- preserve input action names unless change is required

## Input
Do not add or rename input actions without calling it out.
Changes to input map are project-level decisions.

## Autoloads
Do not add singletons/autoloads casually.
Adding an autoload is an architecture decision.

## Physics
Do not change collision layers or masks casually.
Changing them can break unrelated scenes.

## Verification commands
Prefer commands from docs/ai/working_state.md.
If commands fail because Godot is missing locally, report that directly.
