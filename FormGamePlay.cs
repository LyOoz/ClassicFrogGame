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
        public FormGamePlay()
        {
            InitializeComponent();
        }

        private void FormGamePlay_Load(object sender, EventArgs e)
        {
            controller = new GameController();
            // default position frog
            picFrog.Location = new Point(controller.PlayerFrog.X, controller.PlayerFrog.Y);
            picFrog.BringToFront();
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

            // กลับรูปท่ายืนตามปุ่ม
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
            Application.Exit();
        }

        private void picFrog_Click(object sender, EventArgs e)
        {
        }
    }
}
