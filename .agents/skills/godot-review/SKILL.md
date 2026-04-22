---
name: godot-review
description: Use when reviewing Godot diffs before commit or merge. Focus on regressions, unsafe Godot file edits, scene/resource path breakage, lifecycle mistakes, signals, input map changes, autoloads, and accidental architecture creep.
---

Review focus:
1. Did the diff solve the requested task?
2. Did it touch more files than necessary?
3. Did it edit .tscn/.tres safely?
4. Did it change resource paths, node names, signals, exported variables, input actions, collision layers, autoloads, or project.godot?
5. Did it introduce architecture not requested?
6. Does the code follow nearby Godot/GDScript style?
7. Is there a simple manual test path?

Do not review as a game designer unless asked.
Do not suggest new mechanics.
Do not request broad refactors unless there is a concrete risk.

Return:
- blocking issues
- non-blocking issues
- Godot-specific risks
- suggested verification
