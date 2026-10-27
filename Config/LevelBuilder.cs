using System;
using System.Collections.Generic;

namespace ClassicFrogJump
{
    public static class LevelBuilder
    {
        private static int PickLaneStartX(Random rng, int minX, int maxX, int count, int spacing, int width)
        {
            int span = (count - 1) * spacing + width;
            int lo = minX;
            int hi = maxX - span;
            if (hi < lo) hi = lo; // ขอบเขตแคบเกินไป ให้เว้นกันเต็มช่วง

            return rng.Next(lo, hi + 1);
        }

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
                var enemySet = type == EnemyType.turtle ? SpriteConfig.Turtle : SpriteConfig.Crocodile;
                int x0 = PickLaneStartX(rng, minX, maxX, count, enemySet.Spacing, enemySet.Width);

                for (int i = 0; i < count; i++)
                {
                    var enemy = new EnemyMonster(x0 + i * enemySet.Spacing, y0, enemySet.Width, enemySet.Height, baseSpeed: type == EnemyType.turtle ? cfg.TurtleSpeed : cfg.CrocodileSpeed, movingRight: dir > 0, type: type, minX: minX, maxX: maxX);
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
                var friendSet = type == FriendType.FishBlue ? SpriteConfig.FishBlue : SpriteConfig.FishRed;
                int x0 = PickLaneStartX(rng, minX, maxX, count, friendSet.Spacing, friendSet.Width);

                for (int i = 0; i < count; i++)
                {
                    var friend = new FriendMonster(x0 + i * friendSet.Spacing, y0, friendSet.Width, friendSet.Height, baseSpeed: type == FriendType.FishBlue ? cfg.FishBlueSpeed : cfg.FishRedSpeed, movingRight: dir > 0, type: type, minX: minX, maxX: maxX);
                    friend.Sprite = friendSet.Get(dir > 0);
                    friends.Add(friend);
                }
            }
        }
    }
}
