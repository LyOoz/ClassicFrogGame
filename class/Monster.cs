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
        protected float speed;
        protected bool movingRight;
        protected int minX;
        protected int maxX;

        // animation เดิน เฟรมแยก
        protected Image[] leftFrames;
        protected Image[] rightFrames;
        protected int frameIndex;
        protected float animTimer;
        protected const float AnimInterval = 0.15f;

        // properties
        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }
        public Image Sprite { get; set; }

        public Rectangle Bounds => new Rectangle((int)x, (int)y, width, height);

        // ตั้งเฟรม animation เดิน
        public void SetWalkFrames(Image[] left, Image[] right)
        {
            this.leftFrames = left;
            this.rightFrames = right;
            this.frameIndex = 0;
            this.animTimer = 0f;
        }

        // เฟรมปัจจุบันตามทิศทางการเคลื่่อนไหว
        public Image CurrentFrame
        {
            get
            {
                Image[] frames = movingRight ? rightFrames : leftFrames;
                if (frames == null || frames.Length == 0) return Sprite;
                return frames[frameIndex % frames.Length];
            }
        }

        // constructor
        public Monster(float x, float y, int width, int height, float baseSpeed, bool movingRight, int minX = -100, int maxX = 900)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.speed = baseSpeed;
            this.movingRight = movingRight;
            this.minX = minX;
            this.maxX = maxX;
        }
        public virtual void Update(float deltaTime)
        {
            // สลับเฟรม animation เดิน
            if (leftFrames != null || rightFrames != null)
            {
                animTimer += deltaTime;
                if (animTimer >= AnimInterval)
                {
                    animTimer -= AnimInterval;
                    frameIndex++;
                }
            }

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
