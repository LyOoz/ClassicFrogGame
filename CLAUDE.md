# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

JumfrogbyMark — a Frogger-style arcade game in C# WinForms. A single-player, multi-form game for the course **040223102 Object Oriented Programming**. The player jumps a frog across a road (avoiding enemies) and a river (riding friendly fish) to reach target lotus pads.

## Build & Run

- Target: **.NET Framework 4.7.2**, `WinExe`, namespace `JumfrogbyMark`.
- Build **on Windows only** (audio uses the **WMPLib** COM reference — Windows Media Player):
  - `msbuild JumfrogbyMark.sln /p:Configuration=Debug`
  - Output EXE: `bin\Debug\JumfrogbyMark.exe`
- Audio files (`.mp3`) live in `assets\sound\` and are copied to output via `<Content CopyToOutputDirectory>` in the `.csproj`.
- There is **no test suite or linter** configured.

## Architecture

Two layers:

- **UI (Forms)** — `FormStartGame` (menu) → `FormSetName` (name dialog, modal) → `FormGamePlay` (gameplay). `Program.cs` launches `FormStartGame`.
- **Game logic** — framework-agnostic model classes driven by `GameController`.

`GameController` is the orchestrator: it owns `PlayerFrog`, the `EnemyMonster` / `FriendMonster` / `TargetLotus` lists, `JumpingFiled`, `River`, `Road`, `Item`, and `GameTimer`. Its `Update(deltaTime)` is a **complete, self-contained simulation step** — timer, monster movement, item spawn/collect, road collision, river drowning, and lotus/goal checks all live there.

Class hierarchy:

- `Monster` (abstract) → `EnemyMonster` (road) and `FriendMonster` (river). Speed is set per-lane at construction (no per-level speed method).
- `Zone` (abstract) → `River` and `Road`. `ContainsFrog` tests whether the frog's center is inside the zone.

### Game loop — the important gotcha

`FormGamePlay` runs a `System.Windows.Forms.Timer` at **30 ms** and, in `MoveTimer_Tick`, drives the model **manually**: it calls `controller.GameTimer.Update(...)` and `enemy.Update(...)` / `friend.Update(...)` directly, then syncs sprite positions. It does **not** call `controller.Update(deltaTime)`.

Consequently, the collision / item / lotus / level-progression logic inside `GameController.Update` is **not currently reachable from the running form** — the form only advances the timer and monster X-positions. Integrating `controller.Update` into the tick (and removing the form's duplicated monster/timer calls) is the key remaining wiring.

Other loop details:

- Monster sprites are **fixed `PictureBox`es** placed in the designer (one per lane). `SyncMonster` copies only the model's `X` (Y is fixed per-lane in the designer). `EnemyAt(lane, pos)` / `FriendAt(lane, pos)` translate a (lane, position-in-lane) pair into a flat-list index via the `roadLaneStarts` / `riverLaneStarts` index maps built in `InitializeLevel`.
- Arrow keys are captured in `ProcessCmdKey` → `Jump()`, which moves the frog one step, swaps the sprite to `frog_jump`, and reverts to a direction sprite after a 100 ms delay.

### Levels

`GameController.InitializeLevel(level)` reads a `LevelConfig` from the static `Levels[]` array (time limit + per-lane `EnemySpeeds` / `FriendSpeeds`) via `GetLevelConfig`, which clamps out-of-range levels. Lane layout (direction, type, count, start X) is defined by the `roadLaneSettings` / `riverLaneSettings` arrays inside `InitializeLevel`. `FormGamePlay` hardcodes the level display as `controller.Level + "/3"`.

## Key Conventions

- Source comments are in **Thai**; keep new comments consistent with the surrounding style.
- All game classes live in namespace `JumfrogbyMark`.
- Sprites come from `Properties.Resources` (e.g. `frog_up`, `frog_jump`, `sound_on`).
- Audio is the static `Soundplayer` class (WMPLib / Windows Media Player); `ToggleMute` mutes both the music and SFX players.
- **Watch for inconsistent magic-number dimensions.** The field is constructed at 396×510, but `PlayerFrog` move clamps default to 830/560 and the form's monster-update bounds use `-84..820` / `-84..500`. These do not all agree — treat them as a known area of confusion, not an intentional system.

## Spec Requirements (from the course brief)

- **3 main levels**, easy → hard, each with different road/river lane counts, monster speeds, and a time limit.
- Each main level has **5 sub-levels** reducing the target lotus count 5 → 4 → 3 → 2 → 1.
- 5 hearts at start; a hit (enemy collision, drowning, or timeout) costs 1 heart; 0 hearts = Game Over.
- Must display: current level, remaining time (counting down), and current score.
- Score is time-based: less time used to reach a lotus = more points.
- **Items** spawn randomly on the road and last 10 s: (1) extra heart, (2) +30 s time.
- A Game Over screen and a Congratulations screen, both allowing a restart.
- Player name is set via `FormSetName`.
- **High score**: top-5 names + scores persisted to an external `Highscore.txt`, shown in a sub-form.
- Sound effects: frog jump, item pickup, water flow, enemy roar, frog hit.
- Deliverables include a UML Class Diagram and 9 UML Activity Diagrams (Visio).

## Current Gaps (not yet implemented)

- **Game loop not integrated**: `FormGamePlay.MoveTimer_Tick` does not call `controller.Update(deltaTime)` (see gotcha above), so collision, item collection, lotus capture, and level progression do not run in the live game.
- **Sub-levels**: `InitializeLevel` builds a single fixed layout (3 road lanes, 3 river lanes, 5 lotuses) for every level — only the time limit and per-lane speeds vary. The 5-sub-level lotus-count reduction is not wired up.
- **Item types** are `BonusScore` / `ExtraHeart`; the spec's "+30 s time" item is not implemented.
- No **Game Over** / **Congratulations** screens yet.
- No **`Highscore.txt`** persistence or high-score form.
- Only the jump SFX is wired; the other SFX (pickup, water, roar, hit) are not.
- `MoveTimer_Tick` maps both `EnemyAt(2,0)` and `EnemyAt(2,1)` to the same `picCrocR` sprite — likely a bug to fix when expanding monster rendering.
