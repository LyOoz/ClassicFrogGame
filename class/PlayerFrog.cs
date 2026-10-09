using System;
using System.Drawing;

namespace JumfrogbyMark
{
    // control frog 
    public enum FrogDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public class PlayerFrog
    {
        // field
        private int x;
        private int y;
        private int width;
        private int height;
        private int hearts;
        private int initialHearts;
        private int startX;
        private int startY;
        private int stepSize;

        // properties
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public int Hearts { get => hearts; set => hearts = value; }
        public int InitialHearts => initialHearts;

        public Rectangle Bounds => new Rectangle(x, y, width, height);

        // Constructor frog
        public PlayerFrog(int startX = 396, int startY = 490, int width = 50, int height = 50, int initialHearts = 5, int stepSize = 55)
        {
            this.startX = startX;
            this.startY = startY;
            this.x = startX;
            this.y = startY;
            this.width = width;
            this.height = height;
            this.initialHearts = initialHearts;
            this.hearts = initialHearts;
            this.stepSize = stepSize;
        }
        // method move frog
        public void MoveUp(int minY = 40) // min ขอบจอแกน y
        {
            if (y - stepSize >= minY)
                y -= stepSize;
            else
                y = minY;
        }

        public void MoveDown(int maxY = 660) // max ขอบจอแกน y
        {
            if (y + stepSize + height <= maxY)
                y += stepSize;
            else
                y = maxY - height;
        }

        public void MoveLeft(int minX = -10) // min ขอบจอแกน x
        {
            if (x - stepSize >= minX)
                x -= stepSize;
            else
                x = minX;
        }

        public void MoveRight(int maxX = 1000) // max ขอบจอแกน x
        {
            if (x + stepSize + width <= maxX)
                x += stepSize;
            else
                x = maxX - width;
        }

        /// frog take damage -1 heart if heart 0 == game over
        public bool TakeDamage()
        {
            if (hearts > 0)
            {
                hearts--;
            }

            if (hearts <= 0)
            {
                hearts = 0;
                return true; // Game Over
            }

            ResetToStart();
            return false;
        }

        // reset frog position to start position
        public void ResetToStart()
        {
            x = startX;
            y = startY;
        }

        /// reset all frog properties
        public void ResetAll()
        {
            hearts = initialHearts;
            ResetToStart();
        }

    }
}
