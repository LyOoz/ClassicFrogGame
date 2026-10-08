using System;
using System.Collections.Generic;
using System.Drawing;

namespace JumfrogbyMark
{
    public class River : Zone
    {
        // คุณสมบัติคงไว้เพื่อให้เข้ากันได้กับโค้ดเดิม
        public Rectangle RiverBounds => bounds;

        // คอนสตรัคเตอร์
        public River(int x = 0, int y = 40, int width = 830, int height = 230)
            : base(x, y, width, height)
        {
        }

        /// <summary>
        /// ตรวจสอบว่ากบอยู่ในอาณาเขตของแม่น้ำหรือไม่ (ใช้ ContainsFrog จากคลาสแม่)
        /// </summary>
        public bool IsFrogInRiver(PlayerFrog frog)
        {
            return ContainsFrog(frog);
        }

        /// <summary>
        /// ตรวจสอบว่ากบตกน้ำจมน้ำหรือไม่
        /// (กบอยู่ในแม่น้ำและไม่ได้อยู่บนหลังเพื่อนตัวใดเลย)
        /// </summary>
        /// <param name="frog">กบผู้เล่น</param>
        /// <param name="friends">รายการสัตว์ที่เป็นเพื่อนในแม่น้ำ</param>
        /// <returns>true หากกบตกน้ำและจมน้ำ</returns>
        public bool CheckFrogDrowned(PlayerFrog frog, List<FriendMonster> friends)
        {
            if (!IsFrogInRiver(frog))
                return false;

            // ตรวจสอบว่าอยู่บนหลังเพื่อนหรือไม่
            foreach (var friend in friends)
            {
                if (friend.IsFrogOnTop(frog))
                {
                    // ปลอดภัย อยู่บนหลังเพื่อน
                    return false;
                }
            }

            // ถ้าอยู่ในแม่น้ำและไม่มีเพื่อนรองรับ -> ตกน้ำจมน้ำตาย
            return true;
        }
    }
}
