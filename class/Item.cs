using System;
using System.Drawing;

namespace JumfrogbyMark
{
    public enum ItemType
    {
        ExtraHeart,
        ExtraTime
    }

    public class Item
    {
        // field
        private int x;
        private int y;
        private int width;
        private int height;
        private bool isActive;
        private float duration;
        private float remainingTime;
        private ItemType type;
        private int heartBonus;
        private float timeBonusSeconds;

        // Properties
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public bool IsActive => isActive;
        public ItemType Type => type;
        public int HeartBonus => heartBonus;
        public float TimeBonusSeconds => timeBonusSeconds;

        public Rectangle Bounds => new Rectangle(x, y, width, height);

        // constructor
        public Item(int width = 30, int height = 30, float durationSeconds = 10.0f)
        {
            this.width = width;
            this.height = height;
            this.duration = durationSeconds;
            this.remainingTime = durationSeconds;
            this.isActive = false;
            this.type = ItemType.ExtraTime;
            this.heartBonus = 1;
            this.timeBonusSeconds = 10.0f;
        }

        /// สุ่มเกิดไอเทมพิเศษกลางถนน
        public void SpawnRandom(Rectangle roadArea, Random rng, float extraHeartChance, int heartBonus, float timeBonusSeconds)
        {
            int minX = roadArea.Left + 50;
            int maxX = roadArea.Right - width - 50;
            int minY = roadArea.Top + 20;
            int maxY = roadArea.Bottom - height - 20;

            this.x = rng.Next(minX, Math.Max(minX + 1, maxX));
            this.y = rng.Next(minY, Math.Max(minY + 1, maxY));

            float heartChance = Math.Max(0.0f, Math.Min(1.0f, extraHeartChance));
            this.type = rng.NextDouble() < heartChance ? ItemType.ExtraHeart : ItemType.ExtraTime;
            this.heartBonus = Math.Max(1, heartBonus);
            this.timeBonusSeconds = Math.Max(0.0f, timeBonusSeconds);

            this.remainingTime = duration; // 10 วินาที
            this.isActive = true;
        }

        /// อัปเดตเวลาถอยหลัง 10 วินาที
        public void Update(float deltaTime)
        {
            if (!isActive) return;

            remainingTime -= deltaTime;
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                isActive = false; // หมดเวลา 10 วินาที ไอเทมหายไป
            }
        }

        /// ตรวจสอบการเก็บไอเทมโดยกบ
        public bool CheckCollect(PlayerFrog frog)
        {
            if (!isActive) return false;

            if (this.Bounds.IntersectsWith(frog.Bounds))
            {
                isActive = false;
                return true;
            }
            return false;
        }

        /// รีเซ็ตสถานะไอเทม
        public void Reset()
        {
            isActive = false;
            remainingTime = duration;
        }
    }
}
