namespace BulkImportDelimitedFlatFiles
{
    partial class frm_infoBox
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
            this.lbl_message = new System.Windows.Forms.Label();
            this.btn_closeMe = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_message
            // 
            this.lbl_message.Location = new System.Drawing.Point(12, 9);
            this.lbl_message.Name = "lbl_message";
            this.lbl_message.Size = new System.Drawing.Size(455, 126);
            this.lbl_message.TabIndex = 0;
            this.lbl_message.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btn_closeMe
            // 
            this.btn_closeMe.Location = new System.Drawing.Point(194, 148);
            this.btn_closeMe.Name = "btn_closeMe";
            this.btn_closeMe.Size = new System.Drawing.Size(75, 23);
            this.btn_closeMe.TabIndex = 1;
            this.btn_closeMe.Text = "Close";
            this.btn_closeMe.UseVisualStyleBackColor = true;
            this.btn_closeMe.Click += new System.EventHandler(this.btn_closeMe_Click);
            // 
            // frm_infoBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 183);
            this.Controls.Add(this.btn_closeMe);
            this.Controls.Add(this.lbl_message);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_infoBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_message;
        private System.Windows.Forms.Button btn_closeMe;
    }
}