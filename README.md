# ClassicFrogGame

A ClassicFrogGame written in **C# / Windows Forms** for the course **Object Oriented Programming KMUTNB-MC**.

The player jumps a frog across a road (dodging enemies) and a river (riding friendly fish) to reach the target lotus.

## Features

- 3 main levels (easy → hard), each with different road/river lane counts, monster speeds, and a time limit
- 5 hearts at start; a hit (enemy collision, drowning, or timeout) costs 1 heart; 0 hearts = Game Over
- HUD shows the current level, a counting-down time limit, and the current score
- **Time-based scoring**: reaching a lotus awards `LotusScore × (time remaining / time limit)` — less time used earns the full base score, and using more time scales the points down proportionally
- Random items spawn on the road and last 10 s: (1) extra heart, (2) +time
- Pause overlay (Continue / Menu / Restart)
- Game Over and Congratulations screens, both allowing a restart
- Player name set `FormSetName`
- High score: top-5 names + scores persisted to `Highscore.txt`, shown in a sub-form
- Sound effects (jump, damage, lotus score, item pickup, winner, game-over) and background music
- Debug helpers: F1 location overlay, F2 God Mode

## Game Play
![Gameplay](https://res.cloudinary.com/dpij1hdso/image/upload/v1791997532/classicfroggame_l8ho6p.png)

## Build & Run

- **Target:** .NET Framework 4.7.2, `WinExe`, namespace `ClassicFrogJump`
- **Build on Windows only** (audio uses the **WMPLib** COM reference — Windows Media Player):

```
msbuild ClassicFrogJump.sln /p:Configuration=Debug
```

- **Output EXE:** `bin\Debug\ClassicFrogJump.exe`
- Audio files (`.mp3`) live in `assets\sfx\` and are copied to output via `<Content CopyToOutputDirectory>` in the `.csproj`
- No test suite or linter is configured

## How to Play

| Key | Action |
|---|---|
| ↑ ↓ ← → | Jump the frog one step |
| Esc / pause button | Pause (Continue / Menu / Restart) |
| F1 | Toggle debug location overlay (shows `X,Y` of the frog and lotuses) |
| F2 | Toggle God Mode (no heart loss, no drowning) |

## Project Structure

Two layers:

- **UI (Forms)** — `FormStartGame` (menu) → `FormSetName` (name dialog, modal) → `FormGamePlay` (gameplay). End states: `FormGameOver`, `FormGameWinner`, `FormNextLevel` (level transition), and `FormHighScore` (top-5 list). `Program.cs` launches `FormStartGame`.
- **Game logic** — framework-agnostic model classes driven by `GameController`.

```
class/            # model: GameObject, PlayerFrog, Monster (Enemy/Friend),
                  #        TargetLotus, Item, River, Road, Zone, JumpingFiled,
                  #        GameTimer, HighScoreManager
Config/           # GameController (orchestrator), LevelBuilder,
                  #        LevelConfig (3 levels), SpriteConfig
Forms/            # FormStartGame, FormSetName, FormGamePlay,
                  #        FormGameOver, FormGameWinner, FormNextLevel, FormHighScore
assets/           # sprites (gameplay/), menu art, sound (sfx/), icons
```

### Class Hierarchy

- `GameObject` (abstract) → `PlayerFrog`, `Monster` (abstract), `TargetLotus`, `Item`
- `Monster` → `EnemyMonster` (road) and `FriendMonster` (river). Speed is set per-lane at construction.
- `Zone` (abstract) → `River` and `Road`. `ContainsFrog` tests whether the frog's center is inside the zone.

### Game Loop

`FormGamePlay` runs a `System.Windows.Forms.Timer` at **30 ms** and, in `MoveTimer_Tick`, calls **`controller.Update(deltaTime)`** — a single, self-contained simulation step (timer, monster movement, item spawn/collect, road collision, river drowning, riding friends, lotus/goal checks). The form only reads `GameTimer.TimeRemaining` / `Score` / `Level` back into the HUD labels, calls `UpdateHeartsDisplay()`, and `Invalidate()` to repaint.

- **Sprites are drawn via GDI+ in `OnPaint`** (not `PictureBox`es), in z-order with `Graphics.DrawImage` — lotuses (only uncollected), friends, enemies, item chest, then the frog on top — so PNG alpha composites correctly over overlaps.
- **Walk animation (turtle / crocodile):** road enemies play a 4-frame walk cycle; `Monster.CurrentFrame` returns the frame for the current facing (fish have no frames and fall back to `Sprite`).
- **Item pickup popup:** collecting an item shows a floating pill above the frog with what was gained (+heart / +time), rising with ease-out and fading over 1.1 s.
- **Pause:** `TogglePause()` (Esc / button) flips `GameState.Paused` and shows a 3-button overlay; while paused, `Update` returns early and arrow keys are ignored.
- **Score persistence:** `SaveCurrentScoreIfAny()` writes `HighScoreManager.SaveScore(playerName, score)` once per run (guarded by `scoreSaved`) when the form closes or the player returns to the menu.

### Levels

`GameController.InitializeLevel(level)` reads a `LevelConfig` from the static `Levels[]` array via `LevelConfigManager.GetLevelConfig`, which clamps out-of-range levels to `[1, MaxLevel]` (3). `LevelConfig` holds a `Time` limit, `LotusScore` (base score per lotus, scaled by time), `LevelClearBonus`, per-creature speeds (`TurtleSpeed`, `CrocodileSpeed`, `FishBlueSpeed`, `FishRedSpeed`), item tuning, and the layout arrays `LotusSettings` / `RoadLaneSettings` / `RiverLaneSettings`. Each lane entry is a tuple `(dir, type, count, y0, minX, maxX)` — direction, monster type, count per lane, start Y, and the left/right wrap bounds. `LevelBuilder.Build` constructs the lotuses, road enemies, and river fish from the config (random per-lane start X, spaced by `SpriteSet.Spacing`). When all 5 lotuses are collected, the level-clear bonus is awarded and the state becomes `Victory` on the last level or `LevelComplete` otherwise.

### SpriteConfig

`SpriteConfig` (static) is the single source of truth for sprite images and per-creature dimensions. Each creature is a `SpriteSet` carrying its own `Left`/`Right` images, `Width`, `Height`, and `Spacing`, with `Get(bool movingRight)` picking the facing image. The sets are `Frog`, `Turtle`, `Crocodile`, `FishBlue`, `FishRed`; the frog's directional/jump frames are exposed as `FrogUp`/`FrogDown`/`FrogJump`/`FrogJumpDown`/`FrogJumpLeft`/`FrogJumpRight`. `Lotus` is `SpriteConfig.Lotus` (`lilypad_flower`, `LotusWidth = 85, LotusHeight = 45`), and `Item` dimensions are `const`s with `ItemChest` (`treasure_chest`). The walk-cycle frames are held here as `Image[]`s: `TurtleLeftFrames`/`TurtleRightFrames` (4 frames each) and `CrocodileRightFrames`/`CrocodileLeftFrames` (4 frames; left-facing is synthesized by mirroring).

### High Score

`HighScoreManager` (static) persists the top-5 names + scores to `Highscore.txt` next to the EXE (`Application.StartupPath`). Entries are stored one per line as `name|score`, loaded with `Load()` (tolerant of malformed lines), sorted descending by score (then name), and capped at 5. `SaveScore` no-ops on a non-positive score and cleans the name (falls back to `ClassicFrog001`). `FormHighScore` renders the five rows; it is opened from the start menu.

## UML Class Diagram

<!-- Replace the placeholder below with the rendered UML Class Diagram (e.g. a PNG/SVG from PlantUML or Visio). -->

![UML Class Diagram](diagrams/class-diagram.png)

## Notes

- **Field size matches the form.** `GameController` defaults to `fieldWidth = 1008, fieldHeight = 661` (the `FormGamePlay` `ClientSize`). The `River` spans `y = 45, height = 300` and the `Road` spans `y = 315, height = 295` (4 lanes), so the road/river collision zones cover the whole play area. The frog spawns centered in the safe zone below the road: `startX = (fieldWidth - Frog.Width)/2`, `startY = fieldHeight - Frog.Height`. The frog hitbox is **60×50** (`SpriteConfig.Frog`) with `stepSize = 55`.
- **Still inconsistent magic numbers (known, not a system).** `PlayerFrog` move clamps default to `MoveUp(minY=40)` / `MoveDown(maxY=660)` / `MoveLeft(minX=-10)` / `MoveRight(maxX=1000)`, and the monster wrap bounds in `LevelConfig` use `-84..1000` / `-84..1200`. These roughly agree with the 1008-wide field but are not derived from it — treat them as ad-hoc tuning values.

## Installation

**Requirements**

- **Windows** (the game uses the **WMPLib** COM reference — Windows Media Player — for audio, so it will not build or run on macOS/Linux)
- **.NET Framework 4.7.2** (bundled with Windows 10/11)
- **Visual Studio 2019/2022** (with the ".NET desktop development" workload) **or** the **.NET Framework 4.7.2 Developer Pack** + **MSBuild** for command-line builds

**Steps**

1. Clone the repository:

   ```
   git clone https://github.com/LyOoz/ClassicFrogGame
   cd ClassicFrogGame
   ```

2. Build the solution:

   - **Visual Studio:** open `ClassicFrogJump.sln`, set the configuration to **Debug**, and press **Build → Build Solution** (Ctrl+Shift+B).
   - **Command line:**

     ```
     msbuild ClassicFrogJump.sln /p:Configuration=Debug
     ```

3. Run the game:

   ```
   bin\Debug\ClassicFrogJump.exe
   ```

   Or press **F5** from Visual Studio.

**Notes**

- Audio files (`.mp3`) are copied to the output folder automatically via `<Content CopyToOutputDirectory>` in the `.csproj` — no manual setup needed.
- High scores are saved to `Highscore.txt` next to the EXE on first run; no configuration is required.
