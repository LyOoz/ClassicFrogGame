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
        private SoundPlayer sound = new SoundPlayer();
        public FormStartGame()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sound.Play("C:\Users\mark\Downloads\JumfrogbyMark\sound\sound-startgame.mp3");
        }


        private void btnPlay_Click(object sender, EventArgs e)
        {

        }
    }
}
