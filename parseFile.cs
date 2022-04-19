using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using Microsoft.VisualBasic.FileIO;

namespace BulkImportDelimitedFlatFiles
{
    class parseFile : frm_Main
    {
        private DirectoryInfo dirInfo;
        private subClass parent;
        public FileInfo[] tfiles = new FileInfo[] { };
        private string fileDelmiter;
        public ListView iLV = new ListView();
        // Class Constructor
        public parseFile(string dirLocation, string fileDelmiter)
        {
            this.parent = parent;
            this.fileDelmiter = fileDelmiter;
            try
            {
                this.dirInfo = new DirectoryInfo(dirLocation);
            } catch (Exception e)
            {
                throw (new Exception(@"Sorry but your directory does not exist or you do not have access"));
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

        public Task ldFiles()
        {
            //MessageBox.Show("Made it to the func");
            return Task.Run(() =>
            {
                try
                {

                    string[] extensions = new[] { ".txt", ".csv" };

                    // create local thread object to store the files
                    this.tfiles = dirInfo.EnumerateFiles().Where(f => extensions.Contains(f.Extension.ToLower())).ToArray();
                    // MessageBox.Show(tfiles[0].FullName.ToString());


                    // Create new local thread objec to hold files to load
                    Dictionary<string, DataTable> ftLoad = new Dictionary<string, DataTable>();

                    //List<string> failedFiles = new List<string>();
                    foreach (FileInfo fl in tfiles)
                    {
                        // use custom class to get specific info from file passed
                        getFileDetails fileDetails = new getFileDetails(fl.FullName.ToString());

                        // setup some local variables
                        string fileName = fileDetails.FullName();
                        string humanSize = fileDetails.humanSize();
                        long fileSize = fileDetails.Size();
                        string[] fileArr = fileDetails.createFileArray();
                        ListViewItem nlvi = new ListViewItem(fileArr);
                        iLV.Items.Add(nlvi);

                        parent.addFileToList(fileName);
                        // use parent class to add item to FileList
                        base.addFileToList(fileName);
                        //Invoke(new Action<ListViewItem>(addFileToList), nlvi);

                        // Get the DataTable from the file
                        DataTable dtfl =  this.GetDataTableFromCSVFile(fileName);

                        // if the datatable is not null then add the data file to the list of files loaded
                        if (dtfl != null)
                        {
                            // add to filesToLoad
                            base.addAFileToLoad(fileName, dtfl);
                            // Invoke(new Action<string, DataTable>(addAFileToLoad), new object[] { fileName, dtfl });
                        }
                        // if the datatable is null then that means the CSV Extraction failed and we'll load it to failed files.
                        else
                        {
                            // add to FailedFilesToLoad
                            base.addFailedFileToList(fileName);
                            // Invoke(new Action<ListViewItem>(addFailedFileToList), nlvi);
                        }
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
            try
            {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return null;
            }
            return csvData;
        }

    }
}
