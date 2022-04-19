using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkImportDelimitedFlatFiles
{
    class getFileDetails
    {

        private FileInfo fi;
        public getFileDetails(string filePath)
        {
            try
            {
                this.fi = new FileInfo(filePath);
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public string humanSize ()
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

        public string Name ()
        {
            return fi.Name;
        } 
        public string FullName ()
        {
            return fi.FullName;
        }

        public long Size()
        {
            return fi.Length;
        }

        public string[] createFileArray()
        {
            string[] fileArr = new string[4];
            fileArr[0] = fi.Name;
            fileArr[1] = this.humanSize();
            fileArr[2] = fi.Length.ToString();
            return fileArr;
        }
    }
}

