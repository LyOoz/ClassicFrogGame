using System;
using System.Drawing;

namespace JumfrogbyMark
{
    /// จัดการขอบเขตพื้นที่การเล่น พื้นที่ปลอดภัย และการจำกัดพื้นที่
    public class JumpingFiled
    {
        // Fields
        private int width;
        private int height;
        private Rectangle fieldBounds;
        private Rectangle startZone;
        private Rectangle medianZone; // ตรงกลาง
        private Rectangle goalZone;   // พื้นที่ใบบัว

        // Properties
        public int Width
        {
            get => width;
            set
            {
                width = value;
                fieldBounds.Width = value;
                startZone.Width = value;
                medianZone.Width = value;
                goalZone.Width = value;
            }
        }

        public int Height
        {
            get => height;
            set
            {
                height = value;
                fieldBounds.Height = value;
                startZone.Height = Math.Max(0, height - startZone.Y);
            }
        }

        public Rectangle FieldBounds => fieldBounds;
        public Rectangle StartZone => startZone;
        public Rectangle MedianZone => medianZone;
        public Rectangle GoalZone => goalZone;
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
            this.startZone = new Rectangle(0, startY, width, height - startY);
            this.medianZone = new Rectangle(0, medianY, width, 45);
            this.goalZone = new Rectangle(0, goalY, width, 40);
        }

        /// ตรวจสอบว่ากบยังอยู่ในพื้นที่มั้ย
        public bool IsWithinField(PlayerFrog frog)
        {
            return frog.X >= 0 &&
                   frog.X + frog.Width <= width &&
                   frog.Y >= 0 &&
                   frog.Y + frog.Height <= height;
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

        /// ตรวจกบอยู่ใน Start Zone หรือ Median Zone
        public bool IsInSafeZone(PlayerFrog frog)
        {
            return startZone.IntersectsWith(frog.Bounds) || medianZone.IntersectsWith(frog.Bounds);
        }
    }
}
