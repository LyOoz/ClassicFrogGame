# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Happy Jumping Frog — a Frogger-style arcade game built in C# WinForms. This is a group project for the course **040223102 Object Oriented Programming** (AY 1/2569). The game is single-player, multi-form, and written in MS Visual C# only.

## Build & Run

- Target: **.NET Framework 4.7.2**, `WinExe`, namespace `JumfrogbyMark`.
- Build with Visual Studio or MSBuild **on Windows**:
  - `msbuild JumfrogbyMark.sln /p:Configuration=Debug`
  - Output EXE: `bin\Debug\JumfrogbyMark.exe`
- Audio uses the **WMPLib** COM reference (Windows Media Player) — build and run on Windows only.
- Audio files (`.mp3`) live in `assets\sound\` and are copied to the output directory.

## Architecture

Two layers:

- **UI (Forms)** — `FormStartGame` (menu), `FormSetName` (name dialog), `FormGamePlay` (gameplay). `Program.cs` launches `FormStartGame`.
- **Game logic** — framework-agnostic model classes driven by `GameController`.

`GameController` is the orchestrator: it owns `PlayerFrog`, the `EnemyMonster` / `FriendMonster` / `TargetLotus` lists, `JumpingFiled`, `River`, `Road`, `Item`, and `GameTimer`. Its `Update(deltaTime)` runs the whole game loop — timer, monster movement, item spawn/collect, road collision, river drowning, and lotus/goal checks.

Class hierarchy:

- `Monster` (abstract) → `EnemyMonster` (road) and `FriendMonster` (river). `ApplyLevelSpeed(level)` scales speed per level.
- `Zone` (abstract) → `River` and `Road`. `ContainsFrog` tests whether the frog is inside the zone.

### Game loop & rendering

`FormGamePlay` runs a `System.Windows.Forms.Timer` at 30 ms. Arrow keys are captured in `ProcessCmdKey` → `Jump()`, which moves the frog one step and swaps the sprite. Monster `PictureBox` positions are synced from the model in `MoveTimer_Tick`.

## Key Conventions

- Source comments are in **Thai**; keep new comments consistent with the surrounding style.
- All game classes live in namespace `JumfrogbyMark`.
- Sprites come from `Properties.Resources` (e.g. `frog_up`, `frog_jump`, `sound_on`).
- Audio is the static `Soundplayer` class (WMPLib / Windows Media Player).

## Spec Requirements (from the course brief)

- **3 main levels**, easy → hard, each with different road/river lane counts, monster speeds, and a time limit:
  - Level 1: road 3 lanes, river 3 lanes, speed tier 1, 1500 s
  - Level 2: road 4 lanes, river 3 lanes, speed tier 2, 1200 s
  - Level 3: road 4 lanes, river 4 lanes, speed tier 3, 900 s
- Each main level has **5 sub-levels** that reduce the target lotus count from 5 → 4 → 3 → 2 → 1.
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

- **Level progression**: `InitializeLevel` currently builds one fixed layout (4 road lanes, 5 river rows) regardless of level. The 3-level / 5-sub-level structure and per-level lane counts are not wired up.
- **Item types** are `BonusScore` / `ExtraHeart`; the spec's "+30 s time" item is not implemented.
- No **Game Over** / **Congratulations** screens yet.
- No **`Highscore.txt`** persistence or high-score form.
- Only the jump sound is wired; the other SFX (pickup, water, roar, hit) are not.
- `FormGamePlay.MoveTimer_Tick` syncs only 3 monster sprites (`Enemies[0..2]`) and does **not** call `controller.Update(deltaTime)` — the model update loop (timer, collision, items, lotus) is not yet integrated into the form.
