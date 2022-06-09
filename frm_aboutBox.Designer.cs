namespace BulkImportDelimitedFlatFiles
{
    partial class frm_aboutBox
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
            this.components = new System.ComponentModel.Container();
            this.timer_splash = new System.Windows.Forms.Timer(this.components);
            this.picBox_backgroundImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBox_backgroundImage)).BeginInit();
            this.SuspendLayout();
            // 
            // timer_splash
            // 
            this.timer_splash.Enabled = true;
            this.timer_splash.Interval = 2000;
            this.timer_splash.Tick += new System.EventHandler(this.timer_splash_Tick);
            // 
            // picBox_backgroundImage
            // 
            this.picBox_backgroundImage.Location = new System.Drawing.Point(0, 0);
            this.picBox_backgroundImage.Name = "picBox_backgroundImage";
            this.picBox_backgroundImage.Size = new System.Drawing.Size(606, 398);
            this.picBox_backgroundImage.TabIndex = 0;
            this.picBox_backgroundImage.TabStop = false;
            this.picBox_backgroundImage.Click += new System.EventHandler(this.picBox_backgroundImage_Click);
            // 
            // frm_aboutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 398);
            this.Controls.Add(this.picBox_backgroundImage);
            this.Name = "frm_aboutBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.Load += new System.EventHandler(this.frm_aboutBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBox_backgroundImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer_splash;
        private System.Windows.Forms.PictureBox picBox_backgroundImage;
    }
}