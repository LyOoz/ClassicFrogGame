using System;
using System.Windows.Forms;

namespace JumfrogbyMark.Forms
{
    public class FormHighScore : Form
    {
        private Button btnBack;
        private Label lblNameNo1;
        private Label lblNameNo2;
        private Label lblNameNo3;
        private Label lblNameNo4;
        private Label lblNameNo5;
        private Label lblScoreNo1;
        private Label lblScoreNo2;
        private Label lblScoreNo3;
        private Label lblScoreNo4;
        private Label lblScoreNo5;

        public FormHighScore()
        {
            InitializeComponent();
            btnBack.Click += btnBack_Click;
            Load += FormHighScore_Load;
        }

        private void FormHighScore_Load(object sender, EventArgs e)
        {
            Label[] nameLabels = { lblNameNo1, lblNameNo2, lblNameNo3, lblNameNo4, lblNameNo5 };
            Label[] scoreLabels = { lblScoreNo1, lblScoreNo2, lblScoreNo3, lblScoreNo4, lblScoreNo5 };
            var scores = HighScoreManager.Load();

            for (int i = 0; i < 5; i++)
            {
                if (i < scores.Count)
                {
                    nameLabels[i].Text = scores[i].PlayerName;
                    scoreLabels[i].Text = scores[i].Score.ToString();
                }
                else
                {
                    nameLabels[i].Text = "-";
                    scoreLabels[i].Text = "-";
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormHighScore));
            this.btnBack = new System.Windows.Forms.Button();
            this.lblNameNo1 = new System.Windows.Forms.Label();
            this.lblNameNo2 = new System.Windows.Forms.Label();
            this.lblNameNo3 = new System.Windows.Forms.Label();
            this.lblNameNo4 = new System.Windows.Forms.Label();
            this.lblNameNo5 = new System.Windows.Forms.Label();
            this.lblScoreNo1 = new System.Windows.Forms.Label();
            this.lblScoreNo2 = new System.Windows.Forms.Label();
            this.lblScoreNo3 = new System.Windows.Forms.Label();
            this.lblScoreNo4 = new System.Windows.Forms.Label();
            this.lblScoreNo5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.Transparent;
            this.btnBack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBack.BackgroundImage")));
            this.btnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Location = new System.Drawing.Point(374, 520);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(265, 85);
            this.btnBack.TabIndex = 5;
            this.btnBack.UseVisualStyleBackColor = false;
            // 
            // lblNameNo1
            // 
            this.lblNameNo1.AutoSize = true;
            this.lblNameNo1.BackColor = System.Drawing.Color.Transparent;
            this.lblNameNo1.Font = new System.Drawing.Font("Ravie", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNameNo1.ForeColor = System.Drawing.Color.Khaki;
            this.lblNameNo1.Location = new System.Drawing.Point(312, 219);
            this.lblNameNo1.Name = "lblNameNo1";
            this.lblNameNo1.Size = new System.Drawing.Size(196, 26);
            this.lblNameNo1.TabIndex = 8;
            this.lblNameNo1.Text = "ClassicFrog001";
            // 
            // lblNameNo2
            // 
            this.lblNameNo2.AutoSize = true;
            this.lblNameNo2.BackColor = System.Drawing.Color.Transparent;
            this.lblNameNo2.Font = new System.Drawing.Font("Ravie", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNameNo2.ForeColor = System.Drawing.Color.Khaki;
            this.lblNameNo2.Location = new System.Drawing.Point(312, 278);
            this.lblNameNo2.Name = "lblNameNo2";
            this.lblNameNo2.Size = new System.Drawing.Size(196, 26);
            this.lblNameNo2.TabIndex = 9;
            this.lblNameNo2.Text = "ClassicFrog001";
            // 
            // lblNameNo3
            // 
            this.lblNameNo3.AutoSize = true;
            this.lblNameNo3.BackColor = System.Drawing.Color.Transparent;
            this.lblNameNo3.Font = new System.Drawing.Font("Ravie", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNameNo3.ForeColor = System.Drawing.Color.Khaki;
            this.lblNameNo3.Location = new System.Drawing.Point(312, 342);
            this.lblNameNo3.Name = "lblNameNo3";
            this.lblNameNo3.Size = new System.Drawing.Size(196, 26);
            this.lblNameNo3.TabIndex = 10;
            this.lblNameNo3.Text = "ClassicFrog001";
            // 
            // lblNameNo4
            // 
            this.lblNameNo4.AutoSize = true;
            this.lblNameNo4.BackColor = System.Drawing.Color.Transparent;
            this.lblNameNo4.Font = new System.Drawing.Font("Ravie", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNameNo4.ForeColor = System.Drawing.Color.Khaki;
            this.lblNameNo4.Location = new System.Drawing.Point(312, 403);
            this.lblNameNo4.Name = "lblNameNo4";
            this.lblNameNo4.Size = new System.Drawing.Size(196, 26);
            this.lblNameNo4.TabIndex = 11;
            this.lblNameNo4.Text = "ClassicFrog001";
            // 
            // lblNameNo5
            // 
            this.lblNameNo5.AutoSize = true;
            this.lblNameNo5.BackColor = System.Drawing.Color.Transparent;
            this.lblNameNo5.Font = new System.Drawing.Font("Ravie", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNameNo5.ForeColor = System.Drawing.Color.Khaki;
            this.lblNameNo5.Location = new System.Drawing.Point(312, 466);
            this.lblNameNo5.Name = "lblNameNo5";
            this.lblNameNo5.Size = new System.Drawing.Size(196, 26);
            this.lblNameNo5.TabIndex = 12;
            this.lblNameNo5.Text = "ClassicFrog001";
            // 
            // lblScoreNo1
            // 
            this.lblScoreNo1.AutoSize = true;
            this.lblScoreNo1.BackColor = System.Drawing.Color.Transparent;
            this.lblScoreNo1.Font = new System.Drawing.Font("Ravie", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScoreNo1.ForeColor = System.Drawing.Color.Khaki;
            this.lblScoreNo1.Location = new System.Drawing.Point(693, 224);
            this.lblScoreNo1.Name = "lblScoreNo1";
            this.lblScoreNo1.Size = new System.Drawing.Size(58, 21);
            this.lblScoreNo1.TabIndex = 13;
            this.lblScoreNo1.Text = "xxxx";
            // 
            // lblScoreNo2
            // 
            this.lblScoreNo2.AutoSize = true;
            this.lblScoreNo2.BackColor = System.Drawing.Color.Transparent;
            this.lblScoreNo2.Font = new System.Drawing.Font("Ravie", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScoreNo2.ForeColor = System.Drawing.Color.Khaki;
            this.lblScoreNo2.Location = new System.Drawing.Point(693, 283);
            this.lblScoreNo2.Name = "lblScoreNo2";
            this.lblScoreNo2.Size = new System.Drawing.Size(58, 21);
            this.lblScoreNo2.TabIndex = 14;
            this.lblScoreNo2.Text = "xxxx";
            // 
            // lblScoreNo3
            // 
            this.lblScoreNo3.AutoSize = true;
            this.lblScoreNo3.BackColor = System.Drawing.Color.Transparent;
            this.lblScoreNo3.Font = new System.Drawing.Font("Ravie", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScoreNo3.ForeColor = System.Drawing.Color.Khaki;
            this.lblScoreNo3.Location = new System.Drawing.Point(693, 346);
            this.lblScoreNo3.Name = "lblScoreNo3";
            this.lblScoreNo3.Size = new System.Drawing.Size(58, 21);
            this.lblScoreNo3.TabIndex = 15;
            this.lblScoreNo3.Text = "xxxx";
            // 
            // lblScoreNo4
            // 
            this.lblScoreNo4.AutoSize = true;
            this.lblScoreNo4.BackColor = System.Drawing.Color.Transparent;
            this.lblScoreNo4.Font = new System.Drawing.Font("Ravie", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScoreNo4.ForeColor = System.Drawing.Color.Khaki;
            this.lblScoreNo4.Location = new System.Drawing.Point(693, 408);
            this.lblScoreNo4.Name = "lblScoreNo4";
            this.lblScoreNo4.Size = new System.Drawing.Size(58, 21);
            this.lblScoreNo4.TabIndex = 16;
            this.lblScoreNo4.Text = "xxxx";
            // 
            // lblScoreNo5
            // 
            this.lblScoreNo5.AutoSize = true;
            this.lblScoreNo5.BackColor = System.Drawing.Color.Transparent;
            this.lblScoreNo5.Font = new System.Drawing.Font("Ravie", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScoreNo5.ForeColor = System.Drawing.Color.Khaki;
            this.lblScoreNo5.Location = new System.Drawing.Point(693, 470);
            this.lblScoreNo5.Name = "lblScoreNo5";
            this.lblScoreNo5.Size = new System.Drawing.Size(58, 21);
            this.lblScoreNo5.TabIndex = 17;
            this.lblScoreNo5.Text = "xxxx";
            // 
            // FormHighScore
            // 
            this.BackgroundImage = global::JumfrogbyMark.Properties.Resources.bg_score;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.Controls.Add(this.lblScoreNo5);
            this.Controls.Add(this.lblScoreNo4);
            this.Controls.Add(this.lblScoreNo3);
            this.Controls.Add(this.lblScoreNo2);
            this.Controls.Add(this.lblScoreNo1);
            this.Controls.Add(this.lblNameNo5);
            this.Controls.Add(this.lblNameNo4);
            this.Controls.Add(this.lblNameNo3);
            this.Controls.Add(this.lblNameNo2);
            this.Controls.Add(this.lblNameNo1);
            this.Controls.Add(this.btnBack);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FormHighScore";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HigherScore";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
