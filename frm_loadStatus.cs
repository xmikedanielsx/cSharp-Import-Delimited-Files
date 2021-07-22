using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BulkImportDelimitedFlatFiles
{
    public partial class frm_loadStatus : Form
    {
        public frm_loadStatus(string title = "")
        {
            string ttl  = "Loading your tables to SQL";
            InitializeComponent();
            if(title == "") {
                this.Text = ttl ;
            } else {
                this.Text = ttl + " Server " + title;
            }
            this.btn_closeDialog.Visible = false;
        }
        public void setLoadingText(string ldText)
        {
            this.lbl_loadingTableStatus.Text = ldText;
        }
        public void setLoadStatus(decimal ldVal)
        {
            this.pgrbar_loadingTables.Value = Decimal.ToInt32(Math.Round(ldVal, 0));
        }

        public void showOkButton()
        {
            this.btn_closeDialog.Text = "OK";
            this.btn_closeDialog.Visible = true;
        }

        public void showCancelButton()
        {
            this.btn_closeDialog.Text = "Close";
            this.btn_closeDialog.Visible = true;
        }

        private void btn_closeDialog_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
