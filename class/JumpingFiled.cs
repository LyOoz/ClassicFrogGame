using System;
using System.Drawing;

namespace JumfrogbyMark
{
    /// จัดการขอบเขตพื้นที่การเล่น และการจำกัดพื้นที่
    public class JumpingFiled
    {
        // Fields
        private int width;
        private int height;
        private Rectangle fieldBounds;
        private Rectangle goalZone;   // พื้นที่ใบบัว

        // Properties
        public int Width => width;
        public int MinMoveX => fieldBounds.Left;
        public int MaxMoveX => fieldBounds.Right;
        public int MinMoveY => Math.Max(fieldBounds.Top, goalZone.Height);
        public int MaxMoveY => fieldBounds.Bottom;

        // Constructor
        public JumpingFiled(int width = 830, int height = 560, int startY = 515, int medianY = 270, int goalY = 0)
        {
            this.width = width;
            this.height = height;
            this.fieldBounds = new Rectangle(0, 0, width, height);
            this.goalZone = new Rectangle(0, goalY, width, 40);
        }

        /// บังคับให้ตำแหน่งของกบไม่หลุดออกจากพื้นที่
        public void ClampFrogPosition(PlayerFrog frog)
        {
            if (frog.X < 0) frog.X = 0;
            if (frog.X + frog.Width > width) frog.X = width - frog.Width;
            if (frog.Y < 0) frog.Y = 0;
            if (frog.Y + frog.Height > height) frog.Y = height - frog.Height;
        }

        public void MoveFrog(PlayerFrog frog, FrogDirection direction)
        {
            switch (direction)
            {
                case FrogDirection.Up:
                    frog.MoveUp(MinMoveY);
                    break;
                case FrogDirection.Down:
                    frog.MoveDown(MaxMoveY);
                    break;
                case FrogDirection.Left:
                    frog.MoveLeft(MinMoveX);
                    break;
                case FrogDirection.Right:
                    frog.MoveRight(MaxMoveX);
                    break;
            }

            ClampFrogPosition(frog);
        }
    }
}
