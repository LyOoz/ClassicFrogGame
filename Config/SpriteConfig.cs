using System.Drawing;

namespace JumfrogbyMark
{
    public static class SpriteConfig
    {
        public static readonly SpriteSet Frog = new SpriteSet(
            Properties.Resources.frog_left, Properties.Resources.frog_right, width: 60, height: 50, spacing: 40);
        public static readonly Image FrogUp = Properties.Resources.frog_up;
        public static readonly Image FrogDown = Properties.Resources.frog_down;
        public static readonly Image FrogJump = Properties.Resources.frog_jump;
        public static readonly Image FrogJumpDown = Properties.Resources.frog_jumpback;
        public static readonly Image FrogJumpLeft = Properties.Resources.frog_jumpleft;
        public static readonly Image FrogJumpRight = Properties.Resources.frog_jumpright;


        public static readonly SpriteSet Turtle = new SpriteSet(
            Properties.Resources.turtle_left, Properties.Resources.turtle_right, width: 85, height: 45, spacing: 320);
        public static readonly SpriteSet Crocodile = new SpriteSet(
            Properties.Resources.crocodile_left, Properties.Resources.crocodile_right, width: 100, height: 50, spacing: 360);

        public static readonly Image[] TurtleLeftFrames =
        {
            Properties.Resources.turtle_left_walk_0,
            Properties.Resources.turtle_left_walk_1,
            Properties.Resources.turtle_left_walk_2,
            Properties.Resources.turtle_left_walk_3,
        };
        public static readonly Image[] TurtleRightFrames =
        {
            Properties.Resources.turtle_right_walk_0,
            Properties.Resources.turtle_right_walk_1,
            Properties.Resources.turtle_right_walk_2,
            Properties.Resources.turtle_right_walk_3,
        };
        public static readonly Image[] CrocodileRightFrames =
        {
            Properties.Resources.crocodile_right_walk_0,
            Properties.Resources.crocodile_right_walk_1,
            Properties.Resources.crocodile_right_walk_2,
            Properties.Resources.crocodile_right_walk_3,
        };

        public static readonly SpriteSet FishBlue = new SpriteSet(
            Properties.Resources.fish_blue_left, Properties.Resources.fish_blue_right, width: 90, height: 36, spacing: 270);
        public static readonly SpriteSet FishRed = new SpriteSet(
            Properties.Resources.fish_red_left, Properties.Resources.fish_red_right, width: 90, height: 36, spacing: 270);

        public const int LotusWidth = 85;
        public const int LotusHeight = 45;
        public static readonly Image Lotus = Properties.Resources.lilypad_flower;

        public const int ItemWidth = 32;
        public const int ItemHeight = 32;
    }
    public class SpriteSet
    {
        public Image Left { get; }
        public Image Right { get; }
        public int Width { get; }
        public int Height { get; }
        public int Spacing { get; }

        public SpriteSet(Image left, Image right, int width, int height, int spacing)
        {
            this.Left = left;
            this.Right = right;
            this.Width = width;
            this.Height = height;
            this.Spacing = spacing;
        }
        public Image Get(bool movingRight) => movingRight ? Right : Left;
    }
}
