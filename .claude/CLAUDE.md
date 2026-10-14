# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

ClassicFrogGame — a Frogger-style arcade game in C# WinForms. A single-player, multi-form game for the course **040223102 Object Oriented Programming**. The player jumps a frog across a road (avoiding enemies) and a river (riding friendly fish) to reach target lotus pads.

## Build & Run

- Target: **.NET Framework 4.7.2**, `WinExe`, namespace `ClassicFrogGame`.
- Build **on Windows only** (audio uses the **WMPLib** COM reference — Windows Media Player):
  - `msbuild ClassicFrogGame.sln /p:Configuration=Debug`
  - Output EXE: `bin\Debug\ClassicFrogGame.exe`
- Audio files (`.mp3`) live in `assets\sfx\` and are copied to output via `<Content CopyToOutputDirectory>` in the `.csproj`.
- There is **no test suite or linter** configured.

## Architecture

Two layers:

- **UI (Forms)** — `FormStartGame` (menu) → `FormSetName` (name dialog, modal) → `FormGamePlay` (gameplay). End states: `FormGameOver`, `FormGameWinner`, `FormNextLevel` (level transition), and `FormHighScore` (top-5 list). `Program.cs` launches `FormStartGame`.
- **Game logic** — framework-agnostic model classes driven by `GameController`.

`GameController` is the orchestrator: it owns `PlayerFrog`, the `EnemyMonster` / `FriendMonster` / `TargetLotus` lists, `JumpingFiled`, `River`, `Road`, `Item`, and `GameTimer`. Its `Update(deltaTime)` is a **complete, self-contained simulation step** — timer, monster movement, item spawn/collect, road collision, river drowning, riding friends, and lotus/goal checks all live there. It exposes a `GameState` (`Playing`, `Paused`, `GameOver`, `LevelComplete`, `Victory`) plus `IsGameOver` / `IsPaused` / `IsLevelComplete` / `IsVictory` and a debug `IsGodMode` flag.

Class hierarchy:

- `GameObject` (abstract) → `PlayerFrog`, `Monster` (abstract), `TargetLotus`, `Item`. `Monster` → `EnemyMonster` (road) and `FriendMonster` (river). Speed is set per-lane at construction (no per-level speed method).
- `Zone` (abstract) → `River` and `Road`. `ContainsFrog` tests whether the frog's center is inside the zone.

### Game loop

`FormGamePlay` runs a `System.Windows.Forms.Timer` at **30 ms** and, in `MoveTimer_Tick`, calls **`controller.Update(deltaTime)`** — the single self-contained simulation step (timer, monster movement, item spawn/collect, road collision, river drowning, riding friends, lotus/goal checks). The form no longer drives the timer or monster X-positions itself; it only reads `GameTimer.TimeRemaining` / `Score` / `Level` back into the HUD labels, calls `UpdateHeartsDisplay()`, and `Invalidate()` to repaint.

- **Hearts HUD:** the 5 heart `PictureBox`es (`pigHeart1` … `picHeart5`) are refreshed every tick by `UpdateHeartsDisplay()`, swapping `BackgroundImage` between `Properties.Resources.heart_full` and `heart_empty` based on `PlayerFrog.Hearts`.
- **End states:** when `controller.IsGameOver` / `IsLevelComplete` / `IsVictory` becomes true, the tick calls `moveTimer.Stop()` and shows `FormGameOver` / `FormNextLevel` / `FormGameWinner` respectively (each hides the gameplay form and opens the next). `FormNextLevel` carries `playerName`, `score`, and `nextLevel` into a fresh `FormGamePlay`.
- **Pause:** `TogglePause()` (Esc key or the pause button) flips `GameState.Paused` and shows/hides a 3-button overlay (Continue / Menu / Restart) created in `InitializePauseOverlay`. While paused, `Update` returns early and arrow keys are ignored.
- **Score persistence:** `SaveCurrentScoreIfAny()` writes `HighScoreManager.SaveScore(playerName, score)` once per run (guarded by `scoreSaved`) when the form closes or the player returns to the menu.

Other loop details:

- **Sprites are drawn via GDI+ in `OnPaint`, not `PictureBox`es.** The per-monster `PictureBox`es were removed (they caused a "ghosting" bug: a `PictureBox` with `BackColor = Color.Transparent` fills transparent regions with the *form's* background color, not true alpha, so the frog visually bled over the monster underneath). `FormGamePlay.OnPaint` draws every sprite in z-order with `Graphics.DrawImage` — target lotuses (only uncollected `!lotus.IsOccupied`), friends first, then enemies, then the active item chest, then the frog on top — so PNG alpha composites correctly over overlaps. Each `Monster` and `TargetLotus` carries its own `Sprite` (an `Image`), set in `LevelBuilder`; the frog uses the form's `frogSprite` field. `MoveTimer_Tick` calls `Invalidate()` each frame to trigger the repaint.
- **Debug Location overlay.** Press **F1** in `FormGamePlay` to toggle `showDebug`. When on, `OnPaint` calls `DrawDebugLocation` to draw a black-backed lime label above the frog and each target lotus — `Frog X,Y` / `L X,Y` — showing its on-screen `(X, Y)`, updating every frame. Labels near the top edge flip below the creature.
- **God Mode.** Press **F2** to toggle `controller.IsGodMode`. When on, enemy collisions and drowning/falling off-screen do not cost a heart (the frog is clamped back into the field instead), and a gold "GODMODE" banner is drawn in `OnPaint`.
- **Walk animation (turtle / crocodile).** The road enemies play a 4-frame walk cycle instead of a single still sprite. The walk sheets (`crocodile_right_walk_sheet`, `turtle_left_walk_sheet`, `turtle_right_walk_sheet`) were split into individual frame PNGs (`*_walk_0..3`) in `assets/gameplay/`, exposed as `SpriteConfig.TurtleLeftFrames` / `TurtleRightFrames` / `CrocodileRightFrames` / `CrocodileLeftFrames`. `Monster` carries `leftFrames`/`rightFrames` + a frame index and an `AnimInterval` (0.15 s) timer; `Monster.Update(deltaTime)` advances the frame, and the `CurrentFrame` property returns the frame for the current facing (falling back to `Sprite` when a creature has no frames — so fish are unaffected). `LevelBuilder` calls `enemy.SetWalkFrames(...)` per species. The crocodile only has right-facing frames, so `SpriteConfig.MirrorFrames` flips them horizontally to synthesize the left-facing set. `FormGamePlay.OnPaint` draws `m.CurrentFrame` (not `m.Sprite`) for friends and enemies.
- **Item pickup popup.** When the frog collects a special item, a floating pill pops above the frog showing what it got. `GameController.ApplyItemEffect` records the real outcome into an `ItemPickup` struct (`Text`, `IsHeart`) and bumps a monotonically increasing `pickupId`; the form exposes `LastItemPickup` / `PickupId`. The three outcomes: extra heart (hearts not full) → `"+N ♥"` (pink-red), extra heart while hearts are full → converted to time (`TimeBonusSeconds * 0.5f`) shown as `"+Ns"` (light-blue), and extra time → `"+Ns"` (light-blue). `FormGamePlay.MoveTimer_Tick` detects a new pickup by comparing `controller.PickupId` to its cached `lastPickupId` and captures the frog's `Y - 5` as the pop's start position; `OnPaint` then draws `DrawItemPickup` (a rounded pill via the `RoundedRect` `GraphicsPath` helper, `System.Drawing.Drawing2D`) above the frog — it rises 34 px with an ease-out over the first 0.35 s and fades out over the last quarter of a 0.5 s lifetime (`ItemPopDuration`), with a 1 px black text shadow.
- Arrow keys are captured in `ProcessCmdKey` → `Jump()`, which moves the frog one step, swaps `frogSprite` to a directional jump frame (`FrogJump` / `FrogJumpDown` / `FrogJumpLeft` / `FrogJumpRight`), and reverts to a standing direction sprite after a 100 ms delay.

### Levels

`GameController.InitializeLevel(level)` reads a `LevelConfig` from the static `Levels[]` array via `LevelConfigManager.GetLevelConfig`, which clamps out-of-range levels to `[1, MaxLevel]` (3). `LevelConfig` holds a `Time` limit, `LotusScore` (base score per lotus pad, scaled by time — see below), `LevelClearBonus`, **lane counts** (`RoadLanes`, `RiverLanes`), **per-creature** speeds (`TurtleSpeed`, `CrocodileSpeed`, `FishBlueSpeed`, `FishRedSpeed`), item tuning (`ItemFirstSpawnDelay`, `ItemSpawnInterval`, `ItemExtraHeartChance`, `ItemHeartBonus`, `ItemTimeBonusSeconds`), and the layout arrays `LotusSettings` and `RoadLaneSettings` / `RiverLaneSettings`. Each lane entry is a tuple `(dir, type, count, y0, minX, maxX)` — direction, monster type, count per lane, start Y, and the left/right **wrap bounds** the monster bounces between. `LevelBuilder` builds only the first `RoadLanes` / `RiverLanes` entries from those arrays, so the lane counts differ per level: **level 1 = 3 road / 3 river lanes, level 2 = 4 road / 3 river, level 3 = 4 road / 4 river** (all keep 5 lotuses). The levels differ in lane count, time limit, per-lane speeds, and score. `LevelBuilder.Build` constructs the lotuses, road enemies, and river fish from the config (it picks a random per-lane start X and spaces monsters by `SpriteSet.Spacing`). When a lotus is reached, the frog scores `GameController.CalcLotusScore(lotus.ScoreValue)` = `LotusScore × GameTimer.RemainingFraction` (clamped to `[0, LotusScore]`) — so less time used (more time remaining) earns the full base score and using more time scales the points down proportionally; `GameTimer` exposes `TimeLimit` and `RemainingFraction` (`timeRemaining / timeLimit`) for this. When all 5 lotuses are collected, the level-clear bonus is awarded, and the state becomes `Victory` on the last level or `LevelComplete` otherwise. In the collision pipeline, lotuses are evaluated *before* river drowning so frogs landing on lilypads collect the goal and return safely instead of drowning.

### SpriteConfig

`SpriteConfig` (static class) is the single source of truth for sprite images and per-creature dimensions. Each creature is a `SpriteSet` carrying its own `Left`/`Right` images, `Width`, `Height`, and `Spacing`, with `Get(bool movingRight)` picking the facing image. The sets are `Frog`, `Turtle`, `Crocodile`, `FishBlue`, `FishRed`; the frog's directional/jump frames are exposed as `FrogUp`/`FrogDown`/`FrogJump`/`FrogJumpDown`/`FrogJumpLeft`/`FrogJumpRight`. `Lotus` is exposed as `SpriteConfig.Lotus` (`lilypad_flower`, dimensions `LotusWidth = 85, LotusHeight = 45`), and `Item` dimensions are `const`s with `ItemChest` (`treasure_chest`). `LevelBuilder` builds each monster from the matching `SpriteSet` (width/height/spacing) and the matching per-creature speed; `FormGamePlay` uses `SpriteConfig.Frog*` for the frog sprite. The walk-cycle frames are also held here as `Image[]`s: `TurtleLeftFrames`/`TurtleRightFrames` (4 frames each) and `CrocodileRightFrames`/`CrocodileLeftFrames` (4 frames; left-facing is synthesized by mirroring).

### High score

`HighScoreManager` (static) persists the top-5 names + scores to `Highscore.txt` next to the EXE (`Application.StartupPath`). Entries are stored one per line as `name|score`, loaded with `Load()` (tolerant of malformed lines), sorted descending by score (then name), and capped at 5. `SaveScore` no-ops on a non-positive score and cleans the name (falls back to `ClassicFrog001`). `FormHighScore` renders the five rows; it is opened from the start menu.

## Key Conventions

- Source comments are in **Thai**; keep new comments consistent with the surrounding style.
- All game classes live in namespace `ClassicFrogGame`; the end-state and high-score forms live in `ClassicFrogGame.Forms`.
- Game sprites and per-creature dimensions come from `SpriteConfig` (which itself wraps `Properties.Resources`, e.g. `frog_up`, `frog_jump`). UI-only icons like `sound_on`/`sound_off` are still referenced from `Properties.Resources` directly.
- Audio is the static `Soundplayer` class (WMPLib / Windows Media Player). It holds one looping `sound` player (start/gameplay music) and one `sfxJump` player reused for all SFX (jump, damage, lotus score, item pickup, winner, game-over). `ToggleMute` mutes both players.
- **Field size matches the form.** `GameController` defaults to `fieldWidth = 1008, fieldHeight = 661` (the `FormGamePlay` `ClientSize`). The `River` spans `y = 45, height = 300` and the `Road` spans `y = 315, height = 295` (4 lanes), so the road/river collision zones cover the whole play area. The frog spawns centered in the safe zone below the road: `startX = (fieldWidth - Frog.Width)/2`, `startY = fieldHeight - Frog.Height` (its center sits below the road's bottom edge, so it is not immediately in a collision lane). The frog hitbox is **60×50** (`SpriteConfig.Frog`) with `stepSize = 55`.
- **Still inconsistent magic numbers (known confusion, not a system).** `PlayerFrog` move clamps default to `MoveUp(minY=40)` / `MoveDown(maxY=660)` / `MoveLeft(minX=-10)` / `MoveRight(maxX=1000)`, and the monster wrap bounds in `LevelConfig` use `-84..1000` / `-84..1200`. These roughly agree with the 1008-wide field but are not derived from it — treat them as ad-hoc tuning values.

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
- Deliverables include a UML Class Diagram and 9 UML Activity Diagrams (Visio) — the 9 activity diagrams are present as PlantUML in `diagrams/` (AD1…AD9).

## Current Gaps (not yet implemented)

- **Sub-levels**: each main level still builds a single fixed layout (5 lotuses) — the 5-sub-level lotus-count reduction (5 → 4 → 3 → 2 → 1) is not wired up. (Lane counts *do* already vary per level — see Levels.)
- **Missing SFX**: the spec's "water flow" and "enemy roar" sounds are not wired; `Soundplayer` has jump, damage, lotus score, item pickup, winner, and game-over.
