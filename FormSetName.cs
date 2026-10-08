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
    public partial class FormSetName : Form
    {
        public string PlayerName { get; private set; } = "ClassicFrog001";
        public FormSetName(string currentName = "ClassicFrog001")
        {
            InitializeComponent();
            this.PlayerName = currentName;
            txtName.Text = currentName;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtName.Text))
            {
                PlayerName = txtName.Text.Trim();
            }
            else
            {
                PlayerName = "ClassicFrog001";
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
