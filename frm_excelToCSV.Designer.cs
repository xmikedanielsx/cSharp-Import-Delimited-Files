
namespace BulkImportDelimitedFlatFiles
{
    partial class frm_excelToCSV
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_excelToCSV));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtbox_excelFolder = new System.Windows.Forms.TextBox();
            this.txtbox_csvFolder = new System.Windows.Forms.TextBox();
            this.btn_excelPickUp = new System.Windows.Forms.Button();
            this.btn_csvDropOff = new System.Windows.Forms.Button();
            this.btn_convertExceltoCSV = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_fileLoadStatus = new System.Windows.Forms.Label();
            this.cmbox_delimiter = new System.Windows.Forms.ComboBox();
            this.lbl_selectedDelimiter = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Excel File Folder";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(15, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "CSV Drop Off Folder";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtbox_excelFolder
            // 
            this.txtbox_excelFolder.Location = new System.Drawing.Point(156, 99);
            this.txtbox_excelFolder.Name = "txtbox_excelFolder";
            this.txtbox_excelFolder.Size = new System.Drawing.Size(574, 23);
            this.txtbox_excelFolder.TabIndex = 2;
            // 
            // txtbox_csvFolder
            // 
            this.txtbox_csvFolder.Location = new System.Drawing.Point(156, 133);
            this.txtbox_csvFolder.Name = "txtbox_csvFolder";
            this.txtbox_csvFolder.Size = new System.Drawing.Size(574, 23);
            this.txtbox_csvFolder.TabIndex = 3;
            // 
            // btn_excelPickUp
            // 
            this.btn_excelPickUp.Location = new System.Drawing.Point(736, 99);
            this.btn_excelPickUp.Name = "btn_excelPickUp";
            this.btn_excelPickUp.Size = new System.Drawing.Size(34, 23);
            this.btn_excelPickUp.TabIndex = 4;
            this.btn_excelPickUp.Text = "...";
            this.btn_excelPickUp.UseVisualStyleBackColor = true;
            this.btn_excelPickUp.Click += new System.EventHandler(this.btn_excelPickUp_Click);
            // 
            // btn_csvDropOff
            // 
            this.btn_csvDropOff.Location = new System.Drawing.Point(736, 133);
            this.btn_csvDropOff.Name = "btn_csvDropOff";
            this.btn_csvDropOff.Size = new System.Drawing.Size(34, 23);
            this.btn_csvDropOff.TabIndex = 5;
            this.btn_csvDropOff.Text = "...";
            this.btn_csvDropOff.UseVisualStyleBackColor = true;
            this.btn_csvDropOff.Click += new System.EventHandler(this.btn_csvDropOff_Click);
            // 
            // btn_convertExceltoCSV
            // 
            this.btn_convertExceltoCSV.Location = new System.Drawing.Point(703, 181);
            this.btn_convertExceltoCSV.Name = "btn_convertExceltoCSV";
            this.btn_convertExceltoCSV.Size = new System.Drawing.Size(73, 26);
            this.btn_convertExceltoCSV.TabIndex = 6;
            this.btn_convertExceltoCSV.Text = "Convert";
            this.btn_convertExceltoCSV.UseVisualStyleBackColor = true;
            this.btn_convertExceltoCSV.Click += new System.EventHandler(this.btn_convertExceltoCSV_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(22, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(745, 87);
            this.label3.TabIndex = 7;
            this.label3.Text = resources.GetString("label3.Text");
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // lbl_fileLoadStatus
            // 
            this.lbl_fileLoadStatus.Location = new System.Drawing.Point(156, 159);
            this.lbl_fileLoadStatus.Name = "lbl_fileLoadStatus";
            this.lbl_fileLoadStatus.Size = new System.Drawing.Size(620, 23);
            this.lbl_fileLoadStatus.TabIndex = 8;
            this.lbl_fileLoadStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbox_delimiter
            // 
            this.cmbox_delimiter.FormattingEnabled = true;
            this.cmbox_delimiter.Items.AddRange(new object[] {
            "Tab",
            "Comma",
            "Semi-Colon",
            "Pipe"});
            this.cmbox_delimiter.Location = new System.Drawing.Point(586, 185);
            this.cmbox_delimiter.Name = "cmbox_delimiter";
            this.cmbox_delimiter.Size = new System.Drawing.Size(97, 23);
            this.cmbox_delimiter.TabIndex = 38;
            // 
            // lbl_selectedDelimiter
            // 
            this.lbl_selectedDelimiter.Location = new System.Drawing.Point(445, 185);
            this.lbl_selectedDelimiter.Name = "lbl_selectedDelimiter";
            this.lbl_selectedDelimiter.Size = new System.Drawing.Size(135, 23);
            this.lbl_selectedDelimiter.TabIndex = 39;
            this.lbl_selectedDelimiter.Text = "Delimiter";
            this.lbl_selectedDelimiter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frm_excelToCSV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 219);
            this.Controls.Add(this.lbl_selectedDelimiter);
            this.Controls.Add(this.cmbox_delimiter);
            this.Controls.Add(this.lbl_fileLoadStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_convertExceltoCSV);
            this.Controls.Add(this.btn_csvDropOff);
            this.Controls.Add(this.btn_excelPickUp);
            this.Controls.Add(this.txtbox_csvFolder);
            this.Controls.Add(this.txtbox_excelFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_excelToCSV";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Excel to CSV (Tab Delimited)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtbox_excelFolder;
        private System.Windows.Forms.TextBox txtbox_csvFolder;
        private System.Windows.Forms.Button btn_excelPickUp;
        private System.Windows.Forms.Button btn_csvDropOff;
        private System.Windows.Forms.Button btn_convertExceltoCSV;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_fileLoadStatus;
        private System.Windows.Forms.ComboBox cmbox_delimiter;
        private System.Windows.Forms.Label lbl_selectedDelimiter;
    }
}