using System;
using System.Collections.Generic;

namespace JumfrogbyMark
{
    public static class LevelBuilder
    {
        public static void Build(
            LevelConfig cfg,
            Random rng,
            List<EnemyMonster> enemies,
            List<FriendMonster> friends,
            List<TargetLotus> targetLotuses)
        {
            BuildLotuses(cfg, targetLotuses);
            BuildRoadLanes(cfg, rng, enemies);
            BuildRiverLanes(cfg, rng, friends);
        }

        private static void BuildLotuses(LevelConfig cfg, List<TargetLotus> targetLotuses)
        {
            for (int i = 0; i < cfg.LotusSettings.Length; i++)
            {
                var (lx, ly) = cfg.LotusSettings[i];
                var lotus = new TargetLotus(lx, ly, width: SpriteConfig.LotusWidth, height: SpriteConfig.LotusHeight, scoreValue: cfg.LotusScore);
                lotus.Sprite = SpriteConfig.Lotus;
                targetLotuses.Add(lotus);
            }
        }

        private static void BuildRoadLanes(
            LevelConfig cfg,
            Random rng,
            List<EnemyMonster> enemies)
        {
            for (int lane = 0; lane < cfg.RoadLanes; lane++)
            {
                var (dir, type, count, y0, minX, maxX) = cfg.RoadLaneSettings[lane];
                int x0 = rng.Next(minX, maxX - 200);

                for (int i = 0; i < count; i++)
                {
                    var enemySet = type == EnemyType.turtle ? SpriteConfig.Turtle : SpriteConfig.Crocodile;
                    float enemySpeed = type == EnemyType.turtle ? cfg.TurtleSpeed : cfg.CrocodileSpeed;
                    var enemy = new EnemyMonster(x0 + i * enemySet.Spacing, y0, enemySet.Width, enemySet.Height, baseSpeed: enemySpeed, movingRight: dir > 0, type: type, minX: minX, maxX: maxX);
                    enemy.Sprite = enemySet.Get(dir > 0);

                    if (type == EnemyType.turtle)
                    {
                        enemy.SetWalkFrames(SpriteConfig.TurtleLeftFrames, SpriteConfig.TurtleRightFrames);
                    }
                    else
                    {
                        enemy.SetWalkFrames(SpriteConfig.CrocodileLeftFrames, SpriteConfig.CrocodileRightFrames);
                    }

                    enemies.Add(enemy);
                }
            }
        }

        private static void BuildRiverLanes(LevelConfig cfg, Random rng, List<FriendMonster> friends)
        {
            for (int lane = 0; lane < cfg.RiverLanes; lane++)
            {
                var (dir, type, count, y0, minX, maxX) = cfg.RiverLaneSettings[lane];
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
        }
    }
}
