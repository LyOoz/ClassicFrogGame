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
    public partial class FormGameWinner : Form
    {
        private readonly string playerName;

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
            this.Hide();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            FormStartGame formStartGame = new FormStartGame();
            formStartGame.Show();
            this.Hide();
        }
        private void FormGameWinner_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

    }
}
