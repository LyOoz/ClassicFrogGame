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
        public FormNextLevel()
        {
            InitializeComponent();
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
