using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace ImportTabDelimitedFiles
{
    public partial class Form1 : Form
    {

     
        SqlConnection cnn = new SqlConnection();
        int hasError = 0;
        FileInfo[] files;
        Dictionary<string, DataTable> filesToLoad = new Dictionary<string, DataTable>();

        public Form1()
        {
            InitializeComponent();
            lbl_ExpandTopPanel.Visible = false;
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

            
            
            

            
           
            this.cnn.ConnectionString = cnnString;
            lbl_testConnStatus.ForeColor = Color.Black;
            lbl_testConnStatus.Text = "Trying to Connect...";
            try
            {
                this.cnn.Open();
                lbl_testConnStatus.ForeColor = Color.Green;
                lbl_testConnStatus.Text = "Connection Successful";
                this.cnn.Close();

            } catch (Exception err)
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
            if(dr == DialogResult.OK)
            {
                txtbox_pickUpPath.Text = fd.SelectedPath.ToString();
            }           
            
        }

        private void btn_verifyFileCount_Click(object sender, EventArgs e)
        {

            //this.btn_testConnection_Click(sender, e);
            if (this.hasError == 1) { return; }
            if (txtbox_pickUpPath.Text.ToString() == "" || Directory.Exists(txtbox_pickUpPath.Text.ToString()) == false)
            {
                MessageBox.Show("I am sorry your folder path is either blank or does not exist");
                this.hasError = 1;
            }
            if (this.hasError == 1) { return; }


            DirectoryInfo di = new DirectoryInfo(txtbox_pickUpPath.Text.ToString());
            string[] extensions = new[] { ".txt", ".csv" };

            if (this.files != null) { Array.Clear(this.files, 0, this.files.Length); }
            cbl_fileList.Items.Clear();
            this.files =
                di.EnumerateFiles()
                     .Where(f => extensions.Contains(f.Extension.ToLower()))
                     .ToArray();
            cbl_fileList.Items.AddRange(this.files);
            if (this.filesToLoad != null) { this.filesToLoad.Clear(); }
            foreach(FileInfo fl in this.files)
            {
                this.filesToLoad.Add(fl.FullName.ToString(), GetDataTableFromCSVFile(fl.FullName.ToString()));
            }

            //foreach(FileInfo fl in files)
            //{
            //    cbl_fileList.Items.Add(fl.Name.ToString());
            //}
            //foreach (FileInfo fl in files)
            //{
            //    string fname = fl.Name.ToString().Replace(".txt", "").Replace(".csv", "");
            //    //csvPairMatch.Add(fl.Name.ToString(), fname);
            //    //listbox_OriginalTablesWithCSVs.Items.Add("New Table To Create: [SR" + txtbox_lot_SRNumber.Text.ToString() + "_"+fname+"]");
            //    lbl_loadFilesStatus.ForeColor = Color.Black;
            //    lbl_loadFilesStatus.Text = "Loading your files...";
            //    try
            //    {
            //        DataTable dtToLoad;
            //        string tNameToInsert = fl.Name.ToString().Replace(".txt", "").Replace(".csv", "");
                   

            //        dtToLoad = GetDataTabletFromCSVFile(fl.FullName.ToString());
            //        string createTable = "CREATE TABLE [" + tNameToInsert + "] (";

            //        int cLength = dtToLoad.Columns.Count;
            //        for (int i = 0; i < cLength; i++)
            //        {
            //            if (i == cLength - 1)
            //            {
            //                createTable += "[" + dtToLoad.Columns[i].ToString() + "] varchar(MAX)";
            //            }
            //            else
            //            {
            //                createTable += "[" + dtToLoad.Columns[i].ToString() + "]  varchar(MAX),";
            //            }
            //        }
            //        createTable += ") ";
            //        System.Diagnostics.Debug.WriteLine(createTable);
            //        SqlCommand cmd2 = new SqlCommand(createTable, this.cnn);
            //        cmd2.Connection.Open(); cmd2.ExecuteNonQuery(); cmd2.Connection.Close();
            //        cmd2.Dispose();

            //        this.cnn.Open();
            //        using (SqlBulkCopy s = new SqlBulkCopy(cnn))
            //        {
            //            string abc = "dbo.[" + tNameToInsert + "]";
            //            s.DestinationTableName = abc;
            //            s.BulkCopyTimeout = 0;
            //            s.WriteToServer(dtToLoad);
            //        }
            //        this.cnn.Close();
            //        lbl_loadFilesStatus.ForeColor = Color.Green;
            //        lbl_loadFilesStatus.Text = "Your files have been loaded";


            //        //InsertDataIntoSQLServerUsingSQLBulkCopy(dtToLoad, tToInsert, cnn);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message.ToString());
            //        cnn.Close();
            //        lbl_loadFilesStatus.ForeColor = Color.Red;
            //        lbl_loadFilesStatus.Text = ex.Message;
            //    }

            //}

            //// SETUP Scripts So that SQL Can be updated for union table and used to create Union Table
            //try
            //{

            //    cnn.Open();
            //    var enviroment = System.Environment.CurrentDirectory;
            //    string currentLocationOfExe = Directory.GetParent(enviroment).Parent.FullName;

            //    string script = File.ReadAllText(@"" + currentLocationOfExe.ToString().Replace(".dll","") + @"\SQL Scripts\DropOriginalTableCustomFunctions.sql");
            //    SqlCommand cmScripts = new SqlCommand(script, cnn);
            //    cmScripts.ExecuteNonQuery();
            //    script = File.ReadAllText(@"" + currentLocationOfExe.ToString().Replace(".dll", "") + @"\SQL Scripts\OriginalTableList.sql");
            //    cmScripts.CommandText = script;
            //    cmScripts.ExecuteNonQuery();
            //    script = File.ReadAllText(@"" + currentLocationOfExe.ToString().Replace(".dll", "") + @"\SQL Scripts\get_originalUnion.sql");
            //    cmScripts.CommandText = script;
            //    cmScripts.ExecuteNonQuery();
            //    cnn.Close();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message.ToString());
            //    cnn.Close();
            //    return;
            //}

            
            //string qToUnion = @"
            //    DECLARE @tvalues OriginalTableList
            //    INSERT INTO @tvalues VALUES ";
            //int itemsAddedCount = files.Count();
            //int itemsAddedCounter = 1;
            //foreach (FileInfo fl in files)
            //{
            //    if (itemsAddedCounter != itemsAddedCount)
            //    {
            //        qToUnion += @" ('" + fl.Name.ToString().Replace(".txt", "").Replace(".csv", "") + @"'), ";
            //    }
            //    else
            //    {
            //        qToUnion += @" ('" + fl.Name.ToString().Replace(".txt", "").Replace(".csv", "") + @"') ";
            //    }
            //    itemsAddedCounter++;
            //}
            //qToUnion += Environment.NewLine;
            //string nTName = @"AllFilesTable";
            //qToUnion += @"EXEC dbo.get_OriginalUnion @tvalues, '" + nTName + "'";

            //SqlCommand cmd3 = new SqlCommand(qToUnion, cnn);
            //cmd3.Connection.Open(); cmd3.ExecuteNonQuery(); cmd3.Connection.Close();
            //cmd3.Dispose();
            //string ChangeOriginalTableToFile = @"EXEC sp_rename 'dbo.AllFilesTable.OriginalTable', 'OriginalFile', 'COLUMN'; ";
            //SqlCommand cmd4 = new SqlCommand(ChangeOriginalTableToFile, cnn);
            //cmd4.Connection.Open(); cmd4.ExecuteNonQuery(); cmd4.Connection.Close();
            //cmd4.Dispose();

            //MessageBox.Show(@"You Now Have A New Table " + Environment.NewLine + nTName);

        }

        private static DataTable GetDataTableFromCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters( "\t" );
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }
            return csvData;
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

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
            cbl_fileList.Items.Clear();
            dgv_FieldList.Rows.Clear();
            
            // Clear Other Labels
            lbl_LoadToSQLStatus.Text = "";
            lbl_loadFilesStatus.Text = "";
            
        }

        private void chbox_windowsAuth_CheckedChanged(object sender, EventArgs e)
        {
            if(this.chbox_windowsAuth.Checked == true)
            {
                txtbox_sqlUser.Enabled = false;
                txtbox_sqlPass.Enabled = false;
            } else
            {
                txtbox_sqlUser.Enabled = true;
                txtbox_sqlPass.Enabled = true;
            }
        }

        private void cbl_fileList_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgv_FieldList.Rows.Clear();
            FileInfo fl = this.files[cbl_fileList.SelectedIndex];
            DataTable dt = this.filesToLoad[fl.FullName.ToString()];
            foreach(DataColumn col in dt.Columns)
            {
                dgv_FieldList.Rows.Add(
                    col.Ordinal.ToString()
                    ,col.ColumnName.ToString()
                    ,"Text"
                    ,"max"
                );
            }
            





        }
    }
}
