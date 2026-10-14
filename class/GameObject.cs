using System.Drawing;

namespace ClassicFrogJump
{
    public abstract class GameObject
    {
        protected float x;
        protected float y;
        protected int width;
        protected int height;

        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }

        public virtual Rectangle Bounds => new Rectangle((int)x, (int)y, width, height);

        public bool IntersectsWith(GameObject other)
        {
            return other != null && Bounds.IntersectsWith(other.Bounds);
        }
    }
}
