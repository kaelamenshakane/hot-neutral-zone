# Project Context

## Stable facts
- Engine: Godot.
- Team size: 2 people.
- Version control: Git.
- Art direction: pixel art.
- Broad reference: Hotline Miami-like top-down violent action.
- Development stage: early; internal architecture is not fixed.

## Current intent
Build the project incrementally.
Use Codex mostly for programming help, debugging, Godot glue, refactors, and review.

## Constraints
- Keep scope small.
- Keep code readable.
- Keep iteration fast.
- Avoid premature systems.
- Avoid architecture invented by Codex without explicit request.

## Non-decisions
These are not decided yet:
- exact combat model
- exact level structure
- final folder structure
- enemy taxonomy
- weapon taxonomy
- progression
- save/load
- meta-game
- content pipeline
- automated test framework

## Codex rule
If a task touches an undecided area, Codex must not fill the gap with a large design assumption.
It should implement only the requested slice or flag the missing decision.
