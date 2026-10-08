using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JumfrogbyMark
{
    public partial class FormGamePlay : Form
    {
        private GameController controller;
        private string playerName;
        private System.Windows.Forms.Timer moveTimer;
        private Image frogSprite;

        // debug Location (X, Y) on frog
        private bool showDebug = false;
        private readonly Font debugFont = new Font("Consolas", 8f);

        public FormGamePlay(string playerName = "ClassicFrog001")
        {
            InitializeComponent();
            this.playerName = playerName;
            this.frogSprite = SpriteConfig.FrogUp;
        }

        private void FormGamePlay_Load(object sender, EventArgs e)
        {
            controller = new GameController();
            lblName.Text = playerName;

            // gameloop timer
            moveTimer = new System.Windows.Forms.Timer();
            moveTimer.Interval = 30;
            moveTimer.Tick += MoveTimer_Tick;
            moveTimer.Start();
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            // ให GameController.Update() เปนผูรันตรรกะเกมทั้งหมด (timer, monster, ชน, ตกน้ำ, ขี่เพื่อน, lotus)
            float deltaTime = moveTimer.Interval / 1000f;
            controller.Update(deltaTime);

            // อัปเดต HUD
            lblTime.Text = TimeSpan.FromSeconds(Math.Max(0, controller.GameTimer.TimeRemaining)).ToString(@"mm\:ss");
            lblScore.Text = controller.Score.ToString();
            lblLevel.Text = controller.Level + "/3";
            UpdateHeartsDisplay();

            // วาด sprite ลงทุกเฟรม
            Invalidate();

            // เกมจบ -> หยุด timer (รอทำหน้า Game Over แยกภายหลัง)
            if (controller.IsGameOver)
            {
                moveTimer.Stop();
            }
        }

        // อัปเดตรูปหัวใจตามจำนวนหัวใจที่เหลือของกบ
        private void UpdateHeartsDisplay()
        {
            int hearts = controller.PlayerFrog.Hearts;
            PictureBox[] heartBoxes = { pigHeart1, picHeart2, picHeart3, picHeart4, picHeart5 };
            for (int i = 0; i < heartBoxes.Length; i++)
            {
                heartBoxes[i].BackgroundImage = i < hearts
                    ? Properties.Resources.heart_full
                    : Properties.Resources.heart_empty;
            }
        }
        // วาด texture sprite 
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (controller == null) return;
            var g = e.Graphics;

            foreach (var m in controller.Friends)
                if (m.Sprite != null) g.DrawImage(m.Sprite, (int)m.X, (int)m.Y, m.Width, m.Height);
            foreach (var m in controller.Enemies)
                if (m.Sprite != null) g.DrawImage(m.Sprite, (int)m.X, (int)m.Y, m.Width, m.Height);

            // กบอยู่บนสุดเพื่อทับ monster ได้
            var frog = controller.PlayerFrog;
            g.DrawImage(frogSprite, frog.X, frog.Y, frog.Width, frog.Height);

            // debug
            if (showDebug)
            {
                DrawDebugLocation(g, "Frog", frog.X, frog.Y);
                foreach (var m in controller.Friends)
                    DrawDebugLocation(g, "F", frog.X, frog.Y);
                foreach (var m in controller.Enemies)
                    DrawDebugLocation(g, "E", frog.X, frog.Y);
            }
        }

        // debug
        private void DrawDebugLocation(Graphics g, string tag, int x, int y)
        {
            string text = string.Format("{0} {1},{2}", tag, x, y);
            var size = g.MeasureString(text, debugFont);
            float tx = x;
            float ty = y - size.Height - 2;
            if (ty < 0) ty = y + 2; 

            g.FillRectangle(Brushes.Black, tx, ty, size.Width + 4, size.Height + 2);
            g.DrawString(text, debugFont, Brushes.Lime, tx + 2, ty + 1);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // F1 debug Location
            if (keyData == Keys.F1)
            {
                showDebug = !showDebug;
                Invalidate();
                return true;
            }

            switch (keyData) // ตรวจปุ่มแลว้สั่่งให้ method jump ทำงาน
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    Jump(keyData);
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool isJumping = false;
        private async void Jump(Keys direction)
        {
            if (isJumping) return;
            isJumping = true;
            Soundplayer.PlayJumpSound();
            // เปลี่่ยนรูปตามปุม่เดิิน
            switch (direction)
            {
                case Keys.Up:
                    frogSprite = SpriteConfig.FrogJump;
                    controller.PlayerFrog.MoveUp();
                    break;
                case Keys.Down:
                    frogSprite = SpriteConfig.FrogJumpDown;
                    controller.PlayerFrog.MoveDown();
                    break;
                case Keys.Left:
                    frogSprite = SpriteConfig.FrogJumpLeft;
                    controller.PlayerFrog.MoveLeft();
                    break;
                case Keys.Right:
                    frogSprite = SpriteConfig.FrogJumpRight;
                    controller.PlayerFrog.MoveRight();
                    break;
            }
            Invalidate();
            await Task.Delay(100);
            // รูปท่ายืนตามปุ่ม
            switch (direction)
            {
                case Keys.Up: frogSprite = SpriteConfig.FrogUp; break;
                case Keys.Down: frogSprite = SpriteConfig.FrogDown; break;
                case Keys.Left: frogSprite = SpriteConfig.Frog.Left; break;
                case Keys.Right: frogSprite = SpriteConfig.Frog.Right; break;
            }
            Invalidate();

            isJumping = false;
        }
        private void FormGamePlay_FormClosing(object sender, FormClosingEventArgs e)
        {
            moveTimer?.Stop();
            Application.Exit();
        }
        private void btnSound_Click(object sender, EventArgs e)
        {
            bool isMuted = Soundplayer.ToggleMute();
            if (isMuted)
            {
                btnSound.BackgroundImage = Properties.Resources.sound_off;
            }
            else
            {
                btnSound.BackgroundImage = Properties.Resources.sound_on;
            }
        }
    }
}
