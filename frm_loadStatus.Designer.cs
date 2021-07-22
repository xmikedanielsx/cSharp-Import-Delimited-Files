
namespace BulkImportDelimitedFlatFiles
{
    partial class frm_loadStatus
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
            if (disposing && (components != null)) {
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
            this.pgrbar_loadingTables = new System.Windows.Forms.ProgressBar();
            this.lbl_loadingTableStatus = new System.Windows.Forms.Label();
            this.btn_closeDialog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pgrbar_loadingTables
            // 
            this.pgrbar_loadingTables.Location = new System.Drawing.Point(12, 12);
            this.pgrbar_loadingTables.Name = "pgrbar_loadingTables";
            this.pgrbar_loadingTables.Size = new System.Drawing.Size(459, 23);
            this.pgrbar_loadingTables.TabIndex = 0;
            // 
            // lbl_loadingTableStatus
            // 
            this.lbl_loadingTableStatus.Location = new System.Drawing.Point(12, 38);
            this.lbl_loadingTableStatus.Name = "lbl_loadingTableStatus";
            this.lbl_loadingTableStatus.Size = new System.Drawing.Size(459, 49);
            this.lbl_loadingTableStatus.TabIndex = 1;
            this.lbl_loadingTableStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_closeDialog
            // 
            this.btn_closeDialog.Location = new System.Drawing.Point(178, 90);
            this.btn_closeDialog.Name = "btn_closeDialog";
            this.btn_closeDialog.Size = new System.Drawing.Size(114, 27);
            this.btn_closeDialog.TabIndex = 2;
            this.btn_closeDialog.Text = "OK";
            this.btn_closeDialog.UseVisualStyleBackColor = true;
            this.btn_closeDialog.Click += new System.EventHandler(this.btn_closeDialog_Click);
            // 
            // frm_loadStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 127);
            this.Controls.Add(this.btn_closeDialog);
            this.Controls.Add(this.lbl_loadingTableStatus);
            this.Controls.Add(this.pgrbar_loadingTables);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_loadStatus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Loading your tables";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar pgrbar_loadingTables;
        private System.Windows.Forms.Label lbl_loadingTableStatus;
        private System.Windows.Forms.Button btn_closeDialog;
    }
}