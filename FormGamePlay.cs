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

        public FormGamePlay(string playerName = "ClassicFrog001")
        {
            InitializeComponent();
            this.playerName = playerName;
            this.frogSprite = Properties.Resources.frog_up;
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
            // GameTimer
            float deltaTime = moveTimer.Interval / 1000f;
            controller.GameTimer.Update(deltaTime, controller.PlayerFrog);
            lblTime.Text = TimeSpan.FromSeconds(Math.Max(0, controller.GameTimer.TimeRemaining)).ToString(@"mm\:ss");
            lblScore.Text = controller.Score.ToString();
            lblLevel.Text = controller.Level + "/3";

            foreach (var enemy in controller.Enemies)
                enemy.Update();
            foreach (var friend in controller.Friends)
                friend.Update();

            // วาด sprite ลงทุกเฟรม
            Invalidate();
        }

        // วาด background + sprite ทุกตัวในเกม
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
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
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
                    frogSprite = Properties.Resources.frog_jump;
                    controller.PlayerFrog.MoveUp();
                    break;
                case Keys.Down:
                    frogSprite = Properties.Resources.frog_jump;
                    controller.PlayerFrog.MoveDown();
                    break;
                case Keys.Left:
                    frogSprite = Properties.Resources.frog_jump;
                    controller.PlayerFrog.MoveLeft();
                    break;
                case Keys.Right:
                    frogSprite = Properties.Resources.frog_jump;
                    controller.PlayerFrog.MoveRight();
                    break;
            }
            Invalidate();
            // delay รอ animation จบ
            await Task.Delay(100);
            // รูปท่ายืนตามปุม่
            switch (direction)
            {
                case Keys.Up: frogSprite = Properties.Resources.frog_up; break;
                case Keys.Down: frogSprite = Properties.Resources.frog_down; break;
                case Keys.Left: frogSprite = Properties.Resources.frog_left; break;
                case Keys.Right: frogSprite = Properties.Resources.frog_right; break;
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
