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
        // จำ index ตัวแรกของแต้ละเลน/แถว เพื่อให้ง่ายต่อการเลื้ยง index
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

        // constructor (config main game this here)
        public GameController(int fieldWidth = 396, int fieldHeight = 510)
        {
            this.rng = new Random();
            this.score = 0;
            this.level = 1;
            this.gameState = GameState.Playing;
            this.jumpingFiled = new JumpingFiled(fieldWidth, fieldHeight, startY: 515, medianY: 270, goalY: 1);
            this.river = new River(x: 0, y: 45, width: fieldWidth, height: 220);
            this.road = new Road(x: 0, y: 315, width: fieldWidth, height: 195, lanesCount: 4);
            this.playerFrog = new PlayerFrog(startX: fieldWidth , startY: fieldHeight, width: 40, height: 40, initialHearts: 5, stepSize: 30);
            this.gameTimer = new GameTimer();
            this.currentItem = new Item(width: 32, height: 32, durationSeconds: 10.0f);
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

            //สร้างใบบัวเป้าหมาย
            int lotusCount = 5;
            int segmentW = jumpingFiled.Width / lotusCount;
            for (int i = 0; i < lotusCount; i++)
            {
                int lotusX = i * segmentW + (segmentW / 2) - 25;
                targetLotuses.Add(new TargetLotus(lotusX, 5, width: 50, height: 35, scoreValue: 500));
            }

            // config เลน EnemyMonster
            int roadLanes = 3;
            var roadLaneSettings = new (int dir, EnemyType type, int count, int x0)[]
            {
                // -1=ซ้าย 1=ขวา , type, จำนวนตัว, default x
                ( 1, EnemyType.turtle,     3, 12),
                (-1, EnemyType.turtle,     3, 753),
                ( 1, EnemyType.crocodile,  3, 12),
            };
            roadLaneStarts.Clear();
            for (int lane = 0; lane < roadLanes; lane++)
            {
                roadLaneStarts.Add(enemies.Count);
                var (dir, type, count, x0) = roadLaneSettings[lane];
                int laneY = road.Y + 8 + lane * 46;
                for (int i = 0; i < count; i++)
                {
                    var enemy = new EnemyMonster(x0 + i * 270, laneY, 70, 38, baseSpeed: cfg.EnemySpeeds[lane], movingRight: dir > 0, type: type);
                    enemies.Add(enemy);
                }
            }

            // config เลน FriendMonster (ความเร็วแต่ละเลนมาจาก cfg.FriendSpeeds)
            int riverLanes = 3;
            var riverLaneSettings = new (int dir, FriendType type, int count, int x0)[]
            {
                // -1=ซ้าย 1=ขวา , type, จำนวนตัว, default x
                (-1, FriendType.FishBlue, 3, 753),
                ( 1, FriendType.FishRed, 3, 12),
                (-1, FriendType.FishRed, 3, 753),
            };
            riverLaneStarts.Clear();
            for (int lane = 0; lane < riverLanes; lane++)
            {
                riverLaneStarts.Add(friends.Count);
                var (dir, type, count, x0) = riverLaneSettings[lane];
                int laneY = river.Y + 8 + lane * 45;
                for (int i = 0; i < count; i++)
                {
                    var friend = new FriendMonster(x0 + i * 270, laneY, 90, 36, baseSpeed: cfg.FriendSpeeds[lane], movingRight: dir > 0, type: type);
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
            public float[] EnemySpeeds;
            public float[] FriendSpeeds;
        }
        private static readonly LevelConfig[] Levels =
        {
            new LevelConfig { // ด่าน 1
                Time = 120f, 
                EnemySpeeds = new[] { 10f, 1.6f, 1.6f }, // ความเร็วแต่ละเลน
                FriendSpeeds = new[] { 1.6f, 1.6f, 1.6f } 
            },
            new LevelConfig { // ด่าน 2
                Time = 30f,  
                EnemySpeeds = new[] { 10f, 2.0f, 2.0f }, 
                FriendSpeeds = new[] { 2.0f, 2.0f, 2.0f } 
            },  
            new LevelConfig { // ด่าน 3
                Time = 25f,  
                EnemySpeeds = new[] { 10f, 2.4f, 2.4f }, 
                FriendSpeeds = new[] { 2.4f, 2.4f, 2.4f } 
            },  
        };
        private LevelConfig GetLevelConfig(int lvl)
        {
            int i = lvl - 1;
            if (i < 0) i = 0;
            if (i >= Levels.Length) i = Levels.Length - 1;
            return Levels[i];
        }

        /// <summary>
        /// อัปเดตตรรกะเกมทั้งหมดในแต่ละเฟรม (Game Loop)
        /// </summary>
        /// <param name="deltaTime">เวลาที่ผ่านไป (วินาที)</param>
        public void Update(float deltaTime)
        {
            if (gameState != GameState.Playing) return;

            // 1. อัปเดตนาฬิกาจับเวลา (GameTimer)
            bool timeRanOut = gameTimer.Update(deltaTime, playerFrog);
            if (timeRanOut)
            {
                if (playerFrog.IsDead)
                {
                    gameState = GameState.GameOver;
                    return;
                }
            }

            // 2. อัปเดตการเคลื่อนที่ของศัตรูและเพื่อน
            foreach (var enemy in enemies)
                enemy.Update(-100, jumpingFiled.Width + 100);

            foreach (var friend in friends)
                friend.Update(-140, jumpingFiled.Width + 140);

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

            // 4. ตรวจสอบการชนกับศัตรูบนถนน (EnemyMonster)
            if (road.IsFrogOnRoad(playerFrog))
            {
                foreach (var enemy in enemies)
                {
                    if (enemy.CheckCollision(playerFrog))
                    {
                        // กบชนศัตรู กบตายและลดหัวใจ 1 ดวง
                        bool isGameOver = playerFrog.TakeDamage();
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

            // 5. ตรวจสอบแม่น้ำและการเกาะหลังเพื่อน (River & FriendMonster)
            if (river.IsFrogInRiver(playerFrog))
            {
                // ตรวจสอบว่าอยู่บนหลังเพื่อนหรือไม่
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
                    // กบลอยไปตามเพื่อนที่กำลังว่ายน้ำ
                    carryingFriend.Carry(playerFrog);

                    // ถ้าพาหลุดออกนอกจอ
                    if (playerFrog.X < -playerFrog.Width || playerFrog.X > jumpingFiled.Width)
                    {
                        bool isGameOver = playerFrog.TakeDamage();
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
                    // กบตกน้ำในแม่น้ำ -> ตายและลดหัวใจ 1 ดวง
                    bool isGameOver = playerFrog.TakeDamage();
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

        // --- การควบคุมการเคลื่อนที่ของกบ ---
        public void MoveFrogUp()
        {
            if (gameState == GameState.Playing)
            {
                playerFrog.MoveUp(0);
            }
        }
        public void MoveFrogDown()
        {
            if (gameState == GameState.Playing)
                playerFrog.MoveDown(jumpingFiled.Height);
        }

        public void MoveFrogLeft()
        {
            if (gameState == GameState.Playing)
                playerFrog.MoveLeft(0);
        }

        public void MoveFrogRight()
        {
            if (gameState == GameState.Playing)
                playerFrog.MoveRight(jumpingFiled.Width);
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
