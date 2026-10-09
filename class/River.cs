using System;
using System.Drawing;

namespace JumfrogbyMark
{
    public class River : Zone
    {
        // Constructor
        public River(int x = 0, int y = 40, int width = 830, int height = 230)
            : base(x, y, width, height)
        {
        }

        /// ตรวจสอบกบอยู่ในแม่น้ำ
        public bool IsFrogInRiver(PlayerFrog frog)
        {
            return ContainsFrog(frog);
        }
    }
}
