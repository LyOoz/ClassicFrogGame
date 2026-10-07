using System;
using System.Drawing;

namespace JumfrogbyMark
{
    /// <summary>
    /// คลาส JumpingFiled เป็นคลาสซึ่งเป็นตัวแทนของสนามต่อสู้
    /// จัดการขอบเขตพื้นที่การเล่น พื้นที่ปลอดภัย และการจำกัดพื้นที่
    /// </summary>
    public class JumpingFiled
    {
        // ฟิลด์
        private int width;
        private int height;
        private Rectangle fieldBounds;
        private Rectangle startZone;
        private Rectangle medianZone; // พื้นที่พัก/ปลอดภัยตรงกลาง
        private Rectangle goalZone;   // พื้นที่เป้าหมายแถวบนสุด

        // คุณสมบัติ (Properties)
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public Rectangle FieldBounds => fieldBounds;
        public Rectangle StartZone => startZone;
        public Rectangle MedianZone => medianZone;
        public Rectangle GoalZone => goalZone;

        // คอนสตรัคเตอร์
        public JumpingFiled(int width = 830, int height = 560, int startY = 515, int medianY = 270, int goalY = 0)
        {
            this.width = width;
            this.height = height;
            this.fieldBounds = new Rectangle(0, 0, width, height);
            this.startZone = new Rectangle(0, startY, width, height - startY);
            this.medianZone = new Rectangle(0, medianY, width, 45);
            this.goalZone = new Rectangle(0, goalY, width, 40);
        }

        /// <summary>
        /// ตรวจสอบว่ากบยังอยู่ในสนามต่อสู้หรือไม่
        /// </summary>
        public bool IsWithinField(PlayerFrog frog)
        {
            return frog.X >= 0 &&
                   frog.X + frog.Width <= width &&
                   frog.Y >= 0 &&
                   frog.Y + frog.Height <= height;
        }

        /// <summary>
        /// บังคับให้ตำแหน่งของกบไม่หลุดออกจากสนามต่อสู้
        /// </summary>
        public void ClampFrogPosition(PlayerFrog frog)
        {
            if (frog.X < 0) frog.X = 0;
            if (frog.X + frog.Width > width) frog.X = width - frog.Width;
            if (frog.Y < 0) frog.Y = 0;
            if (frog.Y + frog.Height > height) frog.Y = height - frog.Height;
        }

        /// <summary>
        /// ตรวจสอบว่ากบอยู่ในโซนปลอดภัย (Start Zone หรือ Median Zone) หรือไม่
        /// </summary>
        public bool IsInSafeZone(PlayerFrog frog)
        {
            return startZone.IntersectsWith(frog.Bounds) || medianZone.IntersectsWith(frog.Bounds);
        }
    }
}
