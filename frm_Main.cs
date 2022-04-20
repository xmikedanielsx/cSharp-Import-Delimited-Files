using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections;
using System.Globalization;
using System.Diagnostics;

namespace BulkImportDelimitedFlatFiles
{

    public partial class frm_Main : Form
    {


        SqlConnection cnn = new SqlConnection();
        int hasError = 1;
        int hasFileLoadError = 0;
        //FileInfo[] files = default(FileInfo[]);
        Dictionary<string, DataTable> filesToLoad = new Dictionary<string, DataTable>();
        Dictionary<string, Dictionary<string, Dictionary<string, string>>> filesToLoadMapping = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();
        Boolean fileListLoaded = false;
        LocalConfig lconfig;
        string appFile;
        string appFolder;
        string cfgJsonFile;
        // The LVItem being dragged
        private ListViewItem _itemDnD = null;
        ColumnSorter m_lstColumnSorter = new ColumnSorter();
        parseFile pf;
        frm_ErrorList errorDialog;
        string[] extensions = new[] { ".txt", ".csv" };

        public frm_Main()
        {
            InitializeComponent();
            lbl_ExpandTopPanel.Visible = false;
            cmbox_delimiter.SelectedItem = "Tab";
            txtbox_defaultDataLength.Text = "max";
            dgv_FieldList.Enabled = false;
            this.appFile = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
            this.appFolder = Path.GetDirectoryName(this.appFile) + @"\";
            this.cfgJsonFile = appFolder + @"config.json";
            this.setJsonObjectConfig();
            btn_loadToSQL.Enabled = false;
            lv_fileList.ListViewItemSorter = m_lstColumnSorter;

            if (File.Exists(cfgJsonFile))
            {
                string userDirForApp = Path.Combine(Environment.GetEnvironmentVariable("APPDATA"), "sqlBulkImporter");
                string newPathConfigFile;
                if (!Directory.Exists(userDirForApp))
                {
                    Directory.CreateDirectory(userDirForApp);
                }
                newPathConfigFile = Path.Combine(userDirForApp, "config.json");
                if (!File.Exists(newPathConfigFile))
                {
                    File.Copy(cfgJsonFile, newPathConfigFile);
                }
                this.cfgJsonFile = newPathConfigFile;
            }
        }

        private void setJsonObjectConfig()
        {
            if (File.Exists(this.cfgJsonFile))
            {
                this.lconfig = JsonSerializer.Deserialize<LocalConfig>(File.ReadAllText(this.cfgJsonFile));
            }

            txtbox_FinalTableName.Text = this.lconfig.tableName;
            txtbox_tablePrefix.Text = this.lconfig.tablePrefix;
        }


        private void btn_testConnection_Click(object sender, EventArgs e)
        {
            string cnnString;
            string sqlServer;
            string sqlUser;
            string sqlPass;
            string sqlDatabase;

            // set sql variables
            sqlServer = txtbox_sqlServer.Text.ToString(); sqlUser = txtbox_sqlUser.Text.ToString(); sqlPass = txtbox_sqlPass.Text.ToString(); sqlDatabase = txtbox_sqlDatabase.Text.ToString();

            if (chbox_windowsAuth.Checked == true)
            {
                if (txtbox_sqlServer.Text.ToString() == "" || txtbox_sqlDatabase.Text.ToString() == "")
                {
                    MessageBox.Show("I am sorry, but you need to make sure you fill out ALL of the SQL Server Connection Info Boxes");
                    this.hasError = 1;
                    return;
                }
                cnnString = "Data Source=" + sqlServer + ";Initial Catalog=\"" + sqlDatabase + "\";Integrated Security=true;";
            }
            else
            {
                if (txtbox_sqlServer.Text.ToString() == "" || txtbox_sqlUser.Text.ToString() == "" || txtbox_sqlPass.Text.ToString() == "" || txtbox_sqlDatabase.Text.ToString() == "")
                {
                    MessageBox.Show("I am sorry, but you need to make sure you fill out ALL of the SQL Server Connection Info Boxes");
                    this.hasError = 1;
                    return;
                }
                cnnString = "Data Source=" + sqlServer + ";Initial Catalog=" + sqlDatabase + ";User ID=" + sqlUser + ";Password=" + sqlPass + "";
            }


            if (this.cnn.State == ConnectionState.Open)
            {
                this.cnn.Close();
            }
            this.cnn.ConnectionString = cnnString;
            lbl_testConnStatus.ForeColor = Color.Black;
            lbl_testConnStatus.Text = "Trying to Connect...";
            try
            {
                this.hasError = 0;
                this.cnn.Open();
                lbl_testConnStatus.ForeColor = Color.Green;
                lbl_testConnStatus.Text = "Connection Successful";
                btn_loadToSQL.Enabled = (fileListLoaded == true && hasError == 0) ? true : false;
                this.cnn.Close();

            }
            catch (Exception err)
            {
                hasError = 1;
                lbl_testConnStatus.ForeColor = Color.Red;
                lbl_testConnStatus.Text = err.Message.ToString();
                btn_loadToSQL.Enabled = (fileListLoaded == true && hasError == 0) ? true : false;
                return;
            }

        }

        private void btn_openFileDiag_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            DialogResult dr = fd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                txtbox_pickUpPath.Text = fd.SelectedPath.ToString();
            }

        }

        public void addFileToList(string fileName)
        {
            getFileDetails gd = new getFileDetails(fileName);
            ListViewItem nlvi = new ListViewItem(gd.createFileArray());
            this.lv_fileList.Items.Add(nlvi);
            MessageBox.Show(this.lv_fileList.Items.Count.ToString());
        }

        public void addFailedFileToList(string failedFile)
        {
            getFileDetails gd = new getFileDetails(failedFile);
            ListViewItem nlvi = new ListViewItem(gd.createFileArray());
            lv_fileList.Items.Add(nlvi).ForeColor = Color.Red;
            MessageBox.Show(lv_fileList.Items.Count.ToString());
        }

        private async void btn_loadFilesToList_Click(object sender, EventArgs e)
        {
            /*
             *  VALIDATION STUFF
             */
            errorDialog = new frm_ErrorList();
            this.hasFileLoadError = 0;
            int hasFileLoadErrorOnFiles = 0;
            btn_loadFilesToList.Enabled = false;
            btn_loadFilesToList.Text = "Loading...";
            string filesPath = txtbox_pickUpPath.Text;
            string fileDelimiter = cmbox_delimiter.SelectedItem.ToString();


            if ( string.IsNullOrEmpty(filesPath) || !Directory.Exists(filesPath))
            {
                MessageBox.Show("I am sorry your folder path is either blank or does not exist");
                btn_loadFilesToList.Enabled = true;
                btn_loadFilesToList.Text = "Load Files";
                hasFileLoadError = 1;
                return;
            }

            // set Directory Info Object
            DirectoryInfo di = new DirectoryInfo(filesPath);

            // Set MVVM (Model, View, View, Model)


            /*
             * Clear Files List
             */
            lv_fileList.Items.Clear();
            filesToLoad.Clear();
            dgv_FieldList.Rows.Clear();
            


            // Create new object to start processing
            pf = new parseFile(filesPath, fileDelimiter, lv_fileList, filesToLoad, extensions);

            // process files list
            await pf.ldFiles();


            // set fileListLoaded so rest of application knows
            this.fileListLoaded = true;

            // update the file Mapping for each
            //foreach (ListViewItem lv in lv_fileList.Items.OfType<ListViewItem>().ToArray())
            //{
            //    if(lv.ForeColor == Color.Red)
            //    {
            //        hasFileLoadErrorOnFiles = 1;
            //        lv_fileList.Items.Remove(lv);
            //        errorDialog.lv_errorList.Items.Add(lv);
            //    } else
            //    {    // ToDo: This should be a separate Thread
            //        this.updateFileMapping(lv.Index);
            //    }
            //}
            foreach (ListViewItem lv in lv_fileList.Items)
            {
                if (lv.ForeColor == Color.Red)
                {
                    hasFileLoadErrorOnFiles = 1;
                    lv_fileList.Items.Remove(lv);
                    errorDialog.lv_errorList.Items.Add(lv);
                }
                
            }
            foreach (ListViewItem lv in lv_fileList.Items)
            {
                // ToDo: This should be a separate Thread
                this.updateFileMapping(lv.Index);
            }


            if (hasFileLoadErrorOnFiles == 1) { errorDialog.Show(); }

            // Select first Item in List
            if (lv_fileList.Items.Count != 0)
            {
                lv_fileList.Items[0].Selected = true;
                lv_fileList.Items[0].Focused = true;
            }

            // Set standard Button Stuff
            btn_loadFilesToList.Enabled = true;
            btn_loadFilesToList.Text = "Load Files";
            dgv_FieldList.Enabled = true;
            lv_fileList.Enabled = true;
            btn_loadToSQL.Enabled = hasError == 0 ? true : false;
        }



        private async void btn_loadToSQL_Click(object sender, EventArgs e)
        {
            frm_loadStatus ltDialog = new frm_loadStatus();
            Boolean useTablePrefix = chbox_tablePrefix.Checked;
            string tblPrefix = txtbox_tablePrefix.Text;
            string finalTblName = txtbox_FinalTableName.Text;
            Boolean DropTablesOrNot = chbox_DropTables.Checked;
            Boolean CreateMasterTable = chbox_CreateAllTablesTable.Checked;
            Dictionary<string, Dictionary<string, Dictionary<string, string>>> ftlm = this.filesToLoadMapping;
            string AppFolderLocation = this.appFolder;

            loadToSQL lts = new loadToSQL(
                  useTablePrefix
                , tblPrefix
                , finalTblName
                , DropTablesOrNot
                , CreateMasterTable
                , ftlm
                , AppFolderLocation
                , lv_fileList
                , filesToLoad
                , cnn
                , ltDialog
            );

            btn_loadToSQL.Enabled = false;
            btn_loadFilesToList.Enabled = false;
            btn_loadToSQL.Text = "Loading...";


            this.btn_testConnection_Click(sender, e);
            if (hasError == 1) { MessageBox.Show("Please resolve either your SQL Server Error before you try to Load to SQL"); return; }
            if (hasFileLoadError == 1) { MessageBox.Show("Please resolve either your File Load Error before you try to Load to SQL"); return; }

            // some basic validation
            if (txtbox_FinalTableName.Text.Trim() == "" && chbox_CreateAllTablesTable.Checked == true)
            {
                MessageBox.Show("Sorry you have chosen to create an all tables and left the final table name blank. Please fill something in.");
                return;
            }
            int fCount = this.lv_fileList.CheckedItems.Count;
            if (fCount == 0)
            {
                MessageBox.Show("Sorry but you have not selected any files to Load. Please select the check box next to the files you wish to load.");
                return;
            }


            ltDialog.Text = txtbox_sqlServer.Text;
            ltDialog.Show();
            ltDialog.setLoadingText(Color.Black, @"Trying to get your latest incrementing table");

            await lts.loadTablesToSQLServer();

            ltDialog.showCancelButton();
            btn_loadToSQL.Text = "Load To SQL";
            btn_loadToSQL.Enabled = true;
            btn_loadFilesToList.Enabled = true;
        }

        

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, sc_Main.Panel1.ClientRectangle,
                Color.Black, 1, ButtonBorderStyle.Solid,
                Color.Black, 1, ButtonBorderStyle.Solid,
                Color.Black, 1, ButtonBorderStyle.Solid,
                Color.Black, 1, ButtonBorderStyle.Solid);
        }

        private void lbl_CollapseTopPanel_Click(object sender, EventArgs e)
        {
            sc_Main.Panel1Collapsed = true;
            lbl_ExpandTopPanel.Visible = true;
        }

        private void lbl_ExpandTopPanel_Click(object sender, EventArgs e)
        {
            sc_Main.Panel1Collapsed = false;
            lbl_ExpandTopPanel.Visible = false;
        }

        private void tsm_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tsm_clearData_Click(object sender, EventArgs e)
        {
            string msg = "Are you sure you want to clear all the data on screen?";
            var result = MessageBox.Show(msg, "Clear Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            // If the no button was pressed ...
            if (result == DialogResult.OK)
            {
                // clear the screens data
                this.clearTheScreen();

            }
        }

        private void clearTheScreen()
        {
            // Clear SQL Server Stuff
            // CheckBoxes
            txtbox_sqlServer.Text = "";
            txtbox_sqlUser.Text = "";
            txtbox_sqlPass.Text = "";
            txtbox_sqlDatabase.Text = "";
            txtbox_pickUpPath.Text = "";

            // Labels
            lbl_testConnStatus.Text = "";

            // Clear bottom Checkboxes
            chbox_DropTables.Checked = false;
            chbox_CreateAllTablesTable.Checked = true;

            // Clear lists and tables
            lv_fileList.Items.Clear();
            dgv_FieldList.Rows.Clear();

            // Clear Other Labels
            lbl_LoadToSQLStatus.Text = "";
            lbl_loadFilesStatus.Text = "";

        }

        int dblbl_left;
        int dbtxtbox_left;
        int btntestconn_left;
        private void chbox_windowsAuth_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chbox_windowsAuth.Checked == true)
            {
                txtbox_sqlUser.Enabled = false;
                txtbox_sqlPass.Enabled = false;
                txtbox_sqlUser.Visible = false;
                txtbox_sqlPass.Visible = false;
                lbl_sqlUser.Visible = false;
                lbl_sqlPass.Visible = false;
                this.dblbl_left = lbl_sqlDatabase.Left;
                this.dbtxtbox_left = txtbox_sqlDatabase.Left;
                this.btntestconn_left = btn_testConnection.Left;
                lbl_sqlDatabase.Left = txtbox_sqlServer.Right + 10;
                txtbox_sqlDatabase.Left = lbl_sqlDatabase.Right + 10;
                btn_testConnection.Left = txtbox_sqlDatabase.Right + 10;
            }
            else
            {
                txtbox_sqlUser.Enabled = true;
                txtbox_sqlPass.Enabled = true;
                txtbox_sqlUser.Visible = true;
                txtbox_sqlPass.Visible = true;
                lbl_sqlUser.Visible = true;
                lbl_sqlPass.Visible = true;
                lbl_sqlDatabase.Left = this.dblbl_left;
                txtbox_sqlDatabase.Left = this.dbtxtbox_left;
                btn_testConnection.Left = this.btntestconn_left;
            }
        }
        private void updateFileMapping(int idx)
        {

            dgv_FieldList.Rows.Clear();
            string fileName = lv_fileList.Items[idx].SubItems[0].Text;
            FileInfo fl = new FileInfo(fileName);
            if (!this.filesToLoad.ContainsKey(fl.FullName))
            {
                return;
            }
            DataTable dt = this.filesToLoad[fl.FullName];
            Dictionary<string, Dictionary<string, string>> ftm = null;
            if (this.filesToLoadMapping.ContainsKey(fl.FullName))
            {
                ftm = this.filesToLoadMapping[fl.FullName];
            }

            if (ftm == null)
            {
                // Mapping Doesn't exist prefil will generic mapping
                foreach (DataColumn col in dt.Columns)
                {
                    dgv_FieldList.Rows.Add(
                        col.Ordinal.ToString()
                        , col.ColumnName
                        , "Text"
                        , txtbox_defaultDataLength.Text
                    );
                }
                this.updateCellValue(null, idx);
            }
            else
            {
                // Mapping Does Exist. Get Mapping and prefill with mapping
                foreach (System.Collections.Generic.KeyValuePair<string, Dictionary<string, string>> em in ftm)
                {
                    dgv_FieldList.Rows.Add(
                        em.Value["Ordinal"].ToString()
                        , em.Value["fieldName"].ToString()
                        , em.Value["dataType"]
                        , em.Value["dtLength"]
                    );
                }
            }
        }

        private void cbl_fileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(sender is ListView lv)
            {
                this.updateFileMapping(lv.SelectedIndex());
            }
            
        }

        private void dgv_FieldList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            this.dgv_FieldList_CellValueChanged_Call(sender, e, -1);
        }
        private void updateMappingForFile(int idx)
        {
            if (this.lv_fileList != null)
            {
                FileInfo fl =  new FileInfo(lv_fileList.Items[idx].SubItems[0].Text);
                this.filesToLoadMapping.Remove(fl.FullName.ToString());
                Dictionary<string, Dictionary<string, string>> ml = new Dictionary<string, Dictionary<string, string>>();
                foreach (DataGridViewRow r in dgv_FieldList.Rows)
                {
                    Dictionary<string, string> dmr = new Dictionary<string, string>();

                    dmr.Add("Ordinal", r.Cells["index"].Value.ToString());
                    dmr.Add("fieldName", r.Cells["fieldName"].Value.ToString());
                    dmr.Add("dataType", r.Cells["dataType"].Value.ToString());
                    dmr.Add("dtLength", r.Cells["dtLength"].Value.ToString());
                    ml.Add(r.Cells["index"].Value.ToString(), dmr);
                }
                this.filesToLoadMapping.Add(fl.FullName.ToString(), ml);

            }
        }
        private void dgv_FieldList_CellValueChanged_Call(object sender, DataGridViewCellEventArgs e, int ci)
        {
            this.updateCellValue(e.RowIndex, ci);

        }

        //private void updateCellValue(DataGridViewCellEventArgs e, int ci)
        private void updateCellValue(int? rowIndex , int ci)
        {
            int idx = ci == -1 ? lv_fileList.SelectedIndex() : ci;
            if (this.fileListLoaded)
            {
                if (rowIndex == null)
                {
                    this.updateMappingForFile(idx);
                    return;
                }
                if (rowIndex.Value != 0)
                {
                    //MessageBox.Show(ri.ToString());
                    DataGridViewRow dgvr = dgv_FieldList.Rows[rowIndex.Value];
                    string input = dgvr.Cells["dtLength"].Value.ToString();
                    string patt1 = @"\d+";
                    Match m1 = Regex.Match(input, patt1);
                    Boolean mm1;
                    if (m1.Length > 0) { mm1 = true; } else { mm1 = false; }
                    if (
                            dgvr.Cells["dataType"].Value.ToString() == "Text"
                            && input.ToUpper() != "MAX"
                            && !mm1
                        )
                    {
                        MessageBox.Show("Your Value is invalid for the data length");
                        dgvr.Cells["dtLength"].Value = "max";
                    }
                    this.updateMappingForFile(idx);
                }

            }
        }


        private void btn_CheckStuff_Click(object sender, EventArgs e)
        {
            string opt = "";
            foreach (System.Collections.Generic.KeyValuePair<string, System.Data.DataTable> fl in this.filesToLoad)
            {
                opt += fl.Key.ToString() + "\n";
                Dictionary<string, Dictionary<string, string>> fm = this.filesToLoadMapping[fl.Key.ToString()];
                foreach (System.Collections.Generic.KeyValuePair<string, Dictionary<string, string>> fmr in fm)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> fmrv in fmr.Value)
                    {
                        opt += fmrv.Key.ToString() + " >>> " + fmrv.Value.ToString() + "\n";
                    }

                }
            }
            MessageBox.Show(opt);
        }

        

        private void chbox_CreateAllTablesTable_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chbox_CreateAllTablesTable.Checked)
            {
                txtbox_FinalTableName.Visible = true;
            }
            else
            {
                txtbox_FinalTableName.Visible = false;
            }
        }

        private void openExcelToCSv(object sender, EventArgs e)
        {
            frm_excelToCSV etc = new frm_excelToCSV();
            etc.Show();
        }

        private void chbox_tablePrefix_CheckedChanged(object sender, EventArgs e)
        {
            if (chbox_tablePrefix.Checked)
            {
                txtbox_tablePrefix.Visible = true;
            }
            else
            {
                txtbox_tablePrefix.Visible = false;
            }
            if (chbox_tablePrefix.Checked && chbox_DropTables.Checked)
            {
                lbl_warningIncremental.Text = "Drop Tables will be ignored w/create incremental checked";
                lbl_warningIncremental.ForeColor = Color.Red;
            }
            else
            {
                lbl_warningIncremental.Text = "";
            }
        }

        private void chbox_DropTables_CheckedChanged(object sender, EventArgs e)
        {
            if (chbox_tablePrefix.Checked && chbox_DropTables.Checked)
            {
                lbl_warningIncremental.Text = "Drop Tables will be ignored w/create incremental checked";
                lbl_warningIncremental.ForeColor = Color.Red;
            }
            else
            {
                lbl_warningIncremental.Text = "";
            }
        }

        private void lbl_sqlPass_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = @"Bulk Importer Tool" + Environment.NewLine + @"Build 1.0.1.9" + Environment.NewLine + Environment.NewLine + @"Contact: Mike Daniels (Git Hub)" + Environment.NewLine + Environment.NewLine + @"https://github.com/xmikedanielsx";
            frm_infoBox ibox = new frm_infoBox();
            ibox.setMessage(Color.Black, msg);
            //ibox.Left = (this.Left + this.Width) / 2;
            //ibox.Top = (this.Top + this.Height) / 2;

            ibox.ShowDialog();
        }

        private void lv_fileList_MouseDown(object sender, MouseEventArgs e)
        {
            _itemDnD = lv_fileList.GetItemAt(e.X, e.Y);
        }

        private void lv_fileList_MouseMove(object sender, MouseEventArgs e)
        {
            if (_itemDnD == null)
                return;

            // Show the user that a drag operation is happening
            Cursor = Cursors.Hand;

            // calculate the bottom of the last item in the LV so that you don't have to stop your drag at the last item
            int lastItemBottom = Math.Min(e.Y, lv_fileList.Items[lv_fileList.Items.Count - 1].GetBounds(ItemBoundsPortion.Entire).Bottom - 1);

            // use 0 instead of e.X so that you don't have to keep inside the columns while dragging
            ListViewItem itemOver = lv_fileList.GetItemAt(0, lastItemBottom);

            if (itemOver == null)
                return;

            Rectangle rc = itemOver.GetBounds(ItemBoundsPortion.Entire);
            if (e.Y < rc.Top + (rc.Height / 2))
            {
                lv_fileList.LineBefore = itemOver.Index;
                lv_fileList.LineAfter = -1;
            }
            else
            {
                lv_fileList.LineBefore = -1;
                lv_fileList.LineAfter = itemOver.Index;
            }

            // invalidate the LV so that the insertion line is shown
            lv_fileList.Invalidate();
        }

        public void showFirstIndexLV()
        {
            //MessageBox.Show(lv_fileList.CheckedItems[0].Text.ToString());
        }

        private void lv_fileList_MouseUp(object sender, MouseEventArgs e)
        {
            if (_itemDnD == null)
                return;

            try
            {
                // calculate the bottom of the last item in the LV so that you don't have to stop your drag at the last item
                int lastItemBottom = Math.Min(e.Y, lv_fileList.Items[lv_fileList.Items.Count - 1].GetBounds(ItemBoundsPortion.Entire).Bottom - 1);

                // use 0 instead of e.X so that you don't have to keep inside the columns while dragging
                ListViewItem itemOver = lv_fileList.GetItemAt(0, lastItemBottom);

                if (itemOver == null)
                    return;

                Rectangle rc = itemOver.GetBounds(ItemBoundsPortion.Entire);

                // find out if we insert before or after the item the mouse is over
                bool insertBefore;
                if (e.Y < rc.Top + (rc.Height / 2))
                {
                    insertBefore = true;
                }
                else
                {
                    insertBefore = false;
                }

                if (_itemDnD != itemOver) // if we dropped the item on itself, nothing is to be done
                {
                    if (insertBefore)
                    {
                        lv_fileList.Items.Remove(_itemDnD);
                        lv_fileList.Items.Insert(itemOver.Index, _itemDnD);
                    }
                    else
                    {
                        lv_fileList.Items.Remove(_itemDnD);
                        lv_fileList.Items.Insert(itemOver.Index + 1, _itemDnD);
                    }
                }

                // clear the insertion line
                lv_fileList.LineAfter =
                lv_fileList.LineBefore = -1;

                lv_fileList.Invalidate();

            }
            finally
            {
                // finish drag&drop operation
                _itemDnD = null;
                this.showFirstIndexLV();
                Cursor = Cursors.Default;
            }
        }

        private void lv_fileList_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            //MessageBox.Show(lv_fileList.Sorting.ToString());
            

            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == m_lstColumnSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (m_lstColumnSorter.Order == System.Windows.Forms.SortOrder.Ascending)
                {
                    m_lstColumnSorter.Order = System.Windows.Forms.SortOrder.Descending;
                }
                else
                {
                    m_lstColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                m_lstColumnSorter.SortColumn = e.Column;
                m_lstColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            lv_fileList.Sort();
            lv_fileList.SetSortIcon(m_lstColumnSorter.SortColumn, m_lstColumnSorter.Order);
        }

        private void lv_fileList_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if(e.Item.ForeColor == Color.Red)
            {
                e.Item.Checked = false;
            }
        }

        // ToDo: Add a Debouncer to update JSON Config when someone changes the prefix or master table
        private void txtbox_FinalTableName_TextChanged(object sender, EventArgs e)
        {
            //Action<int> a = (arg) =>
            //{
            //    this.lconfig.tableName = ((TextBox)sender).Text;
            //    string json = JsonSerializer.Serialize(this.lconfig);
            //    File.WriteAllText(cfgJsonFile, json);
                
            //};
            //var debouncedWrapper = a.Debounce<int>();

            //while (true)
            //{
            //    var rndVal = 400;
            //    Thread.Sleep(rndVal);
            //    debouncedWrapper(rndVal);
            //}

        }

        
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileToOpen = "SomeFilePathHere";
            var process = new Process();
            process.StartInfo = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = this.cfgJsonFile
            };

            process.Start();
            process.WaitForExit();
            this.setJsonObjectConfig();


        }

        private void txtbox_tablePrefix_TextChanged(object sender, EventArgs e)
        {
          
        }
    }
        

    public class ColumnSorter : IComparer
    {
        private int sortColumn;

        public int SortColumn
        {
            set { sortColumn = value; }
            get { return sortColumn; }
        }

        private System.Windows.Forms.SortOrder sortOrder;

        public System.Windows.Forms.SortOrder Order
        {
            set { sortOrder = value; }
            get { return sortOrder; }
        }

        private Comparer listViewItemComparer;

        public ColumnSorter()
        {
            sortColumn = 0;

            sortOrder = System.Windows.Forms.SortOrder.None;

            listViewItemComparer = new Comparer(CultureInfo.CurrentUICulture);
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            try
            {
                ListViewItem lviX = (ListViewItem)x;
                ListViewItem lviY = (ListViewItem)y;
                int cntOfPad = 20;
                string str1 = lviX.SubItems[sortColumn].Text;
                string str2 = lviY.SubItems[sortColumn].Text;
                str1 = str1.PadLeft(cntOfPad, '0');
                str2 = str2.PadLeft(cntOfPad, '0');
                if (int.TryParse(str1, out int result) && int.TryParse(str2, out int result1))
                {
                    str1 = str1.Substring(str1.Length - cntOfPad, cntOfPad);
                    str2 = str2.Substring(str2.Length - cntOfPad, cntOfPad);
                }

                int compareResult = 0;

                if (lviX.SubItems[sortColumn].Tag != null && lviY.SubItems[sortColumn].Tag != null)
                {
                    compareResult = listViewItemComparer.Compare(lviX.SubItems[sortColumn].Tag, lviY.SubItems[sortColumn].Tag);
                }
                else
                {
                    compareResult = listViewItemComparer.Compare(str1, str2);
                    //compareResult = listViewItemComparer.Compare(lviX.SubItems[sortColumn].Text, lviY.SubItems[sortColumn].Text);
                }

                if (sortOrder == System.Windows.Forms.SortOrder.Ascending)
                {
                    return compareResult;
                }
                else if (sortOrder == System.Windows.Forms.SortOrder.Descending)
                {
                    return (-compareResult);
                }
                else
                {
                    return 0;
                }

            }
            catch
            {
                return 0;
            }
        }
    }

    public static class Extension
    {
        public static int SelectedIndex(this ListView listView)
        {
            if (listView.SelectedIndices.Count > 0)
                return listView.SelectedIndices[0];
            else
                return 0;
        }

        public static Action<T> Debounce<T>(this Action<T> func, int milliseconds = 300)
        {
            var last = 0;
            return arg =>
            {
                var current = Interlocked.Increment(ref last);
                Task.Delay(milliseconds).ContinueWith(task =>
                {
                    if (current == last) func(arg);
                    task.Dispose();
                });
            };
        }

    }

    public class LocalConfig
    {
        public string tableName { get; set; }
        public string tablePrefix { get; set; }
    }

    internal static class ListViewExtensions
    {
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct LVCOLUMN
        {
            public Int32 mask;
            public Int32 cx;
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPTStr)]
            public string pszText;
            public IntPtr hbm;
            public Int32 cchTextMax;
            public Int32 fmt;
            public Int32 iSubItem;
            public Int32 iImage;
            public Int32 iOrder;
        }

        const Int32 HDI_WIDTH = 0x0001;
        const Int32 HDI_HEIGHT = HDI_WIDTH;
        const Int32 HDI_TEXT = 0x0002;
        const Int32 HDI_FORMAT = 0x0004;
        const Int32 HDI_LPARAM = 0x0008;
        const Int32 HDI_BITMAP = 0x0010;
        const Int32 HDI_IMAGE = 0x0020;
        const Int32 HDI_DI_SETITEM = 0x0040;
        const Int32 HDI_ORDER = 0x0080;
        const Int32 HDI_FILTER = 0x0100;

        const Int32 HDF_LEFT = 0x0000;
        const Int32 HDF_RIGHT = 0x0001;
        const Int32 HDF_CENTER = 0x0002;
        const Int32 HDF_JUSTIFYMASK = 0x0003;
        const Int32 HDF_RTLREADING = 0x0004;
        const Int32 HDF_OWNERDRAW = 0x8000;
        const Int32 HDF_STRING = 0x4000;
        const Int32 HDF_BITMAP = 0x2000;
        const Int32 HDF_BITMAP_ON_RIGHT = 0x1000;
        const Int32 HDF_IMAGE = 0x0800;
        const Int32 HDF_SORTUP = 0x0400;
        const Int32 HDF_SORTDOWN = 0x0200;

        const Int32 LVM_FIRST = 0x1000;         // List messages
        const Int32 LVM_GETHEADER = LVM_FIRST + 31;
        const Int32 HDM_FIRST = 0x1200;         // Header messages
        const Int32 HDM_SETIMAGELIST = HDM_FIRST + 8;
        const Int32 HDM_GETIMAGELIST = HDM_FIRST + 9;
        const Int32 HDM_GETITEM = HDM_FIRST + 11;
        const Int32 HDM_SETITEM = HDM_FIRST + 12;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SendMessage")]
        private static extern IntPtr SendMessageLVCOLUMN(IntPtr hWnd, Int32 Msg, IntPtr wParam, ref LVCOLUMN lPLVCOLUMN);


        //This method used to set arrow icon
        public static void SetSortIcon(this ListView listView, int columnIndex, System.Windows.Forms.SortOrder order)
        {
            IntPtr columnHeader = SendMessage(listView.Handle, LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);

            for (int columnNumber = 0; columnNumber <= listView.Columns.Count - 1; columnNumber++)
            {
                IntPtr columnPtr = new IntPtr(columnNumber);
                LVCOLUMN lvColumn = new LVCOLUMN();
                lvColumn.mask = HDI_FORMAT;

                SendMessageLVCOLUMN(columnHeader, HDM_GETITEM, columnPtr, ref lvColumn);

                if (!(order == System.Windows.Forms.SortOrder.None) && columnNumber == columnIndex)
                {
                    switch (order)
                    {
                        case System.Windows.Forms.SortOrder.Ascending:
                            lvColumn.fmt &= ~HDF_SORTDOWN;
                            lvColumn.fmt |= HDF_SORTUP;
                            break;
                        case System.Windows.Forms.SortOrder.Descending:
                            lvColumn.fmt &= ~HDF_SORTUP;
                            lvColumn.fmt |= HDF_SORTDOWN;
                            break;
                    }
                    lvColumn.fmt |= (HDF_LEFT | HDF_BITMAP_ON_RIGHT);
                }
                else
                {
                    lvColumn.fmt &= ~HDF_SORTDOWN & ~HDF_SORTUP & ~HDF_BITMAP_ON_RIGHT;
                }

                SendMessageLVCOLUMN(columnHeader, HDM_SETITEM, columnPtr, ref lvColumn);
            }
        }
    }

    public interface subClass
    {
       ListViewCustomReorder.ListViewEx lv_fileList { set; get; }
        void addFileToList(string fileName);
    }
}
