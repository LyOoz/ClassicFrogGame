using System;
using System.Windows.Forms;

namespace ClassicFrogJump.Forms
{
    public partial class FormGameWinner : Form
    {
        private readonly string playerName;
        // true = เปลี่่ยนไป form อื่น
        private bool closingForTransition;

        public FormGameWinner(string playerName = "ClassicFrog001", int score = 0)
        {
            InitializeComponent();
            this.playerName = playerName;
            lblName.Text = playerName;
            lblScore.Text = score.ToString();
            btnPlayAgain.Click += btnPlayAgain_Click;
        }
        private void FormGameWinner_Load(object sender, EventArgs e)
        {
            Soundplayer.PlayWinner();
        }


        private void btnPlayAgain_Click(object sender, EventArgs e)
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
        private void FormGameWinner_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closingForTransition)
                Application.Exit(); // ปิดจบ
        }

    }
}
