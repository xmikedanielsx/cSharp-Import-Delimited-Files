using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BulkImportDelimitedFlatFiles
{
    public partial class frm_aboutBox : Form
    {
        public frm_aboutBox()
        {
            InitializeComponent();
        }

        string appFile;
        string appFolder;
        string cfgJsonFile;
        LocalConfig lconfig;
        private void frm_aboutBox_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;

            this.appFile = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
            this.appFolder = Path.GetDirectoryName(this.appFile) + @"\";
            this.cfgJsonFile = appFolder + @"config.json";
            if (File.Exists(this.cfgJsonFile))
            {
                this.lconfig = JsonSerializer.Deserialize<LocalConfig>(File.ReadAllText(this.cfgJsonFile));
            }
            picBox_backgroundImage.ImageLocation = lconfig.splashBg.ToString() == "" ? @"https://i.imgur.com/ePHUDhG.jpg" : lconfig.splashBg.ToString();
        }

        private void timer_splash_Tick(object sender, EventArgs e)
        {
            timer_splash.Start();
            this.Close();
        }

        private void picBox_backgroundImage_Click(object sender, EventArgs e)
        {

        }
    }
}
