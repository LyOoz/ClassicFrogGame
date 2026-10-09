using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using JumfrogbyMark.Forms;

namespace JumfrogbyMark
{
    public partial class FormGamePlay : Form
    {
        private GameController controller;
        private string playerName;
        private int startLevel;
        private int startScore;
        private System.Windows.Forms.Timer moveTimer;
        private Image frogSprite;
        private Button btnPauseContinue;
        private Button btnPauseMenu;
        private Button btnPauseRestart;
        private bool scoreSaved;

        // debug Location (X, Y) , godmode
        private bool showDebug = false;
        private readonly Font debugFont = new Font("Consolas", 8f);
        private readonly Font godModeFont = new Font("Consolas", 14f, FontStyle.Bold);

        public FormGamePlay(string playerName = "ClassicFrog001", int startLevel = 1, int startScore = 0)
        {
            InitializeComponent();
            this.playerName = playerName;
            this.startLevel = startLevel;
            this.startScore = startScore;
            this.frogSprite = SpriteConfig.FrogUp;
            InitializePauseOverlay();
        }

        private void FormGamePlay_Load(object sender, EventArgs e)
        {
            Soundplayer.PlayGameplayMusic();
            UpdateSoundButton();
            controller = new GameController(startLevel: startLevel, startScore: startScore);
            lblName.Text = playerName;

            // gameloop timer
            moveTimer = new System.Windows.Forms.Timer();
            moveTimer.Interval = 30;
            moveTimer.Tick += MoveTimer_Tick;
            moveTimer.Start();
        }

        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            float deltaTime = moveTimer.Interval / 1000f;
            controller.Update(deltaTime);

            // HUD STATUS
            lblTime.Text = TimeSpan.FromSeconds(Math.Max(0, controller.GameTimer.TimeRemaining)).ToString(@"mm\:ss");
            lblScore.Text = controller.Score.ToString();
            lblLevel.Text = controller.Level + "/3";
            UpdateHeartsDisplay();

            // วาด sprite ลงทุกเฟรม
            Invalidate();

            // เกมจบหรือผ่านด่าน
            if (controller.IsGameOver)
            {
                ShowGameOver();
            }
            else if (controller.IsLevelComplete)
            {
                ShowNextLevel();
            }
            else if (controller.IsVictory)
            {
                ShowGameWinner();
            }
        }
        private void ShowGameOver()
        {
            moveTimer.Stop();
            FormGameOver formGameOver = new FormGameOver(playerName, controller.Score);
            Soundplayer.StopMusic();
            formGameOver.Show();
            this.Hide();
        }
        private void ShowNextLevel()
        {
            moveTimer.Stop();
            FormNextLevel formNextLevel = new FormNextLevel(playerName, controller.Score, controller.NextLevel);
            Soundplayer.StopMusic();
            formNextLevel.Show();
            this.Hide();
        }
        private void ShowGameWinner()
        {
            moveTimer.Stop();
            FormGameWinner formGameWinner = new FormGameWinner(playerName, controller.Score);
            Soundplayer.StopMusic();
            formGameWinner.Show();
            this.Hide();
        }
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
        // drawsprite
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (controller == null) return;
            var g = e.Graphics;

            // lotus
            foreach (var lotus in controller.TargetLotuses)
                if (!lotus.IsOccupied && lotus.Sprite != null)
                    g.DrawImage(lotus.Sprite, lotus.X, lotus.Y, lotus.Width, lotus.Height);
            // frog friends and enemies
            foreach (var m in controller.Friends)
                if (m.CurrentFrame != null) g.DrawImage(m.CurrentFrame, (int)m.X, (int)m.Y, m.Width, m.Height);
            foreach (var m in controller.Enemies)
                if (m.CurrentFrame != null) g.DrawImage(m.CurrentFrame, (int)m.X, (int)m.Y, m.Width, m.Height);
            // tresure box
            var item = controller.CurrentItem;
            if (item != null && item.IsActive && SpriteConfig.ItemChest != null)
                g.DrawImage(SpriteConfig.ItemChest, item.X, item.Y, item.Width, item.Height);
            // frox
            var frog = controller.PlayerFrog;
            g.DrawImage(frogSprite, frog.X, frog.Y, frog.Width, frog.Height);

            // debug
            if (showDebug)
            {
                DrawDebugLocation(g, "Frog", frog.X, frog.Y);
                foreach (var lotus in controller.TargetLotuses)
                    DrawDebugLocation(g, "L", lotus.X, lotus.Y);
            }

            if (controller.IsGodMode)
            {
                DrawGodModeStatus(g);
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

        private void DrawGodModeStatus(Graphics g)
        {
            const string text = "GODMODE";
            var size = g.MeasureString(text, godModeFont);
            g.FillRectangle(Brushes.Black, 12, 12, size.Width + 12, size.Height + 8);
            g.DrawString(text, godModeFont, Brushes.Gold, 18, 16);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape && controller != null && !controller.IsGameOver && !controller.IsLevelComplete && !controller.IsVictory)
            {
                TogglePause();
                return true;
            }

            // F1 debug Location
            if (keyData == Keys.F1)
            {
                showDebug = !showDebug;
                Invalidate();
                return true;
            }

            // F2 debug God Mode
            if (keyData == Keys.F2 && controller != null)
            {
                controller.IsGodMode = !controller.IsGodMode;
                Invalidate();
                return true;
            }

            switch (keyData) // ตรวจปุ่มสั่่งให้ method jump ทำงาน
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    if (controller != null && !controller.IsGameOver && !controller.IsPaused)
                    {
                        Jump(keyData);
                    }
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
                    controller.MovePlayer(FrogDirection.Up);
                    break;
                case Keys.Down:
                    frogSprite = SpriteConfig.FrogJumpDown;
                    controller.MovePlayer(FrogDirection.Down);
                    break;
                case Keys.Left:
                    frogSprite = SpriteConfig.FrogJumpLeft;
                    controller.MovePlayer(FrogDirection.Left);
                    break;
                case Keys.Right:
                    frogSprite = SpriteConfig.FrogJumpRight;
                    controller.MovePlayer(FrogDirection.Right);
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
            SaveCurrentScoreIfAny();
            moveTimer?.Stop();
            Application.Exit();
        }
        private void btnSound_Click(object sender, EventArgs e)
        {
            Soundplayer.ToggleMute();
            UpdateSoundButton();
        }
        private void UpdateSoundButton()
        {
            btnSound.BackgroundImage = Soundplayer.IsMuted
                ? Properties.Resources.sound_off
                : Properties.Resources.sound_on;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            TogglePause();
        }

        private void InitializePauseOverlay()
        {
            int buttonWidth = 260;
            int buttonHeight = 66;
            int buttonGap = 19;
            int buttonX = (ClientSize.Width - buttonWidth) / 2;
            int buttonY = (ClientSize.Height - ((buttonHeight * 3) + (buttonGap * 2))) / 2;

            btnPauseContinue = CreatePauseButton(Properties.Resources.btn_continue, buttonX, buttonY, buttonWidth, buttonHeight);
            btnPauseMenu = CreatePauseButton(Properties.Resources.btn_menu, buttonX, buttonY + buttonHeight + buttonGap, buttonWidth, buttonHeight);
            btnPauseRestart = CreatePauseButton(Properties.Resources.btn_restart, buttonX, buttonY + ((buttonHeight + buttonGap) * 2), buttonWidth, buttonHeight);

            btnPauseContinue.Click += btnPauseContinue_Click;
            btnPauseMenu.Click += btnPauseMenu_Click;
            btnPauseRestart.Click += btnPauseRestart_Click;

            Controls.Add(btnPauseContinue);
            Controls.Add(btnPauseMenu);
            Controls.Add(btnPauseRestart);
            SetPauseOverlayVisible(false);
        }

        private Button CreatePauseButton(Image image, int x, int y, int width, int height)
        {
            var button = new Button
            {
                BackColor = Color.Transparent,
                BackgroundImage = image,
                BackgroundImageLayout = ImageLayout.Stretch,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(x, y),
                Size = new Size(width, height),
                UseVisualStyleBackColor = false,
                Visible = false
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button.FlatAppearance.MouseOverBackColor = Color.Transparent;
            return button;
        }

        private void TogglePause()
        {
            if (controller == null || controller.IsGameOver || controller.IsLevelComplete || controller.IsVictory) return;

            controller.TogglePause();
            SetPauseOverlayVisible(controller.IsPaused);
            Invalidate();
        }

        private void SetPauseOverlayVisible(bool visible)
        {
            if (btnPauseContinue == null) return;

            btnPauseContinue.Visible = visible;
            btnPauseMenu.Visible = visible;
            btnPauseRestart.Visible = visible;

            if (visible)
            {
                btnPauseContinue.BringToFront();
                btnPauseMenu.BringToFront();
                btnPauseRestart.BringToFront();
            }
        }

        private void btnPauseContinue_Click(object sender, EventArgs e)
        {
            if (controller != null && controller.IsPaused)
                TogglePause();
        }

        private void btnPauseMenu_Click(object sender, EventArgs e)
        {
            SaveCurrentScoreIfAny();
            moveTimer?.Stop();
            Soundplayer.StopMusic();
            FormStartGame formStartGame = new FormStartGame();
            formStartGame.Show();
            this.Hide();
        }

        private void btnPauseRestart_Click(object sender, EventArgs e)
        {
            SaveCurrentScoreIfAny();
            controller?.RestartGame();
            scoreSaved = false;
            frogSprite = SpriteConfig.FrogUp;
            isJumping = false;
            SetPauseOverlayVisible(false);
            Invalidate();

        }

        private void SaveCurrentScoreIfAny()
        {
            if (scoreSaved || controller == null || controller.Score <= 0) return;

            HighScoreManager.SaveScore(playerName, controller.Score);
            scoreSaved = true;
        }
    }
}
