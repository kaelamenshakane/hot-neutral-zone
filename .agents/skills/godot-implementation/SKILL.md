---
name: godot-implementation
description: Use for Godot implementation tasks: GDScript changes, scene wiring, resource wiring, bug fixes, small refactors, input handling, UI hookups, and gameplay prototypes. Do not use for broad game design, content taxonomy, or architecture invention.
---

Before editing:
1. Read AGENTS.md.
2. Read docs/ai/working_state.md.
3. Read docs/ai/project_context.md.
4. Read docs/ai/godot_rules.md.
5. Inspect nearby files before creating new patterns.

Task handling:
1. Convert the user request into a minimal Godot implementation target.
2. Identify touched files before editing.
3. Reuse existing scene/script/resource patterns.
4. Avoid introducing new architecture.
5. Avoid adding autoloads unless explicitly requested.
6. Avoid changing project.godot unless explicitly requested.
7. Keep .tscn/.tres diffs minimal.
8. Update docs/ai/working_state.md only if project state actually changed.

Verification:
- Run the narrowest command from docs/ai/working_state.md.
- If verification cannot run, state why.
- Do not claim success beyond what was checked.

Return:
- implementation summary
- touched files
- verification run
- risks / manual checks
