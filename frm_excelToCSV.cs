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
using System.Threading;

namespace BulkImportDelimitedFlatFiles
{
    public partial class frm_excelToCSV : Form
    {
        public frm_excelToCSV()
        {
            InitializeComponent();
            cmbox_delimiter.SelectedItem = "Tab";
        }
        //FileInfo[] files;
        public static void showErrorMessages(List<string> errorMessages, string title)
        {
            string eMsg = "";
            foreach (var msg in errorMessages.Select((name, index) => (name, index))) {
                eMsg += (msg.index + 1).ToString() + ". " + msg.name.ToString() + Environment.NewLine;
            }
            MessageBox.Show(eMsg, title);

        }

        public void updateMainThreadFiles()
        {

        }

        public void setLblStatusUpdate(Color clr, string str)
        {
            this.lbl_fileLoadStatus.Visible = true;
            this.lbl_fileLoadStatus.ForeColor = clr;
            this.lbl_fileLoadStatus.Text = str;
            this.lbl_fileLoadStatus.Refresh();
        }

        public string getDelimiter ()
        {
            return cmbox_delimiter.SelectedItem.ToString();
        }

        public void cvtExcelFiles (string eFolder, string cfolder)
        {
            List<string> errorMessages = new List<string>();
            if (eFolder == "" || !Directory.Exists(eFolder))
            {
                errorMessages.Add("I am sorry you have either left the excel folder blank or it does not exist");
            }
            if (cfolder == "" || !Directory.Exists(cfolder))
            {
                errorMessages.Add("I am sorry you have either left the CSV folder blank or it does not exist");
            }
            if (errorMessages.Count > 0)
            {
                showErrorMessages(errorMessages, "Cannot Convert Files: Error");
                return;
            }

            try
            {
                Invoke(new Action<Color, string>(setLblStatusUpdate), new object[] { Color.Black, @"Starting to load your files" });

                DirectoryInfo di = new DirectoryInfo(eFolder);
                string[] extensions = new[] { ".xls", ".xlsx" };

                //Invoke(new Action(testFilesClearIfNeeded));
                FileInfo[] files;

                files =
                    di.EnumerateFiles()
                         .Where(f => extensions.Contains(f.Extension.ToLower()))
                         .ToArray();

                List<string> failedFiles = new List<string>();
                foreach (FileInfo fl in files)
                {
                    Invoke(new Action<string, string, FileInfo[]>(convertExcelFiles), new object[] { fl.FullName.ToString(), cfolder, files });
                }

                Invoke(new Action<Color, string>(setLblStatusUpdate), new object[] { Color.Green , @"Files Successfully Loaded" });
                MessageBox.Show("Files Loaded to Directory \"" + cfolder + "\"");
                System.Diagnostics.Process.Start("explorer.exe", cfolder);

            }
            catch (Exception em)
            {
                Invoke(new Action<Color, string>(setLblStatusUpdate), new object[] { Color.Red, @"Files Failed To Load" });
                lbl_fileLoadStatus.ForeColor = Color.Red;
                MessageBox.Show("Error in Converting Files: " + em.Message);
            }
        }

        private void btn_convertExceltoCSV_Click(object sender, EventArgs e)
        {
            string excelFolder = txtbox_excelFolder.Text.ToString();
            string csvFolder = txtbox_csvFolder.Text.ToString();

            Thread cvtFilesThread = new Thread(() => cvtExcelFiles(txtbox_excelFolder.Text.ToString(), txtbox_csvFolder.Text.ToString()));
            cvtFilesThread.Start();
            
        }


        private void convertExcelFiles(string path, string dropPath, FileInfo[] files)
        {
            try {
                
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                FileInfo fi = FileSystem.GetFileInfo(path);
                Invoke(new Action<Color, string>(setLblStatusUpdate), new object[] { Color.Black, "Loading File " + fi.Name.ToString() });
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
                    Invoke(new Action<Color, string>(setLblStatusUpdate), new object[] { Color.Black, "Loading Table " + t.TableName + " for file " + fi.Name.ToString() });
                    string fname = fi.Name.ToString().Replace(".xlsx", "").Replace(".xls", "") + "_" + t.TableName + ".csv";
                    string finalPath = dropPath + "\\" + fname;

                    Invoke(new Action<DataTable, string, string>(ToCSV), new object[] { t, finalPath, fname});
                }
            } catch (Exception em) {
                MessageBox.Show("Error in method convertExcelFiles" + Environment.NewLine + Environment.NewLine + em.Message);
            }
            
        }

        // TODO: Need to Add Custom Delimiter
        private void ToCSV(DataTable dtDataTable, string strFilePath, string fname)
        {
            try {
                string cusDelimiter = (string)Invoke(new Func<string>(getDelimiter));
                string cdelim;
                switch (cusDelimiter)
                {
                    case "Tab":
                        cdelim = "\t";
                        break;
                    case "Comma":
                        cdelim = ",";
                        break;
                    case "Semi-Colon":
                        cdelim = ";";
                        break;
                    case "Pipe":
                        cdelim = "|";
                        break;
                    default:
                        cdelim = "\t";
                        break;
                }
                Invoke(new Action<Color, string>(setLblStatusUpdate), new object[] { Color.Black, "Writing file " + fname });
                var builder = new StringBuilder();
                int colCnt = 0;
                foreach(DataColumn dc in dtDataTable.Columns) {
                    builder.Append(dc.ColumnName.ToString().Replace("\"",""));
                    if (colCnt != (dtDataTable.Columns.Count-1)) {
                        //builder.Append("\t");
                        builder.Append(cdelim);
                    }
                    colCnt++;
                }
                builder.Append(Environment.NewLine);

                foreach (DataRow row in dtDataTable.Rows) {
                    builder.AppendLine(String.Join(cdelim, row.ItemArray));
                }

                File.WriteAllText(strFilePath, builder.ToString());
            } catch (Exception em) {
                MessageBox.Show("Error in toCSV Method" + Environment.NewLine + Environment.NewLine + em.Message);
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

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
