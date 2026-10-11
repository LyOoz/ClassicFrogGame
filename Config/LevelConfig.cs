using System;

namespace JumfrogbyMark
{
    public struct LevelConfig
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
        public float ItemFirstSpawnDelay;
        public float ItemSpawnInterval;
        public float ItemExtraHeartChance;
        public int ItemHeartBonus;
        public float ItemTimeBonusSeconds;

        // Level specific settings
        public (int x, int y)[] LotusSettings;
        public (int dir, EnemyType type, int count, int y0, int minX, int maxX)[] RoadLaneSettings;
        public (int dir, FriendType type, int count, int y0, int minX, int maxX)[] RiverLaneSettings;
    }

    public static class LevelConfigManager
    {
        public const int MaxLevel = 3;

        public static readonly LevelConfig[] Levels =
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
                FishRedSpeed = 2.3f,
                ItemFirstSpawnDelay = 6.0f,
                ItemSpawnInterval = 18.0f,
                ItemExtraHeartChance = 0.40f,
                ItemHeartBonus = 1,
                ItemTimeBonusSeconds = 10.0f,
                LotusSettings = new (int x, int y)[]
                {
                    (58,  85),
                    (260, 85),
                    (461, 85),
                    (663, 85),
                    (865, 85),
                },
                RoadLaneSettings = new (int dir, EnemyType type, int count, int y0, int minX, int maxX)[]
                {
                    ( 1, EnemyType.turtle,     3, 448, -84, 1000),
                    (-1, EnemyType.turtle,     3, 490, -84, 1200),
                    ( 1, EnemyType.crocodile,  3, 555, -84, 1000),
                    (-1, EnemyType.crocodile,  3, 368, -84, 1200),
                },
                RiverLaneSettings = new (int dir, FriendType type, int count, int y0, int minX, int maxX)[]
                {
                    (-1, FriendType.FishBlue,  3, 180, -84, 1200),
                    ( 1, FriendType.FishRed,   3, 235, -84, 1000),
                    (-1, FriendType.FishBlue,  3, 290, -84, 1200),
                    (-1, FriendType.FishRed,   3, 125, -84, 1200),
                }
            },
            new LevelConfig { // ด่าน 2
                Time = 1200f,
                RoadLanes = 4,
                RiverLanes = 3,
                LotusScore = 250,
                LevelClearBonus = 1200,
                TurtleSpeed = 2.6f,
                CrocodileSpeed = 3.6f,
                FishBlueSpeed = 3.1f,
                FishRedSpeed = 3.6f,
                ItemFirstSpawnDelay = 7.0f,
                ItemSpawnInterval = 20.0f,
                ItemExtraHeartChance = 0.35f,
                ItemHeartBonus = 1,
                ItemTimeBonusSeconds = 8.0f,
                LotusSettings = new (int x, int y)[]
                {
                    (58,  85),
                    (260, 85),
                    (461, 85),
                    (663, 85),
                    (865, 85),
                },
                RoadLaneSettings = new (int dir, EnemyType type, int count, int y0, int minX, int maxX)[]
                {
                    ( 1, EnemyType.turtle,     3, 448, -84, 1000),
                    (-1, EnemyType.turtle,     3, 490, -84, 1200),
                    ( 1, EnemyType.crocodile,  3, 555, -84, 1000),
                    (-1, EnemyType.crocodile,  3, 368, -84, 1200),
                },
                RiverLaneSettings = new (int dir, FriendType type, int count, int y0, int minX, int maxX)[]
                {
                    (-1, FriendType.FishBlue,  3, 180, -84, 1200),
                    ( 1, FriendType.FishRed,   3, 235, -84, 1000),
                    (-1, FriendType.FishBlue,  3, 290, -84, 1200),
                    (-1, FriendType.FishRed,   3, 125, -84, 1200),
                }
            },
            new LevelConfig { // ด่าน 3
                Time = 900f,
                RoadLanes = 4,
                RiverLanes = 4,
                LotusScore = 500,
                LevelClearBonus = 2000,
                TurtleSpeed = 3.2f,
                CrocodileSpeed = 4.2f,
                FishBlueSpeed = 4.2f,
                FishRedSpeed = 3.5f,
                ItemFirstSpawnDelay = 8.0f,
                ItemSpawnInterval = 22.0f,
                ItemExtraHeartChance = 0.30f,
                ItemHeartBonus = 1,
                ItemTimeBonusSeconds = 6.0f,
                LotusSettings = new (int x, int y)[]
                {
                    (75,  70),
                    (275, 70),
                    (475, 70),
                    (675, 70),
                    (875, 70),
                },
                RoadLaneSettings = new (int dir, EnemyType type, int count, int y0, int minX, int maxX)[]
                {
                    ( 1, EnemyType.turtle,     3, 448, -84, 1000),
                    (-1, EnemyType.turtle,     3, 490, -84, 1200),
                    ( 1, EnemyType.crocodile,  3, 555, -84, 1000),
                    (-1, EnemyType.crocodile,  3, 368, -84, 1200),
                },
                RiverLaneSettings = new (int dir, FriendType type, int count, int y0, int minX, int maxX)[]
                {
                    (-1, FriendType.FishBlue,  3, 180, -84, 1200),
                    ( 1, FriendType.FishRed,   3, 235, -84, 1000),
                    (-1, FriendType.FishBlue,  3, 290, -84, 1200),
                    (-1, FriendType.FishRed,   3, 125, -84, 1200),
                }
            },
        };

        public static LevelConfig GetLevelConfig(int lvl)
        {
            int i = lvl - 1;
            if (i < 0) i = 0;
            if (i >= Levels.Length) i = Levels.Length - 1;
            return Levels[i];
        }
    }
}
