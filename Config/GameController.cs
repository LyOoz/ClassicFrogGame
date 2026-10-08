using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;

namespace JumfrogbyMark
{
    public enum GameState
    {
        Playing,
        Paused,
        GameOver,
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
        public GameState State => gameState;
        public bool IsGameOver => gameState == GameState.GameOver;
        public bool IsPaused => gameState == GameState.Paused;
        public bool IsVictory => gameState == GameState.Victory;

        // constructor
        public GameController(int fieldWidth = 1008, int fieldHeight = 561)
        {
            this.rng = new Random();
            this.score = 0;
            this.level = 1;
            this.gameState = GameState.Playing;
            this.jumpingFiled = new JumpingFiled(fieldWidth, fieldHeight, startY: 515, medianY: 270, goalY: 1);
            this.river = new River(x: 0, y: 45, width: fieldWidth, height: 220);
            this.road = new Road(x: 0, y: 315, width: fieldWidth, height: 195, lanesCount: 4);
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
            // default level 1
            InitializeLevel(1);
        }

        // constructor ของ สัตว์แต่ละประเภท
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
            this.level = newLevel;
            LevelConfig cfg = GetLevelConfig(newLevel);
            this.enemies.Clear();
            this.friends.Clear();
            this.targetLotuses.Clear();

            // สร้างใบบัวเป้าหมาย
            int lotusCount = 5;
            int segmentW = jumpingFiled.Width / lotusCount;
            for (int i = 0; i < lotusCount; i++)
            {
                int lotusX = i * segmentW + (segmentW / 2) - 25;
                targetLotuses.Add(new TargetLotus(lotusX, 5, width: SpriteConfig.LotusWidth, height: SpriteConfig.LotusHeight, scoreValue: 500));
            }

            // config เลน EnemyMonster
            int roadLanes = 3;
            var roadLaneSettings = new (int dir, EnemyType type, int count, int x0, int y0, int minX, int maxX)[]
            {
                // -1=ซ้าย 1=ขวา , type, จำนวนตัว, start(default x, default y), end(x, y)
                ( 1, EnemyType.turtle,     3, 12,  348, -84, 1000), // เลนแรก บนสุด
                (-1, EnemyType.turtle,     3, 915, 390, -84, 1200),
                ( 1, EnemyType.crocodile,  3, 12,  455, -84, 1000),
            };
            roadLaneStarts.Clear();
            for (int lane = 0; lane < roadLanes; lane++)
            {
                roadLaneStarts.Add(enemies.Count);
                var (dir, type, count, x0, y0, minX, maxX) = roadLaneSettings[lane];
                for (int i = 0; i < count; i++)
                {
                    var enemySet = type == EnemyType.turtle ? SpriteConfig.Turtle : SpriteConfig.Crocodile;
                    float enemySpeed = type == EnemyType.turtle ? cfg.TurtleSpeed : cfg.CrocodileSpeed;
                    var enemy = new EnemyMonster(x0 + i * enemySet.Spacing, y0, enemySet.Width, enemySet.Height, baseSpeed: enemySpeed, movingRight: dir > 0, type: type, minX: minX, maxX: maxX);
                    enemy.Sprite = enemySet.Get(dir > 0);
                    // ตั้งเฟรม animation เดินตามชนิดและทิศทาง
                    if (type == EnemyType.turtle)
                        enemy.SetWalkFrames(SpriteConfig.TurtleLeftFrames, SpriteConfig.TurtleRightFrames);
                    else
                    {
                        // จระเข้มีเฟรมแค่ขวา → mirror เป็นซ้าย
                        enemy.SetWalkFrames(MirrorFrames(SpriteConfig.CrocodileRightFrames), SpriteConfig.CrocodileRightFrames);
                    }
                    enemies.Add(enemy);
                }
            }

            // config เลน FriendMonster
            int riverLanes = 3;
            var riverLaneSettings = new (int dir, FriendType type, int count, int x0, int y0, int minX, int maxX)[]
            {
                // -1=ซ้าย 1=ขวา , type, จำนวนตัว, start(default x, default y), end(x, y)
                (-1, FriendType.FishBlue, 3, 930, 140,  -84, 1200), // เลนแรก บนสุด
                ( 1, FriendType.FishRed,  3, 1,  195,  -84, 1000),
                (-1, FriendType.FishRed,  3, 930, 250, -84, 1200),
            };
            riverLaneStarts.Clear();
            for (int lane = 0; lane < riverLanes; lane++)
            {
                riverLaneStarts.Add(friends.Count);
                var (dir, type, count, x0, y0, minX, maxX) = riverLaneSettings[lane];
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
            public float TurtleSpeed;
            public float CrocodileSpeed;
            public float FishBlueSpeed;
            public float FishRedSpeed;
        }
        private static readonly LevelConfig[] Levels =
        {
            new LevelConfig { // ด่าน 1
                Time = 120f,
                TurtleSpeed = 2.6f,
                CrocodileSpeed = 2.6f,
                FishBlueSpeed = 2.6f,
                FishRedSpeed = 2.6f
            },
            new LevelConfig { // ด่าน 2
                Time = 30f,
                TurtleSpeed = 2.0f,
                CrocodileSpeed = 2.0f,
                FishBlueSpeed = 2.0f,
                FishRedSpeed = 2.0f
            },
            new LevelConfig { // ด่าน 3
                Time = 25f,
                TurtleSpeed = 2.4f,
                CrocodileSpeed = 2.4f,
                FishBlueSpeed = 2.4f,
                FishRedSpeed = 2.4f
            },
        };
        // ทำ mirror (กลับซ้าย-ขวา) ให้ชุดเฟรม sprite สำหรุบสัตว์ท่ีมีเฟรมแค่ทิศเดียว
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

        //logic update game state
        public void Update(float deltaTime)
        {
            if (gameState != GameState.Playing) return;

            // Gametimer
            bool timeRanOut = gameTimer.Update(deltaTime, playerFrog);
            if (timeRanOut)
            {
                if (playerFrog.IsDead)
                {
                    gameState = GameState.GameOver;
                    return;
                }
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
                        // กบชนศัตรู กบตายและลดหัวใจ 1 ดวง
                        bool isGameOver = playerFrog.TakeDamage();
                        Soundplayer.PlayDmgSound();
                        gameTimer.Reset();
                        if (isGameOver)
                        {
                            gameState = GameState.GameOver;
                            return;
                        }
                        break;
                    }
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
                        bool isGameOver = playerFrog.TakeDamage();
                        Soundplayer.PlayDmgSound();
                        gameTimer.Reset();
                        if (isGameOver)
                        {
                            gameState = GameState.GameOver;
                            return;
                        }
                    }
                }
                else
                {
                    // กบตกน้ำ ตายและลดหัวใจ 1 ดวง
                    bool isGameOver = playerFrog.TakeDamage();
                    Soundplayer.PlayDmgSound();
                    gameTimer.Reset();
                    if (isGameOver)
                    {
                        gameState = GameState.GameOver;
                        return;
                    }
                }
            }

            // 6. ตรวจสอบว่ากบกระโดดถึงใบบัวเป้าหมาย (TargetLotus) หรือไม่
            foreach (var lotus in targetLotuses)
            {
                if (lotus.CheckReached(playerFrog))
                {
                    if (!lotus.IsOccupied)
                    {
                        lotus.Occupy();
                        score += lotus.ScoreValue + (int)(gameTimer.TimeRemaining * 10);
                        playerFrog.ResetToStart();
                        gameTimer.Reset();

                        // ตรวจสอบว่าพิชิตใบบัวครบทุกใบหรือยัง
                        if (CheckAllLotusesOccupied())
                        {
                            score += 1000;
                            level++;
                            InitializeLevel(level);
                        }
                    }
                    else
                    {
                        // ชนใบบัวที่มีคนจองแล้ว -> ถอยกลับ
                        playerFrog.ResetToStart();
                    }
                    break;
                }
            }

            // ล็อคตำแหน่งกบไม่ให้ออกนอกขอบเขตสนาม
            jumpingFiled.ClampFrogPosition(playerFrog);
        }

        /// <summary>
        /// ตรวจสอบว่าใบบัวทุกใบถูกพิชิตแล้วหรือไม่
        /// </summary>
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
