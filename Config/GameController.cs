using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

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

    /// GameController control PlayerFrog, EnemyMonster, FriendMonster, TargetLotus, JumpingFiled, River, Road, Item , GameTimer
    public class GameController
    {
        // object หลักของเกม
        private PlayerFrog playerFrog;
        private List<EnemyMonster> enemies;
        private List<FriendMonster> friends;
        private readonly List<int> roadLaneStarts = new List<int>();
        private readonly List<int> riverLaneStarts = new List<int>();
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
        private const float ItemSpawnInterval = 15.0f; // เกิดไอเทมทุกๆ 15 วินาที

        // Properties
        public PlayerFrog PlayerFrog => playerFrog;
        public List<EnemyMonster> Enemies => enemies;
        public List<FriendMonster> Friends => friends;
        public List<TargetLotus> TargetLotuses => targetLotuses;
        public JumpingFiled JumpingFiled => jumpingFiled;
        public River River => river;
        public Road Road => road;
        public Item CurrentItem => currentItem;
        public GameTimer GameTimer => gameTimer;
        public int Score => score;
        public int Level => level;
        public int NextLevel => level + 1;
        public GameState State => gameState;
        public bool IsGameOver => gameState == GameState.GameOver;
        public bool IsPaused => gameState == GameState.Paused;
        public bool IsLevelComplete => gameState == GameState.LevelComplete;
        public bool IsVictory => gameState == GameState.Victory;
        public bool IsGodMode { get; set; }

        // constructor
        private const int MaxLevel = 3;

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
            // InitializeLevel(startLevel);
            InitializeLevel(startLevel);
        }

        // เรียง index ตาม (เลน, ตัวที่)
        public EnemyMonster EnemyAt(int lane, int pos)
        {
            if (lane < 0 || lane >= roadLaneStarts.Count) return null;
            int start = roadLaneStarts[lane];
            int next = (lane + 1 < roadLaneStarts.Count) ? roadLaneStarts[lane + 1] : enemies.Count;
            int idx = start + pos;
            return (idx >= start && idx < next) ? enemies[idx] : null;
        }
        public FriendMonster FriendAt(int lane, int pos)
        {
            if (lane < 0 || lane >= riverLaneStarts.Count) return null;
            int start = riverLaneStarts[lane];
            int next = (lane + 1 < riverLaneStarts.Count) ? riverLaneStarts[lane + 1] : friends.Count;
            int idx = start + pos;
            return (idx >= start && idx < next) ? friends[idx] : null;
        }

        // method create object แต่ละอัน
        public void InitializeLevel(int newLevel)
        {
            this.level = Clamp(newLevel, 1, MaxLevel);
            LevelConfig cfg = GetLevelConfig(newLevel);
            this.enemies.Clear();
            this.friends.Clear();
            this.targetLotuses.Clear();
            (int x, int y)[] lotusSettings;
            // config lotus
            if (level == 1)
            {
                lotusSettings = new (int x, int y)[]
                {
                (58,  85),
                (260, 85),
                (461, 85),
                (663, 85),
                (865, 85),
                };
            }
            else if (level == 2)
            {
                lotusSettings = new (int x, int y)[]
                {
                (58,  85),
                (260, 85),
                (461, 85),
                (663, 85),
                (865, 85),
                };
            }
            else // ด่าน 3
            {
                lotusSettings = new (int x, int y)[]
                {
                (75,  70),
                (275, 70),
                (475, 70),
                (675, 70),
                (875, 70),
                };
            }

            // Sprite lotus
            for (int i = 0; i < lotusSettings.Length; i++)
            {
                var (lx, ly) = lotusSettings[i];
                var lotus = new TargetLotus(lx, ly, width: SpriteConfig.LotusWidth, height: SpriteConfig.LotusHeight, scoreValue: cfg.LotusScore);
                lotus.Sprite = SpriteConfig.Lotus;
                targetLotuses.Add(lotus);
            }

            // config เลน EnemyMonster
            var roadLaneSettings = new (int dir, EnemyType type, int count, int y0, int minX, int maxX)[]
            {
                // start = randomx0 , y0 , end = minX maxX
                // -1=ซ้าย 1=ขวา , type, จำนวนตัว,  y0, minX, maxX
                ( 1, EnemyType.turtle,     3, 448, -84, 1000), // เลนแรกด่าน 1,2
                (-1, EnemyType.turtle,     3, 490, -84, 1200),
                ( 1, EnemyType.crocodile,  3, 555, -84, 1000),
                (-1, EnemyType.crocodile,  3, 368, -84, 1200), // เลนแรกด่าน 3
            };
            roadLaneStarts.Clear();

            // Sprite EnemyMonster
            for (int lane = 0; lane < cfg.RoadLanes; lane++)
            {
                roadLaneStarts.Add(enemies.Count);
                var (dir, type, count, y0, minX, maxX) = roadLaneSettings[lane];

                // สุ่มค่า x0 ใหม่ในแต่ละเลนโดยอิงจาก minX และ maxX
                int x0 = rng.Next(minX, maxX - 200);

                for (int i = 0; i < count; i++)
                {
                    var enemySet = type == EnemyType.turtle ? SpriteConfig.Turtle : SpriteConfig.Crocodile;
                    float enemySpeed = type == EnemyType.turtle ? cfg.TurtleSpeed : cfg.CrocodileSpeed;

                    var enemy = new EnemyMonster(x0 + i * enemySet.Spacing, y0, enemySet.Width, enemySet.Height, baseSpeed: enemySpeed, movingRight: dir > 0, type: type, minX: minX, maxX: maxX);
                    enemy.Sprite = enemySet.Get(dir > 0);

                    if (type == EnemyType.turtle)
                        enemy.SetWalkFrames(SpriteConfig.TurtleLeftFrames, SpriteConfig.TurtleRightFrames);
                    else
                    {
                        // จระเข้มีเฟรมแค่ขวา mirror เป็นซ้าย
                        enemy.SetWalkFrames(MirrorFrames(SpriteConfig.CrocodileRightFrames), SpriteConfig.CrocodileRightFrames);
                    }
                    enemies.Add(enemy);
                }
            }

            // config เลน FriendMonster
            var riverLaneSettings = new (int dir, FriendType type, int count, int y0, int minX, int maxX)[]            
            {
                // start = randomx0 , y0 , end = minX maxX
                // -1=ซ้าย 1=ขวา , type, จำนวนตัว, y0, minX, maxX
               (-1, FriendType.FishBlue,  3, 180, -84, 1200), // เลนแรกด่าน 1,2
               ( 1, FriendType.FishRed,   3, 235, -84, 1000),
               (-1, FriendType.FishBlue,  3, 290, -84, 1200),
               (-1, FriendType.FishRed,   3, 125, -84, 1200), // เลนแรกด่าน 3

            };
            riverLaneStarts.Clear();
            // Sprite FriendMonster
            for (int lane = 0; lane < cfg.RiverLanes; lane++)
            {
                riverLaneStarts.Add(friends.Count);
                var (dir, type, count, y0, minX, maxX) = riverLaneSettings[lane];
                int x0 = rng.Next(minX + 50, maxX - 300);
                for (int i = 0; i < count; i++)
                {
                    var friendSet = type == FriendType.FishBlue ? SpriteConfig.FishBlue : SpriteConfig.FishRed;
                    float friendSpeed = type == FriendType.FishBlue ? cfg.FishBlueSpeed : cfg.FishRedSpeed;
                    var friend = new FriendMonster(x0 + i * friendSet.Spacing, y0, friendSet.Width, friendSet.Height, baseSpeed: friendSpeed, movingRight: dir > 0, type: type, minX: minX, maxX: maxX);
                    friend.Sprite = friendSet.Get(dir > 0);
                    friends.Add(friend);
                }
            }

            playerFrog.ResetToStart();
            currentItem.Reset();
            itemSpawnTimer = 3.0f; // รอ 3 วินาทีก่อนสุ่มเกิดไอเทมชิ้นแรก
            gameTimer.Start(cfg.Time);
            gameState = GameState.Playing;
        }

        // Config แต่ละด่าน
        private struct LevelConfig
        {
            public float Time;
            public int RoadLanes;
            public int RiverLanes;
            public int LotusScore;
            public int LevelClearBonus;
            public float TurtleSpeed;
            public float CrocodileSpeed;
            public float FishBlueSpeed;
            public float FishRedSpeed;
        }
        private static readonly LevelConfig[] Levels =
        {
            new LevelConfig { // ด่าน 1
                Time = 1500f,
                RoadLanes = 3,
                RiverLanes = 3,
                LotusScore = 100,
                LevelClearBonus = 1000,
                TurtleSpeed = 1.7f,
                CrocodileSpeed = 2.6f,
                FishBlueSpeed = 2.2f,
                FishRedSpeed = 2.3f
            },
            new LevelConfig { // ด่าน 2
                Time = 1200f,
                RoadLanes = 4,
                RiverLanes = 3,
                LotusScore = 750,
                LevelClearBonus = 2000,
                TurtleSpeed = 2.6f,
                CrocodileSpeed = 3.6f,
                FishBlueSpeed = 3.1f,
                FishRedSpeed = 3.6f
            },
            new LevelConfig { // ด่าน 3
                Time = 900f,
                RoadLanes = 4,
                RiverLanes = 4,
                LotusScore = 1000,
                LevelClearBonus = 3000,
                TurtleSpeed = 3.2f,
                CrocodileSpeed = 4.2f,
                FishBlueSpeed = 4.2f,
                FishRedSpeed = 3.5f
            },
        };
        private static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
        // ทำชุดเฟรม sprite สำหรุบสัตว์ที่มีเฟรมแค่ทิศเดียว
        private static Image[] MirrorFrames(Image[] frames)
        {
            var mirrored = new Image[frames.Length];
            for (int i = 0; i < frames.Length; i++)
            {
                var src = frames[i];
                var bmp = new Bitmap(src.Width, src.Height);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.TranslateTransform(bmp.Width, 0);
                    g.ScaleTransform(-1, 1);
                    g.DrawImage(src, new Rectangle(0, 0, bmp.Width, bmp.Height));
                }
                mirrored[i] = bmp;
            }
            return mirrored;
        }
        private LevelConfig GetLevelConfig(int lvl)
        {
            int i = lvl - 1;
            if (i < 0) i = 0;
            if (i >= Levels.Length) i = Levels.Length - 1;
            return Levels[i];
        }

        // logic update game state
        public void Update(float deltaTime)
        {
            if (gameState != GameState.Playing) return;

            // Gametimer
            bool timeRanOut = gameTimer.Update(deltaTime, IsGodMode ? null : playerFrog);
            if (timeRanOut)
            {
                if (IsGodMode)
                {
                    gameTimer.Reset();
                    return;
                }

                if (playerFrog.IsDead)
                {
                    gameState = GameState.GameOver;
                    return;
                }
                Soundplayer.PlayDmgSound();
                return;
            }

            // Update EnemyMonster , FriendMonster
            foreach (var enemy in enemies)
                enemy.Update(deltaTime);

            foreach (var friend in friends)
                friend.Update(deltaTime);

            // 3. จัดการการเกิดและหมดอายุของไอเทมพิเศษกลางถนน (Item - 10 วินาที)
            if (!currentItem.IsActive)
            {
                itemSpawnTimer -= deltaTime;
                if (itemSpawnTimer <= 0)
                {
                    currentItem.SpawnRandom(road.RoadBounds, rng);
                    itemSpawnTimer = ItemSpawnInterval;
                }
            }
            else
            {
                currentItem.Update(deltaTime);
                // ตรวจสอบกบเก็บไอเทม
                if (currentItem.CheckCollect(playerFrog))
                {
                    score += currentItem.ScoreBonus;
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

                        // กบชนศัตรูลดหัวใจ
                        Soundplayer.PlayDmgSound();
                        bool isGameOver = playerFrog.TakeDamage();

                        if (isGameOver)
                        {
                            gameState = GameState.GameOver;
                        }
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
                            score += GetLevelConfig(level).LevelClearBonus;
                            gameTimer.Reset();
                            gameState = level >= MaxLevel
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

                    // หลุดออกนอกจอ
                    if (playerFrog.X < -playerFrog.Width || playerFrog.X > jumpingFiled.Width)
                    {
                        if (IsGodMode)
                        {
                            jumpingFiled.ClampFrogPosition(playerFrog);
                            return;
                        }

                        Soundplayer.PlayDmgSound();
                        bool isGameOver = playerFrog.TakeDamage();
                        if (isGameOver)
                        {
                            gameState = GameState.GameOver;
                        }
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

                    // กบตกน้ำ ตายและลดหัวใจ 1 ดวง
                    Soundplayer.PlayDmgSound();
                    bool isGameOver = playerFrog.TakeDamage();
                    if (isGameOver)
                    {
                        gameState = GameState.GameOver;
                    }
                    return;
                }
            }

            // ล็อคตำแหน่งกบไม่ให้ออกนอกขอบเขตสนาม
            jumpingFiled.ClampFrogPosition(playerFrog);
        }
        private bool CheckAllLotusesOccupied()
        {
            foreach (var lotus in targetLotuses)
            {
                if (!lotus.IsOccupied) return false;
            }
            return true;
        }

        // --- จัดการสถานะเกม ---
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
