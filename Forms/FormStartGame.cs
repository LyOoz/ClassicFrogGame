using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JumfrogbyMark
{
    public partial class FormStartGame : Form
    {
        private string playerName = "ClassicFrog001";
        public FormStartGame()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Soundplayer.PlayStartMusic();
            UpdateSoundButton();
            lblName.Text = playerName;
        }
        private void btnPlay_Click(object sender, EventArgs e)
        {
            FormGamePlay gameForm = new FormGamePlay(playerName);
            Soundplayer.StopMusic();
            gameForm.Show();
            this.Hide();
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
        private void btnSetName_Click(object sender, EventArgs e)
        {
            using (FormSetName formSet = new FormSetName(playerName))
            {
                if (formSet.ShowDialog() == DialogResult.OK)
                {
                    playerName = formSet.PlayerName;
                    lblName.Text = playerName; 
                }
            }
        }
        private void lblName_Click(object sender, EventArgs e)
        {

        }

        private void btnHighestScore_Click(object sender, EventArgs e)
        {
            using (var formHighScore = new JumfrogbyMark.Forms.FormHighScore())
            {
                formHighScore.ShowDialog(this);
            }
        }
        private void FormStartGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

    }
}
