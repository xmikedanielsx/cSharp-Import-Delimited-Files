using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;
using System.Drawing;

namespace BulkImportDelimitedFlatFiles
{
    class parseFile
    {
        private DirectoryInfo dirInfo;
        public FileInfo[] tfiles = new FileInfo[] { };
        private string fileDelmiter;
        private readonly Dictionary<string, DataTable> ptFilesToLoad;
        private readonly string[] extensions;
        public ListView iLV;
        // Class Constructor
        public parseFile(string dirLocation, string fileDelmiter, ListView ptFiles, Dictionary<string, DataTable> ptFilesToLoad, string[] extensions)
        {
            this.fileDelmiter = fileDelmiter;
            this.ptFilesToLoad = ptFilesToLoad;
            this.extensions = extensions;
            this.iLV = ptFiles;
            this.dirInfo = new DirectoryInfo(dirLocation);
            if (!dirInfo.Exists)
            {
                throw new Exception(@"Sorry but your directory does not exist or you do not have access");
            }
        }

        public FileInfo[] getFileList ()
        {
            return this.tfiles;
        }
        public ListView getListView ()
        {
            return this.iLV;
        }
        ListViewItem nlvi;
        DataTable dtfl;
        public Task ldFiles()
        {
            //MessageBox.Show("Made it to the func");
            return Task.Run(() =>
            {
                try
                {

                    // create local thread object to store the files
                    this.tfiles = dirInfo.EnumerateFiles().Where(f => extensions.Contains(f.Extension.ToLower())).ToArray();
                    // MessageBox.Show(tfiles[0].FullName.ToString());


                    // Create new local thread objec to hold files to load
                    Dictionary<string, DataTable> ftLoad = new Dictionary<string, DataTable>();

                    //List<string> failedFiles = new List<string>();
                    
                    foreach (FileInfo fl in tfiles)
                    {
                        // setup some local variables
                        string fileName = fl.FullName;
                        string humanSize = fl.humanSize();
                        long fileSize = fl.Length;
                        string[] fileArr = fl.createFileInfoArray();
                        string csvDataException = "";
                        nlvi = new ListViewItem(fileArr);


                        // Get the DataTable from the file
                        try
                        {
                            dtfl = this.GetDataTableFromCSVFile(fileName);
                        } catch (Exception getCsvDataException)
                        {
                            dtfl = null;
                            csvDataException = getCsvDataException.Message.ToString();
                        }
                        

                        // if the datatable is not null then add the data file to the list of files loaded
                        if (dtfl != null)
                        {
                            nlvi.Checked = true;
                            iLV.Invoke((MethodInvoker)delegate {iLV.Items.Add(nlvi);});
                            ptFilesToLoad.Add(fileName, dtfl);

                        }
                        // if the datatable is null then that means the CSV Extraction failed and we'll load it to failed files.
                        else
                        {
                            nlvi.ForeColor = Color.Red;
                            nlvi.Tag = csvDataException;
                            nlvi.Checked = false;
                            iLV.Invoke((MethodInvoker)delegate { iLV.Items.Add(nlvi); });
                        }
                        iLV.Invoke((MethodInvoker) delegate { iLV.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent); });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            });
            
        }

        private  DataTable GetDataTableFromCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();
           
            using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
            {
                string[] dv;
                List<string> dl = new List<string>();
                switch (fileDelmiter)
                {
                    case "Tab":
                        dl.Add("\t");
                        break;
                    case "Comma":
                        dl.Add(",");
                        break;
                    case "Semi-Colon":
                        dl.Add(";");
                        break;
                    case "Pipe":
                        dl.Add("|");
                        break;
                    default:
                        dl.Add("\t");
                        break;
                }
                dv = dl.ToArray();
                //MessageBox.Show(dv);
                csvReader.SetDelimiters(dv);
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
            
            
            return csvData;
        }

    }

    public static class FileInfoExtensions
    {
        public static string humanSize(this FileInfo fi)
        {
            double len = fi.Length;
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (len == 0)
                return "0" + suf[0];

            double bytes = (double)Math.Abs(Convert.ToDecimal(len));
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (string)(Math.Sign(len) * num).ToString() + " " + suf[place];
        }
        public static string[] createFileInfoArray(this FileInfo fi)
        {
            string[] fileArr = new string[4];
            fileArr[0] = fi.FullName;
            fileArr[1] = fi.humanSize();
            fileArr[2] = fi.Length.ToString();
            return fileArr;
        }
    }
}
