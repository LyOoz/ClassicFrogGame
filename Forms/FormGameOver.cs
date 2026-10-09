using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JumfrogbyMark.Forms
{
    public partial class FormGameOver : Form
    {
        private readonly string playerName;

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
            this.Hide();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            FormStartGame formStartGame = new FormStartGame();
            formStartGame.Show();
            this.Hide();
        }
        private void FormGameOver_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

    }
}
