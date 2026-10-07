using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JumfrogbyMark
{
    public partial class FormGamePlay : Form
    {
        public FormGamePlay()
        {
            InitializeComponent();
        }

        private void FormGamePlay_Load(object sender, EventArgs e)
        {

        }
        private void FormGamePlay_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
