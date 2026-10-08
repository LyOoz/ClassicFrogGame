using System;
using System.Drawing;

namespace JumfrogbyMark
{
    public enum FriendType
    {
        Fish     
    }

    public class FriendMonster : Monster
    {
        // field
        private FriendType type;

        // properties
        public FriendType Type { get => type; set => type = value; }

        // constructor
        public FriendMonster(float x, float y, int width, int height, float baseSpeed, bool movingRight, FriendType type)
            : base(x, y, width, height, baseSpeed, movingRight)
        {
            this.type = type;
        }

        // method
        public override void ApplyLevelSpeed(int level)
        {
            float multiplier = 1.0f + (level - 1) * 0.2f;
            this.speed = baseSpeed * multiplier;
        }

        /// ตรวจสอบว่ากบอยู่บนตัว friend monster มั้ย
        public bool IsFrogOnTop(PlayerFrog frog)
        {
            Rectangle frogRect = frog.Bounds;
            Point frogCenter = new Point(frogRect.X + frogRect.Width / 2, frogRect.Y + frogRect.Height / 2);
            return this.Bounds.Contains(frogCenter) || this.Bounds.IntersectsWith(frogRect);
        }
        /// ขี่คอตามเพื่อน
        public void Carry(PlayerFrog frog)
        {
            float dx = movingRight ? speed : -speed;
            frog.X += (int)Math.Round(dx);
        }
    }
}
