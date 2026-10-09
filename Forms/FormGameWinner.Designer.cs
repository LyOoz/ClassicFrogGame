namespace JumfrogbyMark.Forms
{
    partial class FormGameWinner
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGameWinner));
            this.lblName = new System.Windows.Forms.Label();
            this.lblScore = new System.Windows.Forms.Label();
            this.btnPlayAgain = new System.Windows.Forms.Button();
            this.btnMenu = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Font = new System.Drawing.Font("Ravie", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.Color.Khaki;
            this.lblName.Location = new System.Drawing.Point(547, 243);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(196, 26);
            this.lblName.TabIndex = 16;
            this.lblName.Text = "ClassicFrog001";
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.BackColor = System.Drawing.Color.Transparent;
            this.lblScore.Font = new System.Drawing.Font("Ravie", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScore.ForeColor = System.Drawing.Color.Khaki;
            this.lblScore.Location = new System.Drawing.Point(557, 336);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(147, 26);
            this.lblScore.TabIndex = 17;
            this.lblScore.Text = "xxxxxxxxx";
            // 
            // btnPlayAgain
            // 
            this.btnPlayAgain.BackColor = System.Drawing.Color.Transparent;
            this.btnPlayAgain.BackgroundImage = global::JumfrogbyMark.Properties.Resources.btn_playagain;
            this.btnPlayAgain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPlayAgain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPlayAgain.FlatAppearance.BorderSize = 0;
            this.btnPlayAgain.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPlayAgain.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPlayAgain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlayAgain.Location = new System.Drawing.Point(302, 447);
            this.btnPlayAgain.Name = "btnPlayAgain";
            this.btnPlayAgain.Size = new System.Drawing.Size(175, 63);
            this.btnPlayAgain.TabIndex = 18;
            this.btnPlayAgain.UseVisualStyleBackColor = false;
            // 
            // btnMenu
            // 
            this.btnMenu.BackColor = System.Drawing.Color.Transparent;
            this.btnMenu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMenu.BackgroundImage")));
            this.btnMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMenu.FlatAppearance.BorderSize = 0;
            this.btnMenu.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnMenu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMenu.Location = new System.Drawing.Point(538, 447);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(175, 63);
            this.btnMenu.TabIndex = 19;
            this.btnMenu.UseVisualStyleBackColor = false;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // FormGameWinner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::JumfrogbyMark.Properties.Resources.bg_winner_name;
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.Controls.Add(this.btnMenu);
            this.Controls.Add(this.btnPlayAgain);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.lblName);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FormGameWinner";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ClassicFrog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGameWinner_FormClosing);
            this.Load += new System.EventHandler(this.FormGameWinner_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Button btnPlayAgain;
        private System.Windows.Forms.Button btnMenu;
    }
}