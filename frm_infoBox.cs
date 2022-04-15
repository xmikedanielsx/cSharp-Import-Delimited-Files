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
    public partial class frm_infoBox : Form
    {
        public frm_infoBox()
        {
            InitializeComponent();
            //this.CenterToParent();
        }

        public void setMessage(Color clr, string txt)
        {
            this.lbl_message.ForeColor = clr;
            this.lbl_message.Text = txt;
        }

        private void btn_closeMe_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
