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

## 2026-04-23 — Minimal persistent level unlock save
Decision:
Use `user://save.json` for persistent level unlock progress.

Reason:
The project needs Continue and replayable completed levels now, but does not need full game-state persistence.

Impact:
The save currently stores level unlock progress only. Do not add player position, enemy state, inventory, stats, or broader save data unless explicitly requested.

Revisit only if:
We explicitly decide to implement full save/load or a broader progression model.

## 2026-04-23 -- Minimal weapon layer
Decision:
Use a minimal weapon layer with `WeaponKind`, `WeaponPickup`, bat melee hitbox scaling, and pistol projectiles.

Reason:
The game needs pickup/throw and first weapon behavior now, but does not need inventory, ammo economy, weapon taxonomy, or combat framework yet.

Impact:
Agents may extend Bat/Pistol behavior when explicitly tasked, but must not introduce generalized weapon architecture, ammo systems, or combat state machines by default.

Revisit only if:
We explicitly decide to design the final combat/weapon model.

## Template
### YYYY-MM-DD — Decision title
Decision:
Reason:
Impact:
Revisit only if:
