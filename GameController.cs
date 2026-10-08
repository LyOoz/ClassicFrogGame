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
        public GameController(int fieldWidth = 396, int fieldHeight = 510)
        {
            this.rng = new Random();
            this.score = 0;
            this.level = 1;
            this.gameState = GameState.Playing;

            // สร้างส่วนประกอบของเกม
            this.jumpingFiled = new JumpingFiled(fieldWidth, fieldHeight, startY: 515, medianY: 270, goalY: 1);
            this.river = new River(x: 0, y: 45, width: fieldWidth, height: 220);
            this.road = new Road(x: 0, y: 315, width: fieldWidth, height: 195, lanesCount: 4);
            this.playerFrog = new PlayerFrog(startX: fieldWidth , startY: fieldHeight, width: 40, height: 40, initialHearts: 5, stepSize: 30);
            this.gameTimer = new GameTimer(defaultLimitSeconds: 30.0f);
            this.currentItem = new Item(width: 32, height: 32, durationSeconds: 10.0f);

            this.enemies = new List<EnemyMonster>();
            this.friends = new List<FriendMonster>();
            this.targetLotuses = new List<TargetLotus>();
            // default level 1
            InitializeLevel(1);
        }

        // method create object แต่ละอัน
        public void InitializeLevel(int newLevel)
        {
            this.level = newLevel;
            this.enemies.Clear();
            this.friends.Clear();
            this.targetLotuses.Clear();

            // 1. สร้างใบบัวเป้าหมาย (TargetLotus) 5 ตำแหน่งแถวบนสุด
            int lotusCount = 5;
            int segmentW = jumpingFiled.Width / lotusCount;
            for (int i = 0; i < lotusCount; i++)
            {
                int lotusX = i * segmentW + (segmentW / 2) - 25;
                targetLotuses.Add(new TargetLotus(lotusX, 5, width: 50, height: 35, scoreValue: 500));
            }

            // 2. สร้างศัตรูบนถนน (EnemyMonster) - อย่างน้อย 2 แบบ (Snake และ Car)
            // เลน 1: รถวิ่งไปขวา (Car)
            for (int i = 0; i < 3; i++)
            {
                var enemy = new EnemyMonster(i * 270 + 30, road.Y + 8, 70, 38, baseSpeed: 1.6f, movingRight: true, type: EnemyType.turtle);
                enemy.ApplyLevelSpeed(level);
                enemies.Add(enemy);
            }

            // เลน 2: รถวิ่งไปซ้าย (Car)
            for (int i = 0; i < 3; i++)
            {
                var enemy = new EnemyMonster(i * 260 + 80, road.Y + 54, 70, 38, baseSpeed: 2.1f, movingRight: false, type: EnemyType.crocodile);
                enemy.ApplyLevelSpeed(level);
                enemies.Add(enemy);
            }

            // เลน 3: งูพิษเลื้อยไปขวา/ซ้าย (Snake)
            for (int i = 0; i < 2; i++)
            {
                var snake = new EnemyMonster(i * 380 + 50, road.Y + 102, 90, 36, baseSpeed: 1.8f, movingRight: true, type: EnemyType.crocodile);
                snake.ApplyLevelSpeed(level);
                enemies.Add(snake);
            }

            // เลน 4: รถวิ่งเร็วไปซ้าย (Car)
            for (int i = 0; i < 3; i++)
            {
                var enemy = new EnemyMonster(i * 250 + 60, road.Y + 148, 70, 38, baseSpeed: 2.6f, movingRight: false, type: EnemyType.turtle);
                enemy.ApplyLevelSpeed(level);
                enemies.Add(enemy);
            }

            // 3. สร้างเพื่อนในแม่น้ำ (FriendMonster) - อย่างน้อย 2 แบบ (Turtle และ Fish)
            // แถว 1: เต่าว่ายน้ำไปขวา (Turtle)
            for (int i = 0; i < 3; i++)
            {
                var turtle = new FriendMonster(i * 270 + 40, river.Y + 8, 80, 36, baseSpeed: 1.3f, movingRight: true, type: FriendType.Turtle);
                turtle.ApplyLevelSpeed(level);
                friends.Add(turtle);
            }

            // แถว 2: ปลาใหญ่ว่ายไปซ้าย (Fish)
            for (int i = 0; i < 2; i++)
            {
                var fish = new FriendMonster(i * 380 + 60, river.Y + 50, 110, 36, baseSpeed: 1.7f, movingRight: false, type: FriendType.Fish);
                fish.ApplyLevelSpeed(level);
                friends.Add(fish);
            }

            // แถว 3: เต่าว่ายน้ำไปขวา (Turtle)
            for (int i = 0; i < 3; i++)
            {
                var turtle = new FriendMonster(i * 280 + 30, river.Y + 95, 80, 36, baseSpeed: 1.5f, movingRight: true, type: FriendType.Turtle);
                turtle.ApplyLevelSpeed(level);
                friends.Add(turtle);
            }

            // แถว 4: ปลาใหญ่ว่ายไปซ้าย (Fish)
            for (int i = 0; i < 3; i++)
            {
                var fish = new FriendMonster(i * 260 + 50, river.Y + 140, 95, 36, baseSpeed: 2.0f, movingRight: false, type: FriendType.Fish);
                fish.ApplyLevelSpeed(level);
                friends.Add(fish);
            }

            // แถว 5: เต่าว่ายน้ำไปขวา (Turtle)
            for (int i = 0; i < 2; i++)
            {
                var turtle = new FriendMonster(i * 360 + 80, river.Y + 182, 100, 36, baseSpeed: 1.4f, movingRight: true, type: FriendType.Turtle);
                turtle.ApplyLevelSpeed(level);
                friends.Add(turtle);
            }

            // 4. รีเซ็ตกบ ไอเทม และเวลา
            playerFrog.ResetToStart();
            currentItem.Reset();
            itemSpawnTimer = 3.0f; // รอ 3 วินาทีก่อนสุ่มเกิดไอเทมชิ้นแรก
            gameTimer.Start(Math.Max(15.0f, 35.0f - (level - 1) * 2.0f));
            gameState = GameState.Playing;
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
                score += 10;
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
