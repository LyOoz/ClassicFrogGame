using System;
using System.Drawing;

namespace JumfrogbyMark
{
    /// <summary>
    /// คลาส Road เป็นคลาสซึ่งเป็นตัวแทนของถนน (สืบทอดจาก Zone)
    /// ซึ่งกบจะต้องกระโดดข้ามถนนไปอีกฝั่ง
    /// </summary>
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

        /// <summary>
        /// ตรวจสอบว่ากบอยู่บนถนนหรือไม่ (ใช้ ContainsFrog จากคลาสแม่)
        /// </summary>
        public bool IsFrogOnRoad(PlayerFrog frog)
        {
            return ContainsFrog(frog);
        }

        /// <summary>
        /// ตรวจสอบว่ากบกระโดดข้ามถนนไปยังอีกฝั่ง (ฝั่งบนของถนน) ได้สำเร็จหรือไม่
        /// </summary>
        public bool HasCrossed(PlayerFrog frog)
        {
            return frog.Y + frog.Height <= y;
        }
    }
}
