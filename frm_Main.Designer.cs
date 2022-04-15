
namespace BulkImportDelimitedFlatFiles
{
    partial class frm_Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_Main));
            this.btn_testConnection = new System.Windows.Forms.Button();
            this.lbl_filePickUpPath = new System.Windows.Forms.Label();
            this.txtbox_pickUpPath = new System.Windows.Forms.TextBox();
            this.lbl_sqlDatabase = new System.Windows.Forms.Label();
            this.txtbox_sqlDatabase = new System.Windows.Forms.TextBox();
            this.lbl_sqlPass = new System.Windows.Forms.Label();
            this.txtbox_sqlPass = new System.Windows.Forms.TextBox();
            this.lbl_sqlUser = new System.Windows.Forms.Label();
            this.txtbox_sqlUser = new System.Windows.Forms.TextBox();
            this.lbl_sqlServer = new System.Windows.Forms.Label();
            this.txtbox_sqlServer = new System.Windows.Forms.TextBox();
            this.lbl_testConnStatus = new System.Windows.Forms.Label();
            this.btn_openFileDiag = new System.Windows.Forms.Button();
            this.btn_loadFilesToList = new System.Windows.Forms.Button();
            this.lbl_loadFilesStatus = new System.Windows.Forms.Label();
            this.sc_Main = new System.Windows.Forms.SplitContainer();
            this.chbox_windowsAuth = new System.Windows.Forms.CheckBox();
            this.lbl_CollapseTopPanel = new System.Windows.Forms.Label();
            this.txtbox_defaultDataLength = new System.Windows.Forms.TextBox();
            this.lbl_defaultDataLength = new System.Windows.Forms.Label();
            this.cmbox_delimiter = new System.Windows.Forms.ComboBox();
            this.lbl_delimiterLbl = new System.Windows.Forms.Label();
            this.sc_innerContainer = new System.Windows.Forms.SplitContainer();
            this.lv_fileList = new ListViewCustomReorder.ListViewEx();
            this.dgv_FieldList = new System.Windows.Forms.DataGridView();
            this.index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fieldName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dtLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbl_ExpandTopPanel = new System.Windows.Forms.Label();
            this.chbox_CreateAllTablesTable = new System.Windows.Forms.CheckBox();
            this.chbox_DropTables = new System.Windows.Forms.CheckBox();
            this.lbl_LoadToSQLStatus = new System.Windows.Forms.Label();
            this.ms_Main = new System.Windows.Forms.MenuStrip();
            this.tsm_file = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_tools = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmii_excelToCsvConverter = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmi_clearData = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.txtbox_FinalTableName = new System.Windows.Forms.TextBox();
            this.btn_loadToSQL = new System.Windows.Forms.Button();
            this.chbox_tablePrefix = new System.Windows.Forms.CheckBox();
            this.txtbox_tablePrefix = new System.Windows.Forms.TextBox();
            this.lbl_warningIncremental = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sc_Main)).BeginInit();
            this.sc_Main.Panel1.SuspendLayout();
            this.sc_Main.Panel2.SuspendLayout();
            this.sc_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sc_innerContainer)).BeginInit();
            this.sc_innerContainer.Panel1.SuspendLayout();
            this.sc_innerContainer.Panel2.SuspendLayout();
            this.sc_innerContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_FieldList)).BeginInit();
            this.ms_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_testConnection
            // 
            this.btn_testConnection.Location = new System.Drawing.Point(1068, 18);
            this.btn_testConnection.Name = "btn_testConnection";
            this.btn_testConnection.Size = new System.Drawing.Size(43, 23);
            this.btn_testConnection.TabIndex = 21;
            this.btn_testConnection.Text = "Test";
            this.btn_testConnection.UseVisualStyleBackColor = true;
            this.btn_testConnection.Click += new System.EventHandler(this.btn_testConnection_Click);
            // 
            // lbl_filePickUpPath
            // 
            this.lbl_filePickUpPath.AutoSize = true;
            this.lbl_filePickUpPath.Location = new System.Drawing.Point(11, 46);
            this.lbl_filePickUpPath.Name = "lbl_filePickUpPath";
            this.lbl_filePickUpPath.Size = new System.Drawing.Size(122, 15);
            this.lbl_filePickUpPath.TabIndex = 20;
            this.lbl_filePickUpPath.Text = "Flat File Source Folder";
            // 
            // txtbox_pickUpPath
            // 
            this.txtbox_pickUpPath.Location = new System.Drawing.Point(139, 41);
            this.txtbox_pickUpPath.Name = "txtbox_pickUpPath";
            this.txtbox_pickUpPath.Size = new System.Drawing.Size(497, 23);
            this.txtbox_pickUpPath.TabIndex = 19;
            // 
            // lbl_sqlDatabase
            // 
            this.lbl_sqlDatabase.AutoSize = true;
            this.lbl_sqlDatabase.Location = new System.Drawing.Point(755, 20);
            this.lbl_sqlDatabase.Name = "lbl_sqlDatabase";
            this.lbl_sqlDatabase.Size = new System.Drawing.Size(82, 15);
            this.lbl_sqlDatabase.TabIndex = 18;
            this.lbl_sqlDatabase.Text = "SQL Database:";
            // 
            // txtbox_sqlDatabase
            // 
            this.txtbox_sqlDatabase.Location = new System.Drawing.Point(846, 18);
            this.txtbox_sqlDatabase.Name = "txtbox_sqlDatabase";
            this.txtbox_sqlDatabase.Size = new System.Drawing.Size(180, 23);
            this.txtbox_sqlDatabase.TabIndex = 17;
            // 
            // lbl_sqlPass
            // 
            this.lbl_sqlPass.AutoSize = true;
            this.lbl_sqlPass.Location = new System.Drawing.Point(508, 20);
            this.lbl_sqlPass.Name = "lbl_sqlPass";
            this.lbl_sqlPass.Size = new System.Drawing.Size(57, 15);
            this.lbl_sqlPass.TabIndex = 16;
            this.lbl_sqlPass.Text = "SQL Pass:";
            this.lbl_sqlPass.Click += new System.EventHandler(this.lbl_sqlPass_Click);
            // 
            // txtbox_sqlPass
            // 
            this.txtbox_sqlPass.Location = new System.Drawing.Point(572, 18);
            this.txtbox_sqlPass.Name = "txtbox_sqlPass";
            this.txtbox_sqlPass.Size = new System.Drawing.Size(180, 23);
            this.txtbox_sqlPass.TabIndex = 15;
            this.txtbox_sqlPass.UseSystemPasswordChar = true;
            // 
            // lbl_sqlUser
            // 
            this.lbl_sqlUser.AutoSize = true;
            this.lbl_sqlUser.Location = new System.Drawing.Point(260, 20);
            this.lbl_sqlUser.Name = "lbl_sqlUser";
            this.lbl_sqlUser.Size = new System.Drawing.Size(57, 15);
            this.lbl_sqlUser.TabIndex = 14;
            this.lbl_sqlUser.Text = "SQL User:";
            // 
            // txtbox_sqlUser
            // 
            this.txtbox_sqlUser.Location = new System.Drawing.Point(323, 18);
            this.txtbox_sqlUser.Name = "txtbox_sqlUser";
            this.txtbox_sqlUser.Size = new System.Drawing.Size(180, 23);
            this.txtbox_sqlUser.TabIndex = 13;
            // 
            // lbl_sqlServer
            // 
            this.lbl_sqlServer.AutoSize = true;
            this.lbl_sqlServer.Location = new System.Drawing.Point(4, 24);
            this.lbl_sqlServer.Name = "lbl_sqlServer";
            this.lbl_sqlServer.Size = new System.Drawing.Size(66, 15);
            this.lbl_sqlServer.TabIndex = 12;
            this.lbl_sqlServer.Text = "SQL Server:";
            // 
            // txtbox_sqlServer
            // 
            this.txtbox_sqlServer.Location = new System.Drawing.Point(75, 18);
            this.txtbox_sqlServer.Name = "txtbox_sqlServer";
            this.txtbox_sqlServer.Size = new System.Drawing.Size(180, 23);
            this.txtbox_sqlServer.TabIndex = 11;
            // 
            // lbl_testConnStatus
            // 
            this.lbl_testConnStatus.Location = new System.Drawing.Point(260, 45);
            this.lbl_testConnStatus.Name = "lbl_testConnStatus";
            this.lbl_testConnStatus.Size = new System.Drawing.Size(800, 15);
            this.lbl_testConnStatus.TabIndex = 22;
            this.lbl_testConnStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_openFileDiag
            // 
            this.btn_openFileDiag.Location = new System.Drawing.Point(642, 40);
            this.btn_openFileDiag.Name = "btn_openFileDiag";
            this.btn_openFileDiag.Size = new System.Drawing.Size(37, 24);
            this.btn_openFileDiag.TabIndex = 23;
            this.btn_openFileDiag.Text = "...";
            this.btn_openFileDiag.UseVisualStyleBackColor = true;
            this.btn_openFileDiag.Click += new System.EventHandler(this.btn_openFileDiag_Click);
            // 
            // btn_loadFilesToList
            // 
            this.btn_loadFilesToList.Location = new System.Drawing.Point(1066, 41);
            this.btn_loadFilesToList.Name = "btn_loadFilesToList";
            this.btn_loadFilesToList.Size = new System.Drawing.Size(84, 23);
            this.btn_loadFilesToList.TabIndex = 24;
            this.btn_loadFilesToList.Text = "Load Files";
            this.btn_loadFilesToList.UseVisualStyleBackColor = true;
            this.btn_loadFilesToList.Click += new System.EventHandler(this.btn_loadFilesToList_Click);
            // 
            // lbl_loadFilesStatus
            // 
            this.lbl_loadFilesStatus.Location = new System.Drawing.Point(382, 25);
            this.lbl_loadFilesStatus.Name = "lbl_loadFilesStatus";
            this.lbl_loadFilesStatus.Size = new System.Drawing.Size(297, 13);
            this.lbl_loadFilesStatus.TabIndex = 25;
            this.lbl_loadFilesStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sc_Main
            // 
            this.sc_Main.Cursor = System.Windows.Forms.Cursors.Default;
            this.sc_Main.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.sc_Main.Location = new System.Drawing.Point(12, 27);
            this.sc_Main.Name = "sc_Main";
            this.sc_Main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // sc_Main.Panel1
            // 
            this.sc_Main.Panel1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.sc_Main.Panel1.Controls.Add(this.chbox_windowsAuth);
            this.sc_Main.Panel1.Controls.Add(this.lbl_CollapseTopPanel);
            this.sc_Main.Panel1.Controls.Add(this.txtbox_sqlDatabase);
            this.sc_Main.Panel1.Controls.Add(this.txtbox_sqlServer);
            this.sc_Main.Panel1.Controls.Add(this.lbl_sqlServer);
            this.sc_Main.Panel1.Controls.Add(this.txtbox_sqlUser);
            this.sc_Main.Panel1.Controls.Add(this.lbl_testConnStatus);
            this.sc_Main.Panel1.Controls.Add(this.lbl_sqlUser);
            this.sc_Main.Panel1.Controls.Add(this.btn_testConnection);
            this.sc_Main.Panel1.Controls.Add(this.txtbox_sqlPass);
            this.sc_Main.Panel1.Controls.Add(this.lbl_sqlPass);
            this.sc_Main.Panel1.Controls.Add(this.lbl_sqlDatabase);
            this.sc_Main.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            // 
            // sc_Main.Panel2
            // 
            this.sc_Main.Panel2.Controls.Add(this.txtbox_defaultDataLength);
            this.sc_Main.Panel2.Controls.Add(this.lbl_defaultDataLength);
            this.sc_Main.Panel2.Controls.Add(this.cmbox_delimiter);
            this.sc_Main.Panel2.Controls.Add(this.lbl_delimiterLbl);
            this.sc_Main.Panel2.Controls.Add(this.sc_innerContainer);
            this.sc_Main.Panel2.Controls.Add(this.lbl_ExpandTopPanel);
            this.sc_Main.Panel2.Controls.Add(this.btn_loadFilesToList);
            this.sc_Main.Panel2.Controls.Add(this.lbl_loadFilesStatus);
            this.sc_Main.Panel2.Controls.Add(this.txtbox_pickUpPath);
            this.sc_Main.Panel2.Controls.Add(this.lbl_filePickUpPath);
            this.sc_Main.Panel2.Controls.Add(this.btn_openFileDiag);
            this.sc_Main.Size = new System.Drawing.Size(1162, 676);
            this.sc_Main.SplitterDistance = 99;
            this.sc_Main.TabIndex = 26;
            // 
            // chbox_windowsAuth
            // 
            this.chbox_windowsAuth.AutoSize = true;
            this.chbox_windowsAuth.Location = new System.Drawing.Point(75, 44);
            this.chbox_windowsAuth.Name = "chbox_windowsAuth";
            this.chbox_windowsAuth.Size = new System.Drawing.Size(157, 19);
            this.chbox_windowsAuth.TabIndex = 33;
            this.chbox_windowsAuth.Text = "Windows Authentication";
            this.chbox_windowsAuth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chbox_windowsAuth.UseVisualStyleBackColor = true;
            this.chbox_windowsAuth.CheckedChanged += new System.EventHandler(this.chbox_windowsAuth_CheckedChanged);
            // 
            // lbl_CollapseTopPanel
            // 
            this.lbl_CollapseTopPanel.AutoSize = true;
            this.lbl_CollapseTopPanel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_CollapseTopPanel.ForeColor = System.Drawing.Color.Blue;
            this.lbl_CollapseTopPanel.Location = new System.Drawing.Point(1065, 55);
            this.lbl_CollapseTopPanel.Name = "lbl_CollapseTopPanel";
            this.lbl_CollapseTopPanel.Size = new System.Drawing.Size(52, 15);
            this.lbl_CollapseTopPanel.TabIndex = 26;
            this.lbl_CollapseTopPanel.Text = "Collapse";
            this.lbl_CollapseTopPanel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl_CollapseTopPanel.Click += new System.EventHandler(this.lbl_CollapseTopPanel_Click);
            // 
            // txtbox_defaultDataLength
            // 
            this.txtbox_defaultDataLength.Location = new System.Drawing.Point(958, 41);
            this.txtbox_defaultDataLength.Name = "txtbox_defaultDataLength";
            this.txtbox_defaultDataLength.Size = new System.Drawing.Size(83, 23);
            this.txtbox_defaultDataLength.TabIndex = 35;
            // 
            // lbl_defaultDataLength
            // 
            this.lbl_defaultDataLength.AutoSize = true;
            this.lbl_defaultDataLength.Location = new System.Drawing.Point(867, 45);
            this.lbl_defaultDataLength.Name = "lbl_defaultDataLength";
            this.lbl_defaultDataLength.Size = new System.Drawing.Size(85, 15);
            this.lbl_defaultDataLength.TabIndex = 38;
            this.lbl_defaultDataLength.Text = "Default Length";
            // 
            // cmbox_delimiter
            // 
            this.cmbox_delimiter.FormattingEnabled = true;
            this.cmbox_delimiter.Items.AddRange(new object[] {
            "Tab",
            "Comma",
            "Semi-Colon",
            "Pipe"});
            this.cmbox_delimiter.Location = new System.Drawing.Point(758, 41);
            this.cmbox_delimiter.Name = "cmbox_delimiter";
            this.cmbox_delimiter.Size = new System.Drawing.Size(97, 23);
            this.cmbox_delimiter.TabIndex = 37;
            // 
            // lbl_delimiterLbl
            // 
            this.lbl_delimiterLbl.AutoSize = true;
            this.lbl_delimiterLbl.Location = new System.Drawing.Point(697, 46);
            this.lbl_delimiterLbl.Name = "lbl_delimiterLbl";
            this.lbl_delimiterLbl.Size = new System.Drawing.Size(55, 15);
            this.lbl_delimiterLbl.TabIndex = 36;
            this.lbl_delimiterLbl.Text = "Delimiter";
            // 
            // sc_innerContainer
            // 
            this.sc_innerContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sc_innerContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sc_innerContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.sc_innerContainer.Location = new System.Drawing.Point(3, 81);
            this.sc_innerContainer.Name = "sc_innerContainer";
            // 
            // sc_innerContainer.Panel1
            // 
            this.sc_innerContainer.Panel1.Controls.Add(this.lv_fileList);
            // 
            // sc_innerContainer.Panel2
            // 
            this.sc_innerContainer.Panel2.Controls.Add(this.dgv_FieldList);
            this.sc_innerContainer.Size = new System.Drawing.Size(1159, 487);
            this.sc_innerContainer.SplitterDistance = 677;
            this.sc_innerContainer.TabIndex = 34;
            // 
            // lv_fileList
            // 
            /* Custom Attributes */
            this.lv_fileList.AllowDrop = true;
            this.lv_fileList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lv_fileList.FullRowSelect = true;
            this.lv_fileList.LineAfter = -1;
            this.lv_fileList.LineBefore = -1;

            /* Normal Attributes */
            this.lv_fileList.CheckBoxes = true;
            this.lv_fileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lv_fileList.HideSelection = false;
            this.lv_fileList.Location = new System.Drawing.Point(0, 0);
            this.lv_fileList.Name = "lv_fileList";
            this.lv_fileList.Size = new System.Drawing.Size(675, 485);
            this.lv_fileList.TabIndex = 32;
            this.lv_fileList.UseCompatibleStateImageBehavior = false;
            this.lv_fileList.View = System.Windows.Forms.View.Details;
            this.lv_fileList.SelectedIndexChanged += new System.EventHandler(this.cbl_fileList_SelectedIndexChanged);
            this.lv_fileList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lv_fileList_MouseDown);
            this.lv_fileList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lv_fileList_MouseMove);
            this.lv_fileList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lv_fileList_MouseUp);
            // 
            // dgv_FieldList
            // 
            this.dgv_FieldList.AllowUserToAddRows = false;
            this.dgv_FieldList.AllowUserToDeleteRows = false;
            this.dgv_FieldList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_FieldList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.index,
            this.fieldName,
            this.dataType,
            this.dtLength});
            this.dgv_FieldList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_FieldList.Location = new System.Drawing.Point(0, 0);
            this.dgv_FieldList.Name = "dgv_FieldList";
            this.dgv_FieldList.RowHeadersWidth = 51;
            this.dgv_FieldList.RowTemplate.Height = 25;
            this.dgv_FieldList.Size = new System.Drawing.Size(476, 485);
            this.dgv_FieldList.TabIndex = 28;
            this.dgv_FieldList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_FieldList_CellValueChanged);
            // 
            // index
            // 
            this.index.HeaderText = "index";
            this.index.MinimumWidth = 6;
            this.index.Name = "index";
            this.index.ReadOnly = true;
            this.index.Visible = false;
            this.index.Width = 125;
            // 
            // fieldName
            // 
            this.fieldName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.fieldName.HeaderText = "Field Name";
            this.fieldName.MinimumWidth = 6;
            this.fieldName.Name = "fieldName";
            this.fieldName.ReadOnly = true;
            // 
            // dataType
            // 
            this.dataType.HeaderText = "Data Type";
            this.dataType.Items.AddRange(new object[] {
            "Text",
            "Integer",
            "Date",
            "DateTime"});
            this.dataType.MinimumWidth = 6;
            this.dataType.Name = "dataType";
            this.dataType.Width = 80;
            // 
            // dtLength
            // 
            this.dtLength.HeaderText = "Length";
            this.dtLength.MinimumWidth = 6;
            this.dtLength.Name = "dtLength";
            this.dtLength.ToolTipText = "Can be the word MAX or a number from 1-8000";
            this.dtLength.Width = 80;
            // 
            // lbl_ExpandTopPanel
            // 
            this.lbl_ExpandTopPanel.AutoSize = true;
            this.lbl_ExpandTopPanel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_ExpandTopPanel.ForeColor = System.Drawing.Color.Blue;
            this.lbl_ExpandTopPanel.Location = new System.Drawing.Point(1068, 0);
            this.lbl_ExpandTopPanel.Name = "lbl_ExpandTopPanel";
            this.lbl_ExpandTopPanel.Size = new System.Drawing.Size(47, 15);
            this.lbl_ExpandTopPanel.TabIndex = 27;
            this.lbl_ExpandTopPanel.Text = "Expand";
            this.lbl_ExpandTopPanel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl_ExpandTopPanel.Click += new System.EventHandler(this.lbl_ExpandTopPanel_Click);
            // 
            // chbox_CreateAllTablesTable
            // 
            this.chbox_CreateAllTablesTable.AutoSize = true;
            this.chbox_CreateAllTablesTable.Checked = true;
            this.chbox_CreateAllTablesTable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbox_CreateAllTablesTable.Location = new System.Drawing.Point(149, 722);
            this.chbox_CreateAllTablesTable.Name = "chbox_CreateAllTablesTable";
            this.chbox_CreateAllTablesTable.Size = new System.Drawing.Size(203, 19);
            this.chbox_CreateAllTablesTable.TabIndex = 33;
            this.chbox_CreateAllTablesTable.Text = "Create A Table With All Table Data";
            this.chbox_CreateAllTablesTable.UseVisualStyleBackColor = true;
            this.chbox_CreateAllTablesTable.CheckStateChanged += new System.EventHandler(this.chbox_CreateAllTablesTable_CheckedChanged);
            // 
            // chbox_DropTables
            // 
            this.chbox_DropTables.AutoSize = true;
            this.chbox_DropTables.Location = new System.Drawing.Point(19, 722);
            this.chbox_DropTables.Name = "chbox_DropTables";
            this.chbox_DropTables.Size = new System.Drawing.Size(124, 19);
            this.chbox_DropTables.TabIndex = 32;
            this.chbox_DropTables.Text = "Drop Tables If Exist";
            this.chbox_DropTables.UseVisualStyleBackColor = true;
            this.chbox_DropTables.CheckedChanged += new System.EventHandler(this.chbox_DropTables_CheckedChanged);
            // 
            // lbl_LoadToSQLStatus
            // 
            this.lbl_LoadToSQLStatus.Location = new System.Drawing.Point(16, 752);
            this.lbl_LoadToSQLStatus.Name = "lbl_LoadToSQLStatus";
            this.lbl_LoadToSQLStatus.Size = new System.Drawing.Size(1158, 22);
            this.lbl_LoadToSQLStatus.TabIndex = 30;
            this.lbl_LoadToSQLStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ms_Main
            // 
            this.ms_Main.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ms_Main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsm_file,
            this.tsmi_tools,
            this.helpToolStripMenuItem});
            this.ms_Main.Location = new System.Drawing.Point(0, 0);
            this.ms_Main.Name = "ms_Main";
            this.ms_Main.Size = new System.Drawing.Size(1195, 24);
            this.ms_Main.TabIndex = 27;
            this.ms_Main.Text = "menuStrip1";
            // 
            // tsm_file
            // 
            this.tsm_file.AutoSize = false;
            this.tsm_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_exit});
            this.tsm_file.Name = "tsm_file";
            this.tsm_file.Size = new System.Drawing.Size(37, 20);
            this.tsm_file.Text = "File";
            // 
            // tsmi_exit
            // 
            this.tsmi_exit.Name = "tsmi_exit";
            this.tsmi_exit.Size = new System.Drawing.Size(93, 22);
            this.tsmi_exit.Text = "Exit";
            this.tsmi_exit.Click += new System.EventHandler(this.tsm_exit_Click);
            // 
            // tsmi_tools
            // 
            this.tsmi_tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmii_excelToCsvConverter,
            this.toolStripSeparator1,
            this.tsmi_clearData});
            this.tsmi_tools.Name = "tsmi_tools";
            this.tsmi_tools.Size = new System.Drawing.Size(46, 20);
            this.tsmi_tools.Text = "Tools";
            // 
            // tsmii_excelToCsvConverter
            // 
            this.tsmii_excelToCsvConverter.Name = "tsmii_excelToCsvConverter";
            this.tsmii_excelToCsvConverter.Size = new System.Drawing.Size(277, 22);
            this.tsmii_excelToCsvConverter.Text = "Excel to CSV (Tab Delimited) Converter";
            this.tsmii_excelToCsvConverter.Click += new System.EventHandler(this.openExcelToCSv);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(274, 6);
            // 
            // tsmi_clearData
            // 
            this.tsmi_clearData.Name = "tsmi_clearData";
            this.tsmi_clearData.Size = new System.Drawing.Size(277, 22);
            this.tsmi_clearData.Text = "Clear All Data";
            this.tsmi_clearData.Click += new System.EventHandler(this.tsm_clearData_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // txtbox_FinalTableName
            // 
            this.txtbox_FinalTableName.Location = new System.Drawing.Point(360, 720);
            this.txtbox_FinalTableName.Name = "txtbox_FinalTableName";
            this.txtbox_FinalTableName.Size = new System.Drawing.Size(225, 23);
            this.txtbox_FinalTableName.TabIndex = 34;
            // 
            // btn_loadToSQL
            // 
            this.btn_loadToSQL.Location = new System.Drawing.Point(1090, 720);
            this.btn_loadToSQL.Name = "btn_loadToSQL";
            this.btn_loadToSQL.Size = new System.Drawing.Size(83, 23);
            this.btn_loadToSQL.TabIndex = 33;
            this.btn_loadToSQL.Text = "Load To SQL";
            this.btn_loadToSQL.UseVisualStyleBackColor = true;
            this.btn_loadToSQL.Click += new System.EventHandler(this.btn_loadToSQL_Click);
            // 
            // chbox_tablePrefix
            // 
            this.chbox_tablePrefix.AutoSize = true;
            this.chbox_tablePrefix.Checked = true;
            this.chbox_tablePrefix.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chbox_tablePrefix.Location = new System.Drawing.Point(613, 722);
            this.chbox_tablePrefix.Name = "chbox_tablePrefix";
            this.chbox_tablePrefix.Size = new System.Drawing.Size(219, 19);
            this.chbox_tablePrefix.TabIndex = 33;
            this.chbox_tablePrefix.Text = "Create Incremental Tables (w/ Prefix)";
            this.chbox_tablePrefix.UseVisualStyleBackColor = true;
            this.chbox_tablePrefix.CheckedChanged += new System.EventHandler(this.chbox_tablePrefix_CheckedChanged);
            this.chbox_tablePrefix.CheckStateChanged += new System.EventHandler(this.chbox_CreateAllTablesTable_CheckedChanged);
            // 
            // txtbox_tablePrefix
            // 
            this.txtbox_tablePrefix.Location = new System.Drawing.Point(838, 720);
            this.txtbox_tablePrefix.Name = "txtbox_tablePrefix";
            this.txtbox_tablePrefix.Size = new System.Drawing.Size(225, 23);
            this.txtbox_tablePrefix.TabIndex = 34;
            // 
            // lbl_warningIncremental
            // 
            this.lbl_warningIncremental.Location = new System.Drawing.Point(12, 704);
            this.lbl_warningIncremental.Name = "lbl_warningIncremental";
            this.lbl_warningIncremental.Size = new System.Drawing.Size(342, 15);
            this.lbl_warningIncremental.TabIndex = 35;
            // 
            // frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 781);
            this.Controls.Add(this.lbl_warningIncremental);
            this.Controls.Add(this.btn_loadToSQL);
            this.Controls.Add(this.txtbox_tablePrefix);
            this.Controls.Add(this.txtbox_FinalTableName);
            this.Controls.Add(this.lbl_LoadToSQLStatus);
            this.Controls.Add(this.sc_Main);
            this.Controls.Add(this.chbox_DropTables);
            this.Controls.Add(this.chbox_tablePrefix);
            this.Controls.Add(this.chbox_CreateAllTablesTable);
            this.Controls.Add(this.ms_Main);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.ms_Main;
            this.Name = "frm_Main";
            this.Text = "Import Flat Files in Bulk";
            this.sc_Main.Panel1.ResumeLayout(false);
            this.sc_Main.Panel1.PerformLayout();
            this.sc_Main.Panel2.ResumeLayout(false);
            this.sc_Main.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sc_Main)).EndInit();
            this.sc_Main.ResumeLayout(false);
            this.sc_innerContainer.Panel1.ResumeLayout(false);
            this.sc_innerContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sc_innerContainer)).EndInit();
            this.sc_innerContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_FieldList)).EndInit();
            this.ms_Main.ResumeLayout(false);
            this.ms_Main.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_testConnection;
        private System.Windows.Forms.Label lbl_filePickUpPath;
        private System.Windows.Forms.TextBox txtbox_pickUpPath;
        private System.Windows.Forms.Label lbl_sqlDatabase;
        private System.Windows.Forms.TextBox txtbox_sqlDatabase;
        private System.Windows.Forms.Label lbl_sqlPass;
        private System.Windows.Forms.TextBox txtbox_sqlPass;
        private System.Windows.Forms.Label lbl_sqlUser;
        private System.Windows.Forms.TextBox txtbox_sqlUser;
        private System.Windows.Forms.Label lbl_sqlServer;
        private System.Windows.Forms.TextBox txtbox_sqlServer;
        private System.Windows.Forms.Label lbl_testConnStatus;
        private System.Windows.Forms.Button btn_openFileDiag;
        private System.Windows.Forms.Button btn_loadFilesToList;
        private System.Windows.Forms.Label lbl_loadFilesStatus;
        private System.Windows.Forms.SplitContainer sc_Main;
        private System.Windows.Forms.Label lbl_CollapseTopPanel;
        private System.Windows.Forms.Label lbl_ExpandTopPanel;
        private System.Windows.Forms.DataGridView dgv_FieldList;
        private System.Windows.Forms.Label lbl_LoadToSQLStatus;
        private System.Windows.Forms.CheckBox chbox_CreateAllTablesTable;
        private System.Windows.Forms.CheckBox chbox_DropTables;
        private System.Windows.Forms.MenuStrip ms_Main;
        private System.Windows.Forms.ToolStripMenuItem tsm_file;
        private System.Windows.Forms.ToolStripMenuItem tsmi_exit;
        private System.Windows.Forms.ToolStripMenuItem tsmi_tools;
        private System.Windows.Forms.ToolStripMenuItem tsmi_clearData;
        private System.Windows.Forms.CheckBox chbox_windowsAuth;
        private System.Windows.Forms.SplitContainer sc_innerContainer;
        private System.Windows.Forms.DataGridViewTextBoxColumn index;
        private System.Windows.Forms.DataGridViewTextBoxColumn fieldName;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dtLength;
        private System.Windows.Forms.ComboBox cmbox_delimiter;
        private System.Windows.Forms.Label lbl_delimiterLbl;
        private System.Windows.Forms.Label lbl_defaultDataLength;
        private System.Windows.Forms.TextBox txtbox_defaultDataLength;
        private ListViewCustomReorder.ListViewEx lv_fileList;
        private System.Windows.Forms.Button btn_loadToSQL;
        private System.Windows.Forms.TextBox txtbox_FinalTableName;
        private System.Windows.Forms.ToolStripMenuItem tsmii_excelToCsvConverter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.CheckBox chbox_tablePrefix;
        private System.Windows.Forms.TextBox txtbox_tablePrefix;
        private System.Windows.Forms.Label lbl_warningIncremental;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}

