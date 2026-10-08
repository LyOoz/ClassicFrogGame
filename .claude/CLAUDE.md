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

### Game loop

`FormGamePlay` runs a `System.Windows.Forms.Timer` at **30 ms** and, in `MoveTimer_Tick`, calls **`controller.Update(deltaTime)`** — the single self-contained simulation step (timer, monster movement, item spawn/collect, road collision, river drowning, riding friends, lotus/goal checks). The form no longer drives the timer or monster X-positions itself; it only reads `GameTimer.TimeRemaining` / `Score` / `Level` back into the HUD labels, calls `UpdateHeartsDisplay()`, and `Invalidate()` to repaint.

- **Hearts HUD:** the 5 heart `PictureBox`es (`pigHeart1` … `picHeart5`) are refreshed every tick by `UpdateHeartsDisplay()`, swapping `BackgroundImage` between `Properties.Resources.heart_full` and `heart_empty` based on `PlayerFrog.Hearts`.
- **Game Over:** when `controller.IsGameOver` becomes true the tick calls `moveTimer.Stop()`. There is still no dedicated Game Over / Congratulations screen (see Gaps).

Other loop details:

- **Sprites are drawn via GDI+ in `OnPaint`, not `PictureBox`es.** The per-monster `PictureBox`es were removed (they caused a "ghosting" bug: a `PictureBox` with `BackColor = Color.Transparent` fills transparent regions with the *form's* background color, not true alpha, so the frog visually bled over the monster underneath). `FormGamePlay.OnPaint` now draws every sprite in z-order with `Graphics.DrawImage` — friends first, then enemies, then the frog on top — so PNG alpha composites correctly over overlaps. Each `Monster` carries its own `Sprite` (an `Image`), set in `InitializeLevel`; the frog uses the form's `frogSprite` field. `MoveTimer_Tick` calls `Invalidate()` each frame to trigger the repaint.
- **Debug Location overlay.** Press **F1** in `FormGamePlay` to toggle `showDebug`. When on, `OnPaint` calls `DrawDebugLocation` to draw a black-backed lime label above each creature — `Frog X,Y` for the frog, `E X,Y` for enemies, `F X,Y` for fish — showing its on-screen `(X, Y)`, updating every frame as they move. Labels near the top edge flip below the creature.
- `EnemyAt(lane, pos)` / `FriendAt(lane, pos)` still translate a (lane, position-in-lane) pair into a flat-list index via the `roadLaneStarts` / `riverLaneStarts` index maps built in `InitializeLevel`.
- Arrow keys are captured in `ProcessCmdKey` → `Jump()`, which moves the frog one step, swaps `frogSprite` to `frog_jump`, and reverts to a direction sprite after a 100 ms delay.

### Levels

`GameController.InitializeLevel(level)` reads a `LevelConfig` from the static `Levels[]` array via `GetLevelConfig`, which clamps out-of-range levels. `LevelConfig` holds a `Time` limit plus **per-creature** speeds — `TurtleSpeed`, `CrocodileSpeed`, `FishBlueSpeed`, `FishRedSpeed` — so each species can be tuned independently per level. Lane layout is defined by the `roadLaneSettings` / `riverLaneSettings` arrays inside `InitializeLevel`; each entry is a tuple `(dir, type, count, x0, y0, minX, maxX)` — direction, monster type, count per lane, start X, start Y, and the left/right **wrap bounds** the monster bounces between. Each monster is constructed at `x0 + i * <creature>.Spacing` (spread across the lane using that creature's spacing from `SpriteConfig`) and stores its own `minX`/`maxX`, so `Monster.Update()` takes no arguments and wraps using the bounds it was given. `FormGamePlay` hardcodes the level display as `controller.Level + "/3"`.

### SpriteConfig

`SpriteConfig` (static class) is the single source of truth for sprite images and per-creature dimensions. Each creature is a `SpriteSet` carrying its own `Left`/`Right` images, `Width`, `Height`, and `Spacing`, with `Get(bool movingRight)` picking the facing image. The sets are `Frog`, `Turtle`, `Crocodile`, `FishBlue`, `FishRed`; the frog's directional/jump frames are exposed as `FrogUp`/`FrogDown`/`FrogJump`. `Lotus` and `Item` dimensions are `const`s. `GameController` builds each monster from the matching `SpriteSet` (width/height/spacing) and the matching per-creature speed; `FormGamePlay` uses `SpriteConfig.Frog*` for the frog sprite.

## Key Conventions

- Source comments are in **Thai**; keep new comments consistent with the surrounding style.
- All game classes live in namespace `JumfrogbyMark`.
- Game sprites and per-creature dimensions come from `SpriteConfig` (which itself wraps `Properties.Resources`, e.g. `frog_up`, `frog_jump`). UI-only icons like `sound_on`/`sound_off` are still referenced from `Properties.Resources` directly.
- Audio is the static `Soundplayer` class (WMPLib / Windows Media Player); `ToggleMute` mutes both the music and SFX players.
- **Field size now matches the form.** `GameController` defaults to `fieldWidth = 1008, fieldHeight = 561` (the `FormGamePlay` `ClientSize`), so the road/river collision zones span the whole play area. The frog spawns centered in the safe zone below the road: `startX = (fieldWidth - Frog.Width)/2`, `startY = fieldHeight - Frog.Height` (its center sits below the road's bottom edge, so it is not immediately in a collision lane). The frog hitbox is **50×50** (`SpriteConfig.Frog`) with `stepSize = 55`, matching the road lane pitch.
- **Still inconsistent magic numbers (known confusion, not a system).** `PlayerFrog` move clamps still default to `MoveUp(minY=40)` / `MoveDown(maxY=560)` / `MoveLeft(minX=0)` / `MoveRight(maxX=1000)`, and the monster wrap bounds in `InitializeLevel` use `-84..1000` / `-84..1200`. These roughly agree with the 1008-wide field but are not derived from it — treat them as ad-hoc tuning values.

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

- **Sub-levels**: `InitializeLevel` builds a single fixed layout (3 road lanes, 3 river lanes, 5 lotuses) for every level — only the time limit and per-lane speeds vary. The 5-sub-level lotus-count reduction is not wired up.
- **Item types** are `BonusScore` / `ExtraHeart`; the spec's "+30 s time" item is not implemented.
- No **Game Over** / **Congratulations** screens yet.
- No **`Highscore.txt`** persistence or high-score form.
- Only the jump SFX is wired; the other SFX (pickup, water, roar, hit) are not.
