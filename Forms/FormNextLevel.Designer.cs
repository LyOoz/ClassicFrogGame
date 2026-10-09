namespace JumfrogbyMark.Forms
{
    partial class FormNextLevel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNextLevel));
            this.lblName = new System.Windows.Forms.Label();
            this.lblScore = new System.Windows.Forms.Label();
            this.btnNextLevel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Font = new System.Drawing.Font("Ravie", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.Color.Khaki;
            this.lblName.Location = new System.Drawing.Point(552, 245);
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
            this.lblScore.Location = new System.Drawing.Point(552, 333);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(147, 26);
            this.lblScore.TabIndex = 17;
            this.lblScore.Text = "xxxxxxxxx";
            // 
            // btnNextLevel
            // 
            this.btnNextLevel.BackColor = System.Drawing.Color.Transparent;
            this.btnNextLevel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNextLevel.BackgroundImage")));
            this.btnNextLevel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNextLevel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNextLevel.FlatAppearance.BorderSize = 0;
            this.btnNextLevel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNextLevel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNextLevel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextLevel.Location = new System.Drawing.Point(438, 448);
            this.btnNextLevel.Name = "btnNextLevel";
            this.btnNextLevel.Size = new System.Drawing.Size(175, 63);
            this.btnNextLevel.TabIndex = 18;
            this.btnNextLevel.UseVisualStyleBackColor = false;
            // 
            // FormNextLevel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::JumfrogbyMark.Properties.Resources.bg_winner_name;
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.Controls.Add(this.btnNextLevel);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.lblName);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FormNextLevel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ClassicForg";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormNextLevel_FormClosing);
            this.Load += new System.EventHandler(this.FormNextLevel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Button btnNextLevel;
    }
}