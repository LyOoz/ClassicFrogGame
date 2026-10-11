using System;
using System.Collections.Generic;

namespace JumfrogbyMark
{
    public enum GameState
    {
        Playing,
        Paused,
        GameOver,
        LevelComplete,
        Victory
    }
    public class GameController
    {
        private PlayerFrog playerFrog;
        private List<EnemyMonster> enemies;
        private List<FriendMonster> friends;
        private List<TargetLotus> targetLotuses;
        private JumpingFiled jumpingFiled;
        private River river;
        private Road road;
        private Item currentItem;
        private GameTimer gameTimer;

        // สถานะของเกม
        private int score;
        private int level;
        private GameState gameState;
        private Random rng;

        // ตัวจับเวลาสุ่มเกิดไอเทม
        private float itemSpawnTimer;

        // Properties
        public PlayerFrog PlayerFrog => playerFrog;
        public List<EnemyMonster> Enemies => enemies;
        public List<FriendMonster> Friends => friends;
        public List<TargetLotus> TargetLotuses => targetLotuses;
        public Item CurrentItem => currentItem;
        public GameTimer GameTimer => gameTimer;
        public int Score => score;
        public int Level => level;
        public int NextLevel => level + 1;
        public bool IsGameOver => gameState == GameState.GameOver;
        public bool IsPaused => gameState == GameState.Paused;
        public bool IsLevelComplete => gameState == GameState.LevelComplete;
        public bool IsVictory => gameState == GameState.Victory;
        public bool IsGodMode { get; set; }

        public GameController(int fieldWidth = 1008, int fieldHeight = 661, int startLevel = 1, int startScore = 0)
        {
            this.rng = new Random();
            this.score = startScore;
            this.level = startLevel;
            this.gameState = GameState.Playing;
            this.jumpingFiled = new JumpingFiled(fieldWidth, fieldHeight, startY: 515, medianY: 270, goalY: 1);
            this.river = new River(x: 0, y: 45, width: fieldWidth, height: 300);
            this.road = new Road(x: 0, y: 315, width: fieldWidth, height: 295, lanesCount: 4);
            this.playerFrog = new PlayerFrog(
                startX: (fieldWidth - SpriteConfig.Frog.Width) / 2,
                startY: fieldHeight - SpriteConfig.Frog.Height,
                width: SpriteConfig.Frog.Width,
                height: SpriteConfig.Frog.Height,
                initialHearts: 5,
                stepSize: 55);
            this.gameTimer = new GameTimer();
            this.currentItem = new Item(width: SpriteConfig.ItemWidth, height: SpriteConfig.ItemHeight, durationSeconds: 10.0f);
            this.enemies = new List<EnemyMonster>();
            this.friends = new List<FriendMonster>();
            this.targetLotuses = new List<TargetLotus>();
            // InitializeLevel(3);
            InitializeLevel(startLevel);
        }

        // method create object แต่ละอัน
        public void InitializeLevel(int newLevel)
        {
            this.level = Clamp(newLevel, 1, LevelConfigManager.MaxLevel);
            LevelConfig cfg = LevelConfigManager.GetLevelConfig(newLevel);
            this.enemies.Clear();
            this.friends.Clear();
            this.targetLotuses.Clear();

            LevelBuilder.Build(cfg, rng, enemies, friends, targetLotuses);
            playerFrog.ResetToStart();
            currentItem.Reset();
            itemSpawnTimer = cfg.ItemFirstSpawnDelay;
            gameTimer.Start(cfg.Time);
            gameState = GameState.Playing;
        }

        private static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
        // logic update game state
        public void Update(float deltaTime)
        {
            if (gameState != GameState.Playing) return;

            // Gametimer
            bool timeRanOut = gameTimer.Update(deltaTime);
            if (timeRanOut)
            {
                gameState = GameState.GameOver;
                return;
            }

            // Update EnemyMonster , FriendMonster
            foreach (var enemy in enemies)
                enemy.Update(deltaTime);

            foreach (var friend in friends)
                friend.Update(deltaTime);

            // จัดการการเกิดและหมดอายุของไอเทมกลางถนน
            LevelConfig cfg = LevelConfigManager.GetLevelConfig(level);
            if (!currentItem.IsActive)
            {
                itemSpawnTimer -= deltaTime;
                if (itemSpawnTimer <= 0)
                {
                    currentItem.SpawnRandom(road.RoadBounds, rng, cfg.ItemExtraHeartChance, cfg.ItemHeartBonus, cfg.ItemTimeBonusSeconds);
                    itemSpawnTimer = cfg.ItemSpawnInterval;
                }
            }
            else
            {
                currentItem.Update(deltaTime);
                // กบเก็บไอเทม
                if (currentItem.CheckCollect(playerFrog))
                {
                    ApplyItemEffect(currentItem);
                    Soundplayer.PlayTakeItem();
                }
            }

            // Frog Collision Detection
            if (road.IsFrogOnRoad(playerFrog))
            {
                foreach (var enemy in enemies)
                {
                    if (enemy.CheckCollision(playerFrog))
                    {
                        if (IsGodMode)
                        {
                            return;
                        }

                        HandleFrogDamage();
                        return;
                    }
                }
            }
            // TargetLotus
            foreach (var lotus in targetLotuses)
            {
                // กบกระโดดมาถึง
                if (lotus.CheckReached(playerFrog))
                {
                    if (!lotus.IsOccupied)
                    {
                        lotus.Occupy();
                        score += lotus.ScoreValue;
                        Soundplayer.PlayTakeLotus();
                        playerFrog.ResetToStart();

                        if (CheckAllLotusesOccupied())
                        {
                            score += LevelConfigManager.GetLevelConfig(level).LevelClearBonus;
                            gameTimer.Reset();
                            gameState = level >= LevelConfigManager.MaxLevel
                                ? GameState.Victory
                                : GameState.LevelComplete;
                        }
                    }
                    else
                    {
                        playerFrog.ResetToStart();
                    }
                    return;
                }
            }

            // Frog Collision Detection กับ River และ FriendMonster
            if (river.IsFrogInRiver(playerFrog))
            {
                FriendMonster carryingFriend = null;
                foreach (var friend in friends)
                {
                    if (friend.IsFrogOnTop(playerFrog))
                    {
                        carryingFriend = friend;
                        break;
                    }
                }

                if (carryingFriend != null)
                {
                    // กบขี่คอ
                    carryingFriend.Carry(playerFrog);

                    // ขี่คอหลุดออกนอกจอ
                    if (playerFrog.X < -playerFrog.Width || playerFrog.X > jumpingFiled.Width)
                    {
                        if (IsGodMode)
                        {
                            jumpingFiled.ClampFrogPosition(playerFrog);
                            return;
                        }

                        HandleFrogDamage();
                        return;
                    }
                }
                else
                {
                    if (IsGodMode)
                    {
                        jumpingFiled.ClampFrogPosition(playerFrog);
                        return;
                    }

                    HandleFrogDamage();
                    return;
                }
            }

            // ล็อคตำแหน่งกบไม่ให้ออกนอกขอบเขตสนาม
            jumpingFiled.ClampFrogPosition(playerFrog);
        }

        private void ApplyItemEffect(Item item)
        {
            if (item.Type == ItemType.ExtraHeart)
            {
                if (playerFrog.Hearts < playerFrog.InitialHearts)
                {
                    playerFrog.Hearts = Math.Min(playerFrog.InitialHearts, playerFrog.Hearts + item.HeartBonus);
                }
                else
                {
                    gameTimer.AddTime(item.TimeBonusSeconds * 0.5f);
                }

                return;
            }

            gameTimer.AddTime(item.TimeBonusSeconds);
        }

        private void HandleFrogDamage()
        {
            Soundplayer.PlayDmgSound();
            bool isGameOver = playerFrog.TakeDamage();
            if (isGameOver)
            {
                gameState = GameState.GameOver;
            }
        }

        public void MovePlayer(FrogDirection direction)
        {
            if (gameState != GameState.Playing) return;

            jumpingFiled.MoveFrog(playerFrog, direction);
        }

        private bool CheckAllLotusesOccupied()
        {
            foreach (var lotus in targetLotuses)
            {
                if (!lotus.IsOccupied) return false;
            }
            return true;
        }

        public void TogglePause()
        {
            if (gameState == GameState.Playing)
            {
                gameState = GameState.Paused;
                gameTimer.Pause();
            }
            else if (gameState == GameState.Paused)
            {
                gameState = GameState.Playing;
                gameTimer.Resume();
            }
        }
        public void RestartGame()
        {
            score = 0;
            level = 1;
            playerFrog.ResetAll();
            InitializeLevel(1);
        }
    }
}
