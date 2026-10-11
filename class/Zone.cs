using System;
using System.Drawing;

namespace JumfrogbyMark
{
    public abstract class Zone
    {
        // field
        protected int x;
        protected int y;
        protected int width;
        protected int height;
        protected Rectangle bounds;

        // Properties
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public Rectangle Bounds => bounds;

        // Constructor
        public Zone(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.bounds = new Rectangle(x, y, width, height);
        }

        /// ตรวจว่าจุดศูนย์กลางของกบอยู่ภายในโซนนี้หรือไม่
        public virtual bool ContainsFrog(PlayerFrog frog)
        {
            Rectangle frogBounds = frog.Bounds;
            Point frogCenter = new Point(frogBounds.X + frogBounds.Width / 2, frogBounds.Y + frogBounds.Height / 2);
            return bounds.Contains(frogCenter);
        }
    }
}
