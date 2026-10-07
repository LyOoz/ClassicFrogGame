namespace JumfrogbyMark
{
    partial class FormGamePlay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGamePlay));
            this.picWater = new System.Windows.Forms.PictureBox();
            this.picRoad = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picWater)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRoad)).BeginInit();
            this.SuspendLayout();
            // 
            // picWater
            // 
            this.picWater.BackColor = System.Drawing.Color.Transparent;
            this.picWater.BackgroundImage = global::JumfrogbyMark.Properties.Resources.water;
            this.picWater.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picWater.Location = new System.Drawing.Point(1, 12);
            this.picWater.Name = "picWater";
            this.picWater.Size = new System.Drawing.Size(1009, 253);
            this.picWater.TabIndex = 0;
            this.picWater.TabStop = false;
            // 
            // picRoad
            // 
            this.picRoad.BackColor = System.Drawing.Color.Transparent;
            this.picRoad.BackgroundImage = global::JumfrogbyMark.Properties.Resources.road;
            this.picRoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picRoad.Location = new System.Drawing.Point(1, 314);
            this.picRoad.Name = "picRoad";
            this.picRoad.Size = new System.Drawing.Size(1009, 201);
            this.picRoad.TabIndex = 1;
            this.picRoad.TabStop = false;
            // 
            // FormGamePlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::JumfrogbyMark.Properties.Resources.bg_GamePlay;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1008, 561);
            this.Controls.Add(this.picRoad);
            this.Controls.Add(this.picWater);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FormGamePlay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ClassicFrog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGamePlay_FormClosing);
            this.Load += new System.EventHandler(this.FormGamePlay_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picWater)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRoad)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picWater;
        private System.Windows.Forms.PictureBox picRoad;
    }
}