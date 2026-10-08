using System;
using System.Drawing;

namespace JumfrogbyMark
{
    public enum ItemType
    {
        BonusScore,  // เพิ่มคะแนนพิเศษ
        ExtraHeart   // เพิ่มหัวใจพิเศษ
    }

    /// <summary>
    /// คลาส Item เป็นคลาสซึ่งเป็นตัวแทนของไอเทมพิเศษ
    /// ซึ่งจะเกิดในตำแหน่งสุ่มกลางถนน กบจะต้องกระโดดข้ามถนนไปเก็บไอเทมเหล่านี้
    /// โดยจะปรากฏอยู่นาน 10 วินาที
    /// </summary>
    public class Item
    {
        // ฟิลด์
        private int x;
        private int y;
        private int width;
        private int height;
        private bool isActive;
        private bool isCollected;
        private float duration;          // เวลาที่คงอยู่ (10 วินาที)
        private float remainingTime;     // เวลาที่เหลืออยู่
        private ItemType type;
        private int scoreBonus;

        // คุณสมบัติ (Properties)
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public bool IsActive => isActive;
        public bool IsCollected => isCollected;
        public float RemainingTime => remainingTime;
        public float Duration => duration;
        public ItemType Type => type;
        public int ScoreBonus => scoreBonus;

        public Rectangle Bounds => new Rectangle(x, y, width, height);

        // คอนสตรัคเตอร์
        public Item(int width = 30, int height = 30, float durationSeconds = 10.0f)
        {
            this.width = width;
            this.height = height;
            this.duration = durationSeconds;
            this.remainingTime = durationSeconds;
            this.isActive = false;
            this.isCollected = false;
            this.type = ItemType.BonusScore;
            this.scoreBonus = 300;
        }

        /// <summary>
        /// สุ่มเกิดไอเทมพิเศษกลางถนน ปรากฏอยู่นาน 10 วินาที
        /// </summary>
        /// <param name="roadArea">พื้นที่ของถนน</param>
        /// <param name="rng">ตัวสร้างเลขสุ่ม</param>
        public void SpawnRandom(Rectangle roadArea, Random rng)
        {
            int minX = roadArea.Left + 50;
            int maxX = roadArea.Right - width - 50;
            int minY = roadArea.Top + 20;
            int maxY = roadArea.Bottom - height - 20;

            this.x = rng.Next(minX, Math.Max(minX + 1, maxX));
            this.y = rng.Next(minY, Math.Max(minY + 1, maxY));

            // สุ่มประเภทไอเทม (โอกาสได้หัวใจ 20%, คะแนน 80%)
            this.type = rng.NextDouble() < 0.20 ? ItemType.ExtraHeart : ItemType.BonusScore;
            this.scoreBonus = type == ItemType.ExtraHeart ? 100 : 300;

            this.remainingTime = duration; // 10 วินาที
            this.isActive = true;
            this.isCollected = false;
        }

        /// <summary>
        /// อัปเดตเวลาถอยหลัง 10 วินาที
        /// </summary>
        /// <param name="deltaTime">เวลาที่ผ่านไปในเฟรมนี้ (วินาที)</param>
        public void Update(float deltaTime)
        {
            if (!isActive || isCollected) return;

            remainingTime -= deltaTime;
            if (remainingTime <= 0)
            {
                remainingTime = 0;
                isActive = false; // หมดเวลา 10 วินาที ไอเทมหายไป
            }
        }

        /// <summary>
        /// ตรวจสอบการเก็บไอเทมโดยกบ
        /// </summary>
        /// <param name="frog">กบผู้เล่น</param>
        /// <returns>true หากกบเก็บไอเทมได้</returns>
        public bool CheckCollect(PlayerFrog frog)
        {
            if (!isActive || isCollected) return false;

            if (this.Bounds.IntersectsWith(frog.Bounds))
            {
                isCollected = true;
                isActive = false;

                // ผลของไอเทม
                if (type == ItemType.ExtraHeart && frog.Hearts < 5)
                {
                    frog.Hearts++;
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// รีเซ็ตสถานะไอเทม
        /// </summary>
        public void Reset()
        {
            isActive = false;
            isCollected = false;
            remainingTime = duration;
        }
    }
}
