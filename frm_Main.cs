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

namespace BulkImportDelimitedFlatFiles
{

    public partial class frm_Main : Form
    {


        SqlConnection cnn = new SqlConnection();
        int hasError = 0;
        int hasFileLoadError = 0;
        FileInfo[] files = default(FileInfo[]);
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

            //lv_fileList.
        }

        private void setJsonObjectConfig()
        {
            var enviroment = System.Environment.CurrentDirectory;
            //if (File.Exists(@"" + Directory.GetParent(enviroment).Parent.FullName.ToString().Replace(".dll", "") + @"\config.json"))
            //{
            //    this.lconfig = JsonSerializer.Deserialize<LocalConfig>(File.ReadAllText(@"" + Directory.GetParent(enviroment).Parent.FullName.ToString().Replace(".dll", "") + @"\config.json"));
            //}
            if (File.Exists(this.cfgJsonFile))
            {
                this.lconfig = JsonSerializer.Deserialize<LocalConfig>(File.ReadAllText(this.cfgJsonFile));
            }

            txtbox_FinalTableName.Text = "Original_";
            txtbox_tablePrefix.Text = "Original";

            //txtbox_FinalTableName.Text = this.lconfig.tableName.ToString();
            //txtbox_tablePrefix.Text = this.lconfig.tablePrefix.ToString();

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
                this.cnn.Close();

            }
            catch (Exception err)
            {
                lbl_testConnStatus.ForeColor = Color.Red;
                lbl_testConnStatus.Text = err.Message.ToString();
                this.hasError = 1;
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

        public void addAFileToLoad(string file, DataTable dt)
        {
            this.filesToLoad.Add(file, dt);
        }
        public void addAFailedFileToLoad(string file, DataTable dt)
        {
            this.filesToLoad.Add(file, dt);
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
            this.hasFileLoadError = 0;

            
            if (txtbox_pickUpPath.Text.ToString() == "" || Directory.Exists(txtbox_pickUpPath.Text.ToString()) == false)
            {
                MessageBox.Show("I am sorry your folder path is either blank or does not exist");
                this.hasFileLoadError = 1;
                return;
            }

            /*
             * Clear Files List
             */
            lv_fileList.Items.Clear();
            dgv_FieldList.Rows.Clear();

            string filesPath = txtbox_pickUpPath.Text;
            string fileDelimiter = cmbox_delimiter.SelectedItem.ToString();

            pf = new parseFile(filesPath, fileDelimiter);

            btn_loadFilesToList.Enabled = false;
            btn_loadFilesToList.Text = "Loading...";

            await pf.ldFiles();

            this.fileListLoaded = true;

            // SHOULD BE Counted. But doesn't work.
            MessageBox.Show(this.lv_fileList.Items.Count.ToString());
            foreach (ListViewItem lv in lv_fileList.Items)
            {
                this.updateFileMapping(sender, e, lv.Index);
                
                lv_fileList.Items[lv.Index].Checked = true;
            }
            if (lv_fileList.Items.Count != 0)
            {
                lv_fileList.Items[0].Selected = true;
                lv_fileList.Items[0].Focused = true;
            }

            lv_fileList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            btn_loadFilesToList.Enabled = true;
            btn_loadFilesToList.Text = "Load Files";
            dgv_FieldList.Enabled = true;
            lv_fileList.Enabled = true;
            btn_loadToSQL.Enabled = true;


            //Thread lfThread = new Thread(() => ldFiles(new DirectoryInfo(), sender, e));
            //lfThread.Start();
            //Task.Factory.StartNew(/*(*/) => );
            // public void ldFiles (string puPath,FileInfo[] tfiles, ListView lvfiles, Dictionary<string, DataTable> ftLoad, DataGridView dgvfl )
        }

        frm_loadStatus ltDialog = new frm_loadStatus();

        private void btn_loadToSQL_Click(object sender, EventArgs e)
        {   
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
            this.ltDialog.Text = txtbox_sqlServer.Text;
            this.ltDialog.Show();
            this.ltDialog.setLoadingText(Color.Black, @"Trying to get your latest incrementing table");
            Thread ltsThread = new Thread(() => loadTablesToSQLServer(
                chbox_tablePrefix.Checked
                , txtbox_tablePrefix.Text
                , txtbox_FinalTableName.Text
                //, this.filesToLoad
                , chbox_DropTables.Checked
                , chbox_CreateAllTablesTable.Checked
                , this.filesToLoadMapping
                , this.appFolder
            ));
            ltsThread.Start();
        }
        public void updateSQLLoadDialogText (string stToLoad)
        {
            this.ltDialog.Text = stToLoad;
        }

        public void updateStatusTextLoadSQL (Color clr, string strToLoad)
        {
            this.ltDialog.setLoadingText( clr, strToLoad);
        }
        
        public void updateSQLLoadProgress (decimal prg)
        {
            this.ltDialog.setLoadStatus(prg);
        }

        public void finishedLoadingToSQLSteps ()
        {
            this.ltDialog.showCancelButton();
            this.btn_loadToSQL.Text = "Load To SQL";
            this.btn_loadToSQL.Enabled = true;
            this.btn_loadFilesToList.Enabled = true;
        }

        public void showOkLTSButton ()
        {
            this.ltDialog.showCancelButton();
        }

        public Dictionary<string, DataTable> getfilesToLoadDic ()
        {
            Dictionary<string, DataTable> ftld = new Dictionary<string, DataTable> ();
            foreach (System.Collections.Generic.KeyValuePair<string, System.Data.DataTable> fl in this.filesToLoad)
            {
                ftld.Add(fl.Key, fl.Value);
            }
            return ftld;
            
        }

        public List<ListViewItem> getLvFiles ()
        {
            List<ListViewItem> lv = new List<ListViewItem> ();
            foreach(ListViewItem lvi in this.lv_fileList.CheckedItems)
            {
                lv.Add(lvi);
            }
            return lv;
        }

        // TODO: Need to add Checks for Drop Table and for Create Unioned Table
        // TODO: Need to add more validation
        private void loadTablesToSQLServer(
            Boolean useTablePrefix
            , string tblPrefix
            , string finalTblName
            //, Dictionary<string, DataTable> ftl
            , Boolean DropTablesOrNot
            , Boolean CreateMasterTable
            , Dictionary<string, Dictionary<string, Dictionary<string, string>>> ftlm
            , string AppFolderLocation
            )
        {
            List<ListViewItem> filesLV = (List<ListViewItem>)Invoke(new Func<List<ListViewItem>>(getLvFiles));
            Dictionary<string, DataTable> ftl = (Dictionary<string, DataTable>)Invoke(new Func<Dictionary<string, DataTable>>(getfilesToLoadDic));

            int fCount = filesLV.Count;
            int fCnt = 1;
            
            string lastTableName;
            int lastTableNumberOriginal = 1;
            int lastTableNumber = 1;
            string tablePrefix = "";


            decimal currentProg = (decimal)((double)fCnt / fCount * 100);

            if (useTablePrefix)
            {
                tablePrefix = tblPrefix;
                try
                {
                    // prepare query for getting the latest incrementing table name
                    string query = @"SELECT * FROM (SELECT MAX(t.name) AS LastTable, RIGHT(MAX(T.name),3) AS LastTableNumber FROM sys.tables t WHERE name LIKE '" + tablePrefix + @"[0-9]%') a WHERE lastTable IS NOT null";
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand(query, this.cnn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();

                    // fill our DataAdapter from the DataSet retrieved
                    // then we will check to see if the DataSet is empty which means it's a new Incrementing name the user wants to use. So we'll create the first Record Incrementer for that user Current Limit (999)
                    // ToDo: Set Warning for more than 999 files and or create a way to dynamically handle it. 
                    da.Fill(ds);
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            lastTableName = ds.Tables[0].Rows[0]["LastTable"].ToString();
                            lastTableNumberOriginal = Int32.Parse(ds.Tables[0].Rows[0]["LastTableNumber"].ToString()) + 1;
                        }
                        else
                        {
                            lastTableName = tblPrefix;
                            lastTableNumberOriginal = Int32.Parse("000") + 1;
                        }
                    }
                    else
                    {
                        lastTableName = tblPrefix;
                        lastTableNumberOriginal = Int32.Parse("000") + 1;
                    }
                    this.cnn.Close();
                }
                // Something happened. Close the connection and show the error in a message box. 
                // if the user is using a very old version of SQL that doesn't have sys catalogs. Or SQL Info is just bad.
                catch (Exception ex)
                {
                    if (this.cnn.State == ConnectionState.Open) { cnn.Close(); }
                    MessageBox.Show(ex.Message.ToString());
                    return;
                }
            }
            lastTableNumber = lastTableNumberOriginal;
            
            Boolean errorsHappened = false;
            foreach (ListViewItem lii in filesLV)
            {
                Invoke(new Action<Color, String>(updateStatusTextLoadSQL), new object[] { Color.Black, @"Loading your files..." });
                DataTable dtToLoad;
                FileInfo fi = FileSystem.GetFileInfo(lii.Text.ToString());
                string fName = fi.Name.ToString().Replace(".txt", "").Replace(".csv", "");
                string tNameToInsert = useTablePrefix ? tablePrefix + lastTableNumber.ToString().PadLeft(3, '0') : fName;

                Invoke(new Action<string>(updateSQLLoadDialogText), "Loading Table " + tNameToInsert);
                Invoke(new Action<Color, String>(updateStatusTextLoadSQL), new object[] { Color.Black, @"Uploading " + fName });

                Invoke(new Action<decimal>(updateSQLLoadProgress), new object[] { currentProg });
                

                try
                {

                    dtToLoad = ftl[lii.Text.ToString()];
                    if (DropTablesOrNot == true)
                    {
                        string dropTable = "if exists(select 1 FROM sys.tables where name = '" + tNameToInsert + "') BEGIN DROP TABLE [" + tNameToInsert + "]; END";
                        SqlCommand cmdDT = new SqlCommand(dropTable, this.cnn);
                        cmdDT.Connection.Open(); cmdDT.ExecuteNonQuery(); cmdDT.Connection.Close();
                        cmdDT.Dispose();
                    }


                    string createTable = "CREATE TABLE [" + tNameToInsert + "] (";
                    Dictionary<string, Dictionary<string, string>> fd = ftlm[fi.FullName.ToString()];

                    int cLength = dtToLoad.Columns.Count;
                    for (int i = 0; i < cLength; i++)
                    {
                        Dictionary<string, string> dd = fd[dtToLoad.Columns[i].Ordinal.ToString()];
                        string dtstr = dd["dataType"].ToString() == "Text" ? "varchar" : dd["dataType"].ToString();
                        if (dd["dataType"].ToString() == "Text")
                        {
                            dtstr += "(" + dd["dtLength"].ToString() + ")";
                        }
                        //this.filesToLoadMapping[dtToLoad.Columns[i].ToString()]

                        if (i == cLength - 1)
                        {
                            createTable += "[" + dtToLoad.Columns[i].ToString() + "] " + dtstr;
                        }
                        else
                        {
                            createTable += "[" + dtToLoad.Columns[i].ToString() + "]  " + dtstr + ",";
                        }
                    }
                    createTable += ") ";
                    System.Diagnostics.Debug.WriteLine(createTable);
                    SqlCommand cmd2 = new SqlCommand(createTable, this.cnn);
                    cmd2.Connection.Open(); cmd2.ExecuteNonQuery(); cmd2.Connection.Close();
                    cmd2.Dispose();

                    this.cnn.Open();
                    using (SqlBulkCopy s = new SqlBulkCopy(cnn))
                    {
                        string abc = "dbo.[" + tNameToInsert + "]";
                        s.DestinationTableName = abc;
                        s.BulkCopyTimeout = 0;
                        s.WriteToServer(dtToLoad);
                    }
                    this.cnn.Close();
                    Invoke(new Action<Color, String>(updateStatusTextLoadSQL), new object[] { Color.Green, @"Loaded" + fi.FullName });

                    //InsertDataIntoSQLServerUsingSQLBulkCopy(dtToLoad, tToInsert, cnn);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message.ToString());
                    cnn.Close();
                    Invoke(new Action<Color, String>(updateStatusTextLoadSQL), new object[] { Color.Red, ex.Message });
                    errorsHappened = true;
                }
                fCnt++;
                lastTableNumber++;
                currentProg = (decimal)((double)fCnt / fCount * 100) > 100 ? 100 : (decimal)((double)fCnt / fCount * 100);
                Invoke(new Action<decimal>(updateSQLLoadProgress), new object[] { currentProg });

            }



            if (CreateMasterTable == false)
            {
                Invoke(new Action<Color, String>(updateStatusTextLoadSQL), new object[] { errorsHappened ? Color.Red : Color.Black, errorsHappened ? @"We loaded the tables we could, but had some errors" : @"Your Tables have Been Loaded " });
                Invoke(new Action<String>(updateSQLLoadDialogText), new object[] { errorsHappened ? @"Finished (With Errors)" : @"Finished Loading " });
                Invoke(new Action<decimal>(updateSQLLoadProgress), new object[] { new decimal(100) });
                Invoke(new Action(finishedLoadingToSQLSteps));
                return;
            }

            lastTableNumber = lastTableNumberOriginal;
            //foreach (System.Collections.Generic.KeyValuePair<string, DataTable> fl in this.filesToLoad) {


            //} // end foreach


            if (CreateMasterTable == true)
            {
                string finalTable = finalTblName.ToString();
                try
                {
                    Invoke(new Action<decimal>(updateSQLLoadProgress), new object[] { new decimal(90) });
                    Invoke(new Action<Color, String>(updateStatusTextLoadSQL), new object[] { Color.Black, "Trying to create your Unioned Table \"" + finalTblName.ToString() + " \"" });

                    if (DropTablesOrNot == true)
                    {
                        string dropTable = "if exists(select 1 FROM sys.tables where name = '" + finalTable + "') BEGIN DROP TABLE [" + finalTable + "]; END";
                        SqlCommand cmdDT = new SqlCommand(dropTable, this.cnn);
                        cmdDT.Connection.Open(); cmdDT.ExecuteNonQuery(); cmdDT.Connection.Close();
                        cmdDT.Dispose();
                    }

                    cnn.Open();

                    string script = File.ReadAllText(AppFolderLocation + @"\SQL Scripts\DropOriginalTableCustomFunctions.sql");
                    SqlCommand cmScripts = new SqlCommand(script, cnn);
                    cmScripts.ExecuteNonQuery();
                    script = File.ReadAllText(AppFolderLocation + @"\SQL Scripts\OriginalTableList.sql");
                    cmScripts.CommandText = script;
                    cmScripts.ExecuteNonQuery();
                    script = File.ReadAllText(AppFolderLocation + @"\SQL Scripts\get_originalUnion.sql");
                    cmScripts.CommandText = script;
                    cmScripts.ExecuteNonQuery();
                    cnn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                    cnn.Close();
                    Invoke(new Action<decimal>(updateSQLLoadProgress), new object[] { new decimal(100) });
                    Invoke(new Action<Color, String>(updateStatusTextLoadSQL), new object[] {Color.Black, "Completed (With Errors)"});
                    Invoke(new Action(finishedLoadingToSQLSteps));
                    return;
                }


                Invoke(new Action<decimal>(updateSQLLoadProgress), new object[] { new decimal(80) });

                Dictionary<string, string> tListLoaded = new Dictionary<string, string>();
                try
                {

                    string qToUnion = @"
                    DECLARE @tvalues OriginalTableList ";
                    qToUnion += useTablePrefix ? @"
                    INSERT INTO @tvalues (tbl, srcFile) VALUES " : @"
                    INSERT INTO @tvalues(tbl) VALUES ";
                    int itemsAddedCount = filesLV.Count;
                    int itemsAddedCounter = 1;
                    foreach (ListViewItem lii in filesLV)
                    {
                        FileInfo fl = FileSystem.GetFileInfo(lii.Text.ToString());
                        string fName = fl.Name.ToString().Replace(".txt", "").Replace(".csv", "");
                        string tName = useTablePrefix == true ? tablePrefix + lastTableNumber.ToString().PadLeft(3, '0') : fName;
                        if (itemsAddedCounter != itemsAddedCount)
                        {
                            qToUnion +=
                                useTablePrefix ? @" ('" + tName + @"', '" + fName + "'), "
                                : @" ('" + fName + @"'), ";

                        }
                        else
                        {
                            qToUnion +=
                            useTablePrefix ? @" ('" + tName + @"', '" + fName + "') "
                            : @" ('" + fName + @"') ";
                        }
                        if (useTablePrefix)
                        {
                            tListLoaded.Add(fl.FullName.ToString(), tName);
                        }
                        lastTableNumber++;
                        itemsAddedCounter++;
                    }



                    qToUnion += Environment.NewLine;
                    string nTName = finalTable;
                    qToUnion += @"EXEC dbo.get_OriginalUnion @tvalues, '" + nTName + "'";

                    SqlCommand cmd3 = new SqlCommand(qToUnion, cnn);
                    cmd3.Connection.Open(); cmd3.ExecuteNonQuery(); cmd3.Connection.Close();
                    cmd3.Dispose();
                    //string ChangeOriginalTableToFile = @"EXEC sp_rename 'dbo."+finalTable+".OriginalTable', 'OriginalFile', 'COLUMN'; ";
                    //SqlCommand cmd4 = new SqlCommand(ChangeOriginalTableToFile, cnn);
                    //cmd4.Connection.Open(); cmd4.ExecuteNonQuery(); cmd4.Connection.Close();
                    //cmd4.Dispose();

                    Invoke(new Action<Color, String>(updateStatusTextLoadSQL), new object[] { Color.Black, @"You Now Have A New Table " + Environment.NewLine + nTName });
                    Invoke(new Action<decimal>(updateSQLLoadProgress), new object[] { new decimal(100) });
                    
                }
                catch (Exception em)
                {
                    MessageBox.Show(em.Message.ToString());
                    Invoke(new Action(showOkLTSButton));
                    Invoke(new Action(finishedLoadingToSQLSteps));
                }

            }
            // SETUP Scripts So that SQL Can be updated for union table and used to create Union Table
            Invoke(new Action(finishedLoadingToSQLSteps));
            
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
        private void updateFileMapping(object sender, EventArgs e, int idx)
        {

            dgv_FieldList.Rows.Clear();
            int ci = idx == -1 ? lv_fileList.SelectedIndex() : idx;
            FileInfo fl = this.files[ci];
            if (!this.filesToLoad.ContainsKey(fl.FullName.ToString()))
            {
                return;
            }
            DataTable dt = this.filesToLoad[fl.FullName.ToString()];
            Dictionary<string, Dictionary<string, string>> ftm = null;
            if (this.filesToLoadMapping.ContainsKey(fl.FullName.ToString()))
            {
                ftm = this.filesToLoadMapping[fl.FullName.ToString()];
            }

            if (ftm == null)
            {
                // Mapping Doesn't exist prefil will generic mapping
                foreach (DataColumn col in dt.Columns)
                {
                    dgv_FieldList.Rows.Add(
                        col.Ordinal.ToString()
                        , col.ColumnName.ToString()
                        , "Text"
                        , txtbox_defaultDataLength.Text.ToString()
                    );
                }
                this.dgv_FieldList_CellValueChanged_Call(sender, null, ci);
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
            this.updateFileMapping(sender, e, -1);
        }

        private void dgv_FieldList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            this.dgv_FieldList_CellValueChanged_Call(sender, e, -1);
        }
        private void updateMappingForFile(int idx)
        {
            if (this.files != null)
            {
                FileInfo fl = this.files[idx];
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
            int idx = ci == -1 ? lv_fileList.SelectedIndex() : ci;
            if (this.fileListLoaded)
            {
                if (e == null)
                {
                    this.updateMappingForFile(idx);
                    return;
                }
                if (e.RowIndex != 0)
                {
                    int ri = e.RowIndex;
                    //MessageBox.Show(ri.ToString());
                    DataGridViewRow dgvr = dgv_FieldList.Rows[e.RowIndex];
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
    }

    //class ListViewItemComparer : IComparer
    //{
    //    private int col;
    //    public ListViewItemComparer()
    //    {
    //        col = 0;
    //    }
    //    public ListViewItemComparer(int column)
    //    {
    //        col = column;
    //    }
    //    public int Compare(object x, object y)
    //    {
    //        // 	somestring.Substring(somestring.Length-nCount,nCount)
    //        int cntOfPad = 20;
    //        string str1 = ((ListViewItem)x).SubItems[col].Text;
    //        string str2 = ((ListViewItem)y).SubItems[col].Text;
    //        if(int.TryParse(str1, out int result) && int.TryParse(str2, out int result1))
    //        {
    //            str1 = str1.Substring(str1.PadLeft(cntOfPad, '0').Length - cntOfPad, cntOfPad);
    //            str2 = str2.Substring(str2.PadLeft(cntOfPad, '0').Length - cntOfPad, cntOfPad);
    //        }

    //        return String.Compare(str1, str2);


    //    }
    //}
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
