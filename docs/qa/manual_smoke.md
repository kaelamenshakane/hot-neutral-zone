# Manual Smoke

Run these checks in the Godot .NET editor/runtime after gameplay-facing changes.

## Main menu
1. Start project and confirm the main menu opens.
2. Re-enter the main menu several times and confirm title/background palettes change.
3. Press New Game and confirm `level_1` loads.
4. Press Continue and confirm the level list appears.
5. Confirm locked levels cannot be started.
6. Press Debug Reset Save and confirm only level 1 remains unlocked.

## Level flow
1. Kill the enemy in level 1 and confirm level 2 loads.
2. Kill the enemy in level 2 and confirm level 3 loads.
3. Kill the enemy in level 3 and confirm return to main menu.
4. Return to Continue and confirm completed/unlocked levels are selectable.

## Defeat and pause
1. Let the enemy kill the player and confirm the death panel appears.
2. Press R on the death panel and confirm the current level restarts.
3. Press Esc during live gameplay and confirm the pause menu opens.
4. Press Resume and confirm gameplay resumes.
5. Open pause again, press Main Menu, and confirm the main menu loads.

## Weapons
1. Pick up the bat with right mouse and confirm HUD says `In hand: Bat`.
2. Attack an enemy with the bat and confirm the enemy dies.
3. Pick up the pistol with right mouse and confirm HUD says `In hand: Pistol`.
4. Shoot an enemy with the pistol and confirm the enemy dies.
5. Drop a held weapon with right mouse and confirm HUD says `In hand: Hands`.

## Logs
1. Inspect Godot debugger for new errors or warnings.
2. If using CLI logs, run `tools\godot_last_errors.ps1` against the latest log.
