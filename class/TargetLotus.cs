using System;
using System.Drawing;

namespace JumfrogbyMark
{
    public class TargetLotus : GameObject
    {
        // field
        private bool isOccupied;
        private int scoreValue;
        private Image sprite;

        // properties
        public bool IsOccupied { get => isOccupied; set => isOccupied = value; }
        public int ScoreValue { get => scoreValue; set => scoreValue = value; }
        public Image Sprite { get => sprite; set => sprite = value; }

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
        /// ตรวจสอบว่ากบกระโดดมาถึง
        public bool CheckReached(PlayerFrog frog)
        {
            return IntersectsWith(frog);
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
    }
}
