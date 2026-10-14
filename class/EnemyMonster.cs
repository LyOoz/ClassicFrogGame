using System;
using System.Drawing;

namespace ClassicFrogJump
{
    public enum EnemyType
    {
        turtle,
        crocodile
    }
    public class EnemyMonster : Monster
    {
        // constructor
        public EnemyMonster(float x, float y, int width, int height, float baseSpeed, bool movingRight, EnemyType type, int minX = -100, int maxX = 900)
            : base(x, y, width, height, baseSpeed, movingRight, minX, maxX)
        {
        }
        // ตรวจสอบการชนกับกบ
        public bool CheckCollision(PlayerFrog frog)
        {
            return IntersectsWith(frog);
        }
    }
}
