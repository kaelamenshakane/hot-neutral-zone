# Decisions

Only record decisions that should not be re-litigated every session.

## 2026-04-23 — Pre-architecture mode
Decision:
The project stays intentionally pre-architecture.

Reason:
We want to grow the game incrementally from concrete tasks instead of front-loading systems.

Impact:
Agents must not invent combat frameworks, content registries, save systems, progression systems, or folder architecture unless explicitly requested.

Revisit only if:
We explicitly decide to lock project architecture.

## 2026-04-23 — Verification baseline
Decision:
Use `dotnet build` and `tools\smoke_test.ps1` as default verification.
Treat `godot --headless --path . --import` exit 0 as insufficient unless logs are clean.

Reason:
The current environment likely uses a Godot binary that is not the .NET editor/runtime.

Impact:
Agents must parse logs and report exact verification limits instead of overclaiming runtime success.

Revisit only if:
We confirm a working Godot .NET editor/runtime CLI in the environment.

## Template
### YYYY-MM-DD — Decision title
Decision:
Reason:
Impact:
Revisit only if:
