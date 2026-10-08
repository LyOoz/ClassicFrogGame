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

        public FormGamePlay(string playerName = "ClassicFrog001")
        {
            InitializeComponent();
            this.playerName = playerName;
        }

        private void FormGamePlay_Load(object sender, EventArgs e)
        {
            controller = new GameController();
            lblName.Text = playerName;
            // default position frog
            picFrog.Location = new Point(controller.PlayerFrog.X, controller.PlayerFrog.Y);
            picFrog.BringToFront();

            // gameloop timer
            moveTimer = new System.Windows.Forms.Timer();
            moveTimer.Interval = 30;
            moveTimer.Tick += MoveTimer_Tick;
            moveTimer.Start();
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            // จุดสิ้นสุดของ Monster
            foreach (var enemy in controller.Enemies)
                enemy.Update(-84, 820); 
            foreach (var friend in controller.Friends)
                friend.Update(-84, 500);
            // EnemyMonster
            // เลนแรก
            SyncMonster(picTurtleR, controller.EnemyAt(0, 0));
            // เลนสอง
            SyncMonster(picTurtleL, controller.EnemyAt(1, 0));
            // เลนสาม
            SyncMonster(picCrocR, controller.EnemyAt(2, 0));
            SyncMonster(picCrocR, controller.EnemyAt(2, 1));
            // FriendMonster
            // เลนแรก
            SyncMonster(picFishL1, controller.FriendAt(0, 0));
            // เลนสอง
            SyncMonster(picFishR, controller.FriendAt(1, 0));
            // เลนสาม
            SyncMonster(picFishL, controller.FriendAt(2, 0));

        }

        // ให้ PictureBox ขยับ X ตาม Monster (Y คงตำแหน่่งเลนท่ีวางไว้ใน designer)
        private void SyncMonster(PictureBox pic, Monster m)
        {
            pic.Location = new Point((int)m.X, pic.Location.Y);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData) // ตรวจปุ่มแล้วสั่งให้ method jump ทำงาน
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

            // เปลี่ยนรูปตามปุ่มเดิน
            switch (direction)
            {
                case Keys.Up:
                    picFrog.BackgroundImage = Properties.Resources.frog_jump; 
                    controller.PlayerFrog.MoveUp();
                    break;
                case Keys.Down:
                    picFrog.BackgroundImage = Properties.Resources.frog_jump;
                    controller.PlayerFrog.MoveDown();
                    break;
                case Keys.Left:
                    picFrog.BackgroundImage = Properties.Resources.frog_jump;
                    controller.PlayerFrog.MoveLeft();
                    break;
                case Keys.Right:
                    picFrog.BackgroundImage = Properties.Resources.frog_jump;
                    controller.PlayerFrog.MoveRight();
                    break;
            }

            UpdateFrogPosition();

            // delay รอ animation จบ
            await Task.Delay(100);

            // รูปท่ายืนตามปุ่ม
            switch (direction)
            {
                case Keys.Up: picFrog.BackgroundImage = Properties.Resources.frog_up; break;
                case Keys.Down: picFrog.BackgroundImage = Properties.Resources.frog_down; break;
                case Keys.Left: picFrog.BackgroundImage = Properties.Resources.frog_left; break;
                case Keys.Right: picFrog.BackgroundImage = Properties.Resources.frog_right; break;
            }

            isJumping = false;
        }

        private void UpdateFrogPosition()
        {
            // อัพเดทค่า x , y เมื่อกดเดิน
            picFrog.Location = new Point(controller.PlayerFrog.X, controller.PlayerFrog.Y);
            picFrog.BringToFront();
        }

        private void FormGamePlay_FormClosing(object sender, FormClosingEventArgs e)
        {
            moveTimer?.Stop();
            Application.Exit();
        }

        private void picFrog_Click(object sender, EventArgs e)
        {
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
