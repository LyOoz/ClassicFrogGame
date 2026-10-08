using System;
using System.Drawing;

namespace JumfrogbyMark
{
    public abstract class Monster // base class monster
    {
        // field
        protected float x;
        protected float y;
        protected int width;
        protected int height;
        protected float baseSpeed;
        protected float speed;
        protected bool movingRight;
        protected int minX;
        protected int maxX;

        // properties
        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public float Speed { get => speed; set => speed = value; }
        public bool MovingRight { get => movingRight; set => movingRight = value; }
        public Image Sprite { get; set; }

        public Rectangle Bounds => new Rectangle((int)x, (int)y, width, height);

        // constructor
        public Monster(float x, float y, int width, int height, float baseSpeed, bool movingRight, int minX = -100, int maxX = 900)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.baseSpeed = baseSpeed;
            this.speed = baseSpeed;
            this.movingRight = movingRight;
            this.minX = minX;
            this.maxX = maxX;
        }
        public virtual void Update()
        {
            if (movingRight)
            {
                x += speed;
                if (x > maxX)
                    x = minX;
            }
            else
            {
                x -= speed;
                if (x + width < minX)
                    x = maxX;
            }
        }
    }
}
