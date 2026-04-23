---
name: godot-review
description: Use for reviewing diffs in this Godot 4.6 C# repo. Focus on regression risk, unsafe scene/resource edits, project-level setting changes, naming and path breakage, and accidental architecture creep. Do not use for design ideation.
---

Read before review:
- `AGENTS.md`
- `docs/ai/working_state.md`
- `docs/ai/project_context.md`
- `docs/ai/godot_rules.md`

Review for:
1. Did the diff solve the requested task?
2. Did it touch more files than necessary?
3. Did it change `project.godot`, input map, autoloads, build targets, scene routing, physics layers, or resource paths?
4. Did it manually edit `.tscn`, `.tres`, or `.res` safely?
5. Did it expand `GameManager`, level unlock save data, level scenes, weapon architecture, or rename legacy `player.cs` without explicit reason?
6. Did it add architecture that was not requested?
7. Was verification honest and sufficient for this environment?

Do not:
- suggest new features
- propose broad refactors without concrete risk
- redesign systems that were not part of the task

Return:
- blocking issues
- non-blocking issues
- verification gaps
- accidental architecture creep
- Godot-specific risk notes
