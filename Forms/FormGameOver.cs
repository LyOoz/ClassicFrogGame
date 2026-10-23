using System;
using System.Windows.Forms;

namespace ClassicFrogJump.Forms
{
    public partial class FormGameOver : Form
    {
        private readonly string playerName;
        private bool closingForTransition;

        public FormGameOver(string playerName = "ClassicFrog001", int score = 0)
        {
            InitializeComponent();
            this.playerName = playerName;
            lblName.Text = playerName;
            lblScore.Text = score.ToString();
        }
        private void FormGameOver_Load(object sender, EventArgs e)
        {
            Soundplayer.PlayGameOver();
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            FormGamePlay formGamePlay = new FormGamePlay(playerName);
            formGamePlay.Show();
            closingForTransition = true;
            this.Close();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            FormStartGame formStartGame = new FormStartGame(playerName);
            formStartGame.Show();
            closingForTransition = true;
            this.Close();
        }
        private void FormGameOver_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closingForTransition)
                Application.Exit(); 
        }

    }
}
