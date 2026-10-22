using System;
using System.Windows.Forms;

namespace ClassicFrogJump
{
    public partial class FormStartGame : Form
    {
        private string playerName = "ClassicFrog001";

        public FormStartGame(string playerName = "ClassicFrog001")
        {
            InitializeComponent();
            this.playerName = playerName;
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

        private void btnHighestScore_Click(object sender, EventArgs e)
        {
            using (var formHighScore = new ClassicFrogJump.Forms.FormHighScore())
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
