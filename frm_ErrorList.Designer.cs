namespace BulkImportDelimitedFlatFiles
{
    partial class frm_ErrorList
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
            this.lv_errorList = new System.Windows.Forms.ListView();
            this.txt_error = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lv_errorList
            // 
            this.lv_errorList.HideSelection = false;
            this.lv_errorList.Location = new System.Drawing.Point(12, 12);
            this.lv_errorList.Name = "lv_errorList";
            this.lv_errorList.Size = new System.Drawing.Size(313, 318);
            this.lv_errorList.TabIndex = 0;
            this.lv_errorList.UseCompatibleStateImageBehavior = false;
            this.lv_errorList.View = System.Windows.Forms.View.List;
            this.lv_errorList.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lv_errorList_ItemSelectionChanged);
            this.lv_errorList.SelectedIndexChanged += new System.EventHandler(this.lv_errorList_SelectedIndexChanged);
            // 
            // txt_error
            // 
            this.txt_error.Enabled = false;
            this.txt_error.Location = new System.Drawing.Point(331, 12);
            this.txt_error.Multiline = true;
            this.txt_error.Name = "txt_error";
            this.txt_error.Size = new System.Drawing.Size(627, 318);
            this.txt_error.TabIndex = 1;
            // 
            // frm_ErrorList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(970, 342);
            this.Controls.Add(this.txt_error);
            this.Controls.Add(this.lv_errorList);
            this.Name = "frm_ErrorList";
            this.Text = "Errors List";
            this.Shown += new System.EventHandler(this.frm_ErrorList_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ListView lv_errorList;
        private System.Windows.Forms.TextBox txt_error;
    }
}