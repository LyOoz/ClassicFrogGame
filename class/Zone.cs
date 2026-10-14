using System;
using System.Drawing;

namespace ClassicFrogJump
{
    public abstract class Zone : GameObject
    {
        // Constructor
        public Zone(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// ตรวจว่าจุดศูนย์กลางของกบอยู่ภายในโซนนี้หรือไม่
        public virtual bool ContainsFrog(PlayerFrog frog)
        {
            Rectangle frogBounds = frog.Bounds;
            Point frogCenter = new Point(frogBounds.X + frogBounds.Width / 2, frogBounds.Y + frogBounds.Height / 2);
            return Bounds.Contains(frogCenter);
        }
    }
}
