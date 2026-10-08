using System;
using System.Drawing;

namespace JumfrogbyMark
{
    public enum EnemyType
    {
        turtle,
        crocodile
    }
    public class EnemyMonster : Monster 
    {
        // field
        private EnemyType type;

        // properties
        public EnemyType Type { get => type; set => type = value; }

        // constructor
        public EnemyMonster(float x, float y, int width, int height, float baseSpeed, bool movingRight, EnemyType type)
            : base(x, y, width, height, baseSpeed, movingRight)
        {
            this.type = type;
        }
        /// ตรวจสอบการชนกับกบ ถ้าชน กบจะตายและหัวใจลดลง 1 ดวง
        public bool CheckCollision(PlayerFrog frog)
        {

            Rectangle enemyBox = this.Bounds;
            Rectangle frogBox = frog.Bounds;

            // ตรวจสอบว่ากรอบทั้งสองทับซ้อนกันหรือไม่
            if (enemyBox.IntersectsWith(frogBox))
            {
                return true;  // มีการชน
            }

            return false;     // ไม่ชน
        }
    }
}
