using System;
using System.Drawing;

namespace JumfrogbyMark
{
    public class Road : Zone
    {
        // ฟิลด์เฉพาะของ Road
        private int lanesCount;

        // คุณสมบัติ (Properties)
        public Rectangle RoadBounds => bounds;
        public int LanesCount => lanesCount;

        // คอนสตรัคเตอร์
        public Road(int x = 0, int y = 315, int width = 830, int height = 200, int lanesCount = 4)
            : base(x, y, width, height)
        {
            this.lanesCount = lanesCount;
        }

        /// ตรวจว่ากบอยู่บนถนนมั้ย
        public bool IsFrogOnRoad(PlayerFrog frog)
        {
            return ContainsFrog(frog);
        }

        /// ตรวจว่ากบกระโดดข้ามถนนไปยังอีกฝั่ง
        public bool HasCrossed(PlayerFrog frog)
        {
            return frog.Y + frog.Height <= y;
        }
    }
}
