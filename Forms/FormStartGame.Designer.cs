namespace JumfrogbyMark
{
    partial class FormStartGame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStartGame));
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnSound = new System.Windows.Forms.PictureBox();
            this.btnSetName = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.btnHighestScore = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.btnSound)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.BackColor = System.Drawing.Color.Transparent;
            this.btnPlay.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPlay.BackgroundImage")));
            this.btnPlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPlay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPlay.FlatAppearance.BorderSize = 0;
            this.btnPlay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPlay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.Location = new System.Drawing.Point(371, 339);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(273, 98);
            this.btnPlay.TabIndex = 0;
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnSound
            // 
            this.btnSound.BackColor = System.Drawing.Color.Transparent;
            this.btnSound.BackgroundImage = global::JumfrogbyMark.Properties.Resources.sound_on;
            this.btnSound.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSound.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSound.Location = new System.Drawing.Point(668, 414);
            this.btnSound.Name = "btnSound";
            this.btnSound.Size = new System.Drawing.Size(68, 65);
            this.btnSound.TabIndex = 1;
            this.btnSound.TabStop = false;
            this.btnSound.Click += new System.EventHandler(this.btnSound_Click);
            // 
            // btnSetName
            // 
            this.btnSetName.BackColor = System.Drawing.Color.Transparent;
            this.btnSetName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSetName.BackgroundImage")));
            this.btnSetName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSetName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSetName.FlatAppearance.BorderSize = 0;
            this.btnSetName.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSetName.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSetName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetName.Location = new System.Drawing.Point(371, 447);
            this.btnSetName.Name = "btnSetName";
            this.btnSetName.Size = new System.Drawing.Size(273, 98);
            this.btnSetName.TabIndex = 2;
            this.btnSetName.UseVisualStyleBackColor = false;
            this.btnSetName.Click += new System.EventHandler(this.btnSetName_Click);
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.BackColor = System.Drawing.Color.Transparent;
            this.lblName.Font = new System.Drawing.Font("Ravie", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.ForeColor = System.Drawing.Color.Khaki;
            this.lblName.Location = new System.Drawing.Point(448, 244);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(196, 26);
            this.lblName.TabIndex = 7;
            this.lblName.Text = "ClassicFrog001";
            this.lblName.Click += new System.EventHandler(this.lblName_Click);
            // 
            // btnHighestScore
            // 
            this.btnHighestScore.BackColor = System.Drawing.Color.Transparent;
            this.btnHighestScore.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHighestScore.BackgroundImage")));
            this.btnHighestScore.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnHighestScore.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHighestScore.FlatAppearance.BorderSize = 0;
            this.btnHighestScore.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHighestScore.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHighestScore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHighestScore.Location = new System.Drawing.Point(22, 12);
            this.btnHighestScore.Name = "btnHighestScore";
            this.btnHighestScore.Size = new System.Drawing.Size(212, 119);
            this.btnHighestScore.TabIndex = 8;
            this.btnHighestScore.UseVisualStyleBackColor = false;
            this.btnHighestScore.Click += new System.EventHandler(this.btnHighestScore_Click);
            // 
            // FormStartGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(1008, 561);
            this.Controls.Add(this.btnHighestScore);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnSetName);
            this.Controls.Add(this.btnSound);
            this.Controls.Add(this.btnPlay);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FormStartGame";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ClassicFrog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormStartGame_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnSound)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.PictureBox btnSound;
        private System.Windows.Forms.Button btnSetName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnHighestScore;
    }
}

