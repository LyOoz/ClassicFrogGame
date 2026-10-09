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
    public partial class FormNextLevel : Form
    {
        private readonly string playerName;
        private readonly int score;
        private readonly int nextLevel;

        public FormNextLevel(string playerName = "ClassicFrog001", int score = 0, int nextLevel = 2)
        {
            InitializeComponent();
            this.playerName = playerName;
            this.score = score;
            this.nextLevel = nextLevel;
            lblName.Text = playerName;
            lblScore.Text = score.ToString();
            btnNextLevel.Click += btnNextLevel_Click;
        }

        private void btnNextLevel_Click(object sender, EventArgs e)
        {
            FormGamePlay formGamePlay = new FormGamePlay(playerName, nextLevel, score);
            formGamePlay.Show();
            this.Hide();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            FormStartGame formStartGame = new FormStartGame();
            formStartGame.Show();
            this.Hide();
        }
        private void FormNextLevel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
