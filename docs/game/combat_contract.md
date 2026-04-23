# Combat Contract

This file records current gameplay truth, not final design.

## Player capabilities
- Move with WASD.
- Aim toward the mouse.
- Attack with left mouse.
- Pickup or throw current weapon with right mouse.
- Restart with R.
- Pause with Esc.

## Current weapons
- Hands: default short melee hit.
- Bat: melee hit with a larger active hitbox.
- Pistol: fires a projectile.

## Current pickup rules
- A player can hold one weapon at a time.
- Right mouse picks up a nearby `WeaponPickup` if hands are empty.
- Right mouse drops the held weapon if a weapon is held.
- HUD shows `In hand: Hands`, `Bat`, or `Pistol`.

## Hit and death rules
- `Hitbox` and `Projectile` apply hits to `Hurtbox`.
- Enemy death emits `Killed` and queues the enemy for deletion.
- Player death emits `Killed`; `LevelHud` shows the death panel and pauses the level.
- R restarts the current level from the death panel.

## Victory and defeat
- Current level victory means killing the single enemy in the level.
- `LevelController` listens for enemy death and asks `GameManager` to complete the level.
- Current defeat means player death and manual R restart.

## Registered but not active
- Finish enemy on Space has no gameplay behavior yet.
- Look ahead on Shift has no gameplay behavior yet.
- Lock-on target on middle mouse has no gameplay behavior yet.

## Do not infer yet
- No downed enemy state.
- No ammo economy.
- No inventory.
- No weapon durability.
- No enemy taxonomy.
- No final combat model.
