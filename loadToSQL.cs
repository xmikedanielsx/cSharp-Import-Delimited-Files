using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace BulkImportDelimitedFlatFiles
{
    internal class loadToSQL
    {
        private readonly bool useTablePrefix;
        private readonly string tablePrefix;
        private readonly string finalTblName;
        private readonly bool dropTablesOrNot;
        private readonly bool createMasterTable;
        private Dictionary<string, Dictionary<string, Dictionary<string, string>>> ftlm;
        private string appFolderLocation;
        private ListView pFileList = new ListView();
        private readonly Dictionary<string, DataTable> pFilesToLoad;
        private SqlConnection cnn;
        private readonly frm_loadStatus ltDialog;

        public loadToSQL (
            Boolean useTablePrefix
            , string tablePrefix
            , string finalTblName
            , Boolean DropTablesOrNot
            , Boolean CreateMasterTable
            , Dictionary<string, Dictionary<string, Dictionary<string, string>>> ftlm
            , string AppFolderLocation
            , ListView pFileList
            , Dictionary<string, DataTable> pFilesToLoad
            , SqlConnection cnn
            , frm_loadStatus ltDialog
        )
            
        {
            this.useTablePrefix = useTablePrefix;
            this.tablePrefix = tablePrefix;
            this.finalTblName = finalTblName;
            dropTablesOrNot = DropTablesOrNot;
            createMasterTable = CreateMasterTable;
            this.ftlm = ftlm;
            appFolderLocation = AppFolderLocation;
            //this.pFileList = pFileList;
            this.pFilesToLoad = pFilesToLoad;
            this.cnn = cnn;
            this.ltDialog = ltDialog;
            foreach (ListViewItem lvi in pFileList.Items)
            {
                this.pFileList.Items.Add(lvi.Text);
            }
        }

        private string[] getLastTableNumber()
        {
            int lastTableNumberOriginal = Int32.Parse("000") + 1; ;
            string lastTableName = this.tablePrefix;
            string[] returnVal = new string[2];
            if (useTablePrefix)
            {
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
                    }
                    else
                    {
                        lastTableName = tablePrefix;
                        lastTableNumberOriginal = Int32.Parse("000") + 1;
                    }
                    this.cnn.Close();
                    returnVal[0] = lastTableNumberOriginal.ToString();
                    returnVal[1] = lastTableName;
                }
                // Something happened. Close the connection and show the error in a message box. 
                // if the user is using a very old version of SQL that doesn't have sys catalogs. Or SQL Info is just bad.
                catch (Exception ex)
                {
                    if (this.cnn.State == ConnectionState.Open) { cnn.Close(); }
                    throw new Exception(ex.Message);
                }
            } else {
                returnVal[0] = lastTableNumberOriginal.ToString();
                returnVal[1] = lastTableName;
            }
            
            return returnVal;
        }

        private void dropTable (string tNameToInsert)
        {
            string dropTable = "if exists(select 1 FROM sys.tables where name = '" + tNameToInsert + "') BEGIN DROP TABLE [" + tNameToInsert + "]; END";
            SqlCommand cmdDT = new SqlCommand(dropTable, this.cnn);
            cmdDT.Connection.Open(); cmdDT.ExecuteNonQuery(); cmdDT.Connection.Close();
            cmdDT.Dispose();
        }

        public Task loadTablesToSQLServer()
        {
            return Task.Run(() => {

            
                List<ListViewItem> filesLV = new List<ListViewItem>();
                foreach (ListViewItem lvi in pFileList.Items)
                {
                    filesLV.Add(lvi);
                }

                Dictionary<string, DataTable> ftl = new Dictionary<string, DataTable>();
                foreach (System.Collections.Generic.KeyValuePair<string, System.Data.DataTable> fl in pFilesToLoad)
                {
                    ftl.Add(fl.Key, fl.Value);
                }


                int fCount = filesLV.Count;
                int fCnt = 1;

                string lastTableName;
                int lastTableNumberOriginal = 1;
                int lastTableNumber = 1;


                decimal currentProg = (decimal)((double)fCnt / fCount * 100);

                string[] tableStuff = this.getLastTableNumber();
                lastTableNumberOriginal = Int32.Parse(tableStuff[0]);
                lastTableName = tableStuff[1];
                lastTableNumber = lastTableNumberOriginal;


                Boolean errorsHappened = false;
                foreach (ListViewItem lii in filesLV)
                {
                    ltDialog.Invoke((MethodInvoker)delegate { ltDialog.setLoadingText(Color.Black, @"Loading your files"); }); ;

                    FileInfo fi = FileSystem.GetFileInfo(lii.Text.ToString());
                    string fName = fi.Name.ToString().Replace(".txt", "").Replace(".csv", "");
                    string tNameToInsert = useTablePrefix ? this.tablePrefix + lastTableNumber.ToString().PadLeft(3, '0') : fName;

                    ltDialog.Invoke((MethodInvoker)delegate { ltDialog.Text = "Loading Table " + tNameToInsert; }); ;
                    ltDialog.Invoke((MethodInvoker)delegate { ltDialog.setLoadingText(Color.Black, @"Uploading " + fName); }); ;
                    ltDialog.Invoke((MethodInvoker)delegate { ltDialog.setLoadStatus(currentProg); }); ;


                    // drop the table
                    if (dropTablesOrNot == true) { this.dropTable(tNameToInsert); }

                    // create the table
                    this.createTable(tNameToInsert, pFilesToLoad, lii, fi);



                    fCnt++;
                    lastTableNumber++;
                    currentProg = (decimal)((double)fCnt / fCount * 100) > 100 ? 100 : (decimal)((double)fCnt / fCount * 100);
                    ltDialog.Invoke((MethodInvoker)delegate { ltDialog.setLoadStatus(currentProg); }); ;
                }



                if (createMasterTable == false)
                {
                    //Invoke(new Action<Color, String>(updateStatusTextLoadSQL), new object[] { errorsHappened ? Color.Red : Color.Black, errorsHappened ? @"We loaded the tables we could, but had some errors" : @"Your Tables have Been Loaded " });
                    //Invoke(new Action<String>(updateSQLLoadDialogText), new object[] { errorsHappened ? @"Finished (With Errors)" : @"Finished Loading " });
                    //Invoke(new Action<decimal>(updateSQLLoadProgress), new object[] { new decimal(100) });
                    //Invoke(new Action(finishedLoadingToSQLSteps));
                }

                lastTableNumber = lastTableNumberOriginal;
                //foreach (System.Collections.Generic.KeyValuePair<string, DataTable> fl in this.filesToLoad) {


                //} // end foreach


                if (createMasterTable == true)
                {
                    this.createTheMasterTable(filesLV, tablePrefix, lastTableNumber);

                }
            });
            // SETUP Scripts So that SQL Can be updated for union table and used to create Union Table
            //Invoke(new Action(finishedLoadingToSQLSteps));
        }
        private void createTheMasterTable(List<ListViewItem> filesLV, string tablePrefix, int lastTableNumber)
        {
            string finalTable = finalTblName.ToString();
            try
            {
                ltDialog.Invoke((MethodInvoker)delegate { ltDialog.setLoadStatus(new decimal(90)); });
                ltDialog.Invoke((MethodInvoker)delegate { ltDialog.setLoadingText(Color.Black, "Trying to create your Unioned Table \"" + finalTblName.ToString() + " \""); }); ;

                // drop final table if necessary
                if (dropTablesOrNot == true) { this.dropTable(finalTable); }

                cnn.Open();

                string script = File.ReadAllText(appFolderLocation + @"\SQL Scripts\DropOriginalTableCustomFunctions.sql");
                SqlCommand cmScripts = new SqlCommand(script, cnn);
                cmScripts.ExecuteNonQuery();
                script = File.ReadAllText(appFolderLocation + @"\SQL Scripts\OriginalTableList.sql");
                cmScripts.CommandText = script;
                cmScripts.ExecuteNonQuery();
                script = File.ReadAllText(appFolderLocation + @"\SQL Scripts\get_originalUnion.sql");
                cmScripts.CommandText = script;
                cmScripts.ExecuteNonQuery();
                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                cnn.Close();
                //Invoke(new Action<decimal>(updateSQLLoadProgress), new object[] { new decimal(100) });
                //Invoke(new Action<Color, String>(updateStatusTextLoadSQL), new object[] { Color.Black, "Completed (With Errors)" });
                //Invoke(new Action(finishedLoadingToSQLSteps));
                return;
            }


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

                ltDialog.Invoke((MethodInvoker)delegate { ltDialog.setLoadingText(Color.Black, @"You Now Have A New Table " + Environment.NewLine + nTName); }); ;
                ltDialog.Invoke((MethodInvoker)delegate { ltDialog.setLoadStatus(new decimal(100)); });

            }
            catch (Exception em)
            {
                MessageBox.Show(em.Message.ToString());
                ltDialog.Invoke((MethodInvoker)delegate { ltDialog.showOkButton(); });
                //Invoke(new Action(finishedLoadingToSQLSteps));
            }

        }
           

        public void createTable(string tNameToInsert, Dictionary<string, DataTable> pFilesToLoad, ListViewItem lii, FileInfo fi)
        {
            try
            {
                DataTable dtToLoad;
                
                dtToLoad = this.pFilesToLoad[lii.Text];
                string tfName = fi.Name;
                string tfNameClean = fi.Name.Replace(".csv", "").ToString();

                //DataColumn dcOFileName = new DataColumn("OriginalFileName", typeof(String));
                //dcOFileName.DefaultValue = tfName;
                //dtToLoad.Columns.Add(dcOFileName);

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
                ltDialog.Invoke((MethodInvoker)delegate { ltDialog.setLoadingText(Color.Green, @"Loaded" + fi.FullName); });

                //InsertDataIntoSQLServerUsingSQLBulkCopy(dtToLoad, tToInsert, cnn);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
                cnn.Close();
                ltDialog.Invoke((MethodInvoker)delegate { ltDialog.setLoadingText(Color.Black, ex.Message); }); ;
            }
        }

    }
}
