using System;
using System.Drawing;

namespace JumfrogbyMark
{
    public class TargetLotus // class baibua
    {
        // field
        private int x;
        private int y;
        private int width;
        private int height;
        private bool isOccupied;
        private int scoreValue;

        // properties
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
        public int ScoreValue { get => scoreValue; set => scoreValue = value; }

        public Rectangle Bounds => new Rectangle(x, y, width, height);

        // constuctor
        public TargetLotus(int x, int y, int width = 50, int height = 40, int scoreValue = 500)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.isOccupied = false;
            this.scoreValue = scoreValue;
        }
        // Method
        /// ตรวจสอบว่ากบกระโดดมาถึงใบบัวนี้หรือไม่
        public bool CheckReached(PlayerFrog frog)
        {
            // ตรวจสอบว่าขอบเขตของกบตัดกับใบบัว
            return this.Bounds.IntersectsWith(frog.Bounds);
        }


        /// เมื่อกบกระโดดมาถึงใบบัว
        public bool Occupy()
        {
            if (!isOccupied)
            {
                isOccupied = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// รีเซ็ตสถานะใบบัว
        /// </summary>
        public void Reset()
        {
            isOccupied = false;
        }
    }
}
