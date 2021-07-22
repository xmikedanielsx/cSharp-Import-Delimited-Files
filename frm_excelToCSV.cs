using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic.FileIO;



namespace BulkImportDelimitedFlatFiles
{
    public partial class frm_excelToCSV : Form
    {
        public frm_excelToCSV()
        {
            InitializeComponent();
        }
        FileInfo[] files;

        private void btn_convertExceltoCSV_Click(object sender, EventArgs e)
        {
            string excelFolder = txtbox_excelFolder.Text.ToString();
            string csvFolder = txtbox_csvFolder.Text.ToString();
            if (excelFolder == "" || !Directory.Exists(excelFolder)) {
                MessageBox.Show("I am sorry you have either left the excel folder blank or it does not exist");
            }
            if (csvFolder == "" || !Directory.Exists(csvFolder)) {
                MessageBox.Show("I am sorry you have either left the CSV folder blank or it does not exist");
            }

            try {
                lbl_fileLoadStatus.ForeColor = Color.Black;
                lbl_fileLoadStatus.Text = "Starting to load your files";
                DirectoryInfo di = new DirectoryInfo(excelFolder);
                string[] extensions = new[] { ".xls", ".xlsx" };

                if (this.files != null) { Array.Clear(this.files, 0, this.files.Length); }
                this.files =
                    di.EnumerateFiles()
                         .Where(f => extensions.Contains(f.Extension.ToLower()))
                         .ToArray();

                List<string> failedFiles = new List<string>();
                foreach (FileInfo fl in this.files) {
                    this.convertExcelFiles(fl.FullName.ToString(), csvFolder);
                }

                lbl_fileLoadStatus.ForeColor = Color.Green;
                lbl_fileLoadStatus.Text = "Files Successfully Loaded";
                lbl_fileLoadStatus.Refresh();
                MessageBox.Show("Files Loaded to Directory \"" + csvFolder + "\"");

            }
            catch (Exception em) {
                lbl_fileLoadStatus.ForeColor = Color.Red;
                lbl_fileLoadStatus.Text = "Files Failed To Load";
                lbl_fileLoadStatus.Refresh();
                MessageBox.Show("Error in Converting Files: " + em.Message);
            }
        }


        private void convertExcelFiles(string path, string dropPath)
        {
            try {
                
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                FileInfo fi = FileSystem.GetFileInfo(path);
                lbl_fileLoadStatus.Text = "Loading File " + fi.Name.ToString();
                lbl_fileLoadStatus.Refresh();
                var stream = File.Open(path, FileMode.Open, FileAccess.Read);
                var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);
                var result = ExcelDataReader.ExcelDataReaderExtensions.AsDataSet(reader,
                    new ExcelDataReader.ExcelDataSetConfiguration() {
                        ConfigureDataTable = (_) => new ExcelDataReader.ExcelDataTableConfiguration() {
                            UseHeaderRow = true
                        }
                    });
                var tables = result.Tables.Cast<DataTable>();
                foreach (DataTable t in tables) {
                    lbl_fileLoadStatus.Text = "Loading Table " + t.TableName + " for file " + fi.Name.ToString();
                    lbl_fileLoadStatus.Refresh();
                    string fname = fi.Name.ToString().Replace(".xlsx", "").Replace(".xls", "") + "_" + t.TableName + ".csv";
                    string finalPath = dropPath + "\\" + fname;
                    //MessageBox.Show("Trying to write DataTable \"" + t.TableName + "\"");

                    this.ToCSV(t, finalPath, fname);
                }
            } catch (Exception em) {
                MessageBox.Show("Error in method convertExcelFiles --- " + em.Message);
            }
            
        }

        // TODO: Need to Add Custom Delimiter
        private void ToCSV(DataTable dtDataTable, string strFilePath, string fname)
        {
            try {
                lbl_fileLoadStatus.Text = "Writing file " + fname;
                lbl_fileLoadStatus.Refresh();
                //var builder = new StringBuilder();
                //foreach (DataRow row in dtDataTable.Rows) {
                //    builder.AppendLine(String.Join("\t", row.ItemArray));
                //}
                //File.WriteAllText(strFilePath, builder.ToString());
                var builder = new StringBuilder();
                int colCnt = 0;
                foreach(DataColumn dc in dtDataTable.Columns) {
                    //builder.Append("\"");
                    builder.Append(dc.ColumnName.ToString().Replace("\"",""));
                    //builder.Append("\"");
                    if (colCnt != (dtDataTable.Columns.Count-1)) {
                        builder.Append("\t");
                    }
                    colCnt++;
                }
                builder.Append(Environment.NewLine);

                foreach (DataRow row in dtDataTable.Rows) {
                    builder.AppendLine(String.Join("\t", row.ItemArray));
                }

                //foreach (DataRow row in dtDataTable.Rows) {
                //    int cellCnt = 0;
                //    foreach (var cell in row.ItemArray) {
                //        builder.Append("\"");
                //        builder.Append(cell.ToString().Replace("\"", ""));
                //        builder.Append("\"");
                //        if (cellCnt != (row.ItemArray.Count())) {
                //            builder.Append("\t");
                //        } 
                //    }
                //    builder.Append(Environment.NewLine);
                //}

                File.WriteAllText(strFilePath, builder.ToString());
            } catch (Exception em) {
                MessageBox.Show("Error in toCSV Method ---- " + em.Message);
            }
            
        }

        private void btn_excelPickUp_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            DialogResult dr = fd.ShowDialog();
            if (dr == DialogResult.OK) {
                txtbox_excelFolder.Text = fd.SelectedPath.ToString();
            }
        }

        private void btn_csvDropOff_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            DialogResult dr = fd.ShowDialog();
            if (dr == DialogResult.OK) {
                txtbox_csvFolder.Text = fd.SelectedPath.ToString();
            }
        }
    }
}
