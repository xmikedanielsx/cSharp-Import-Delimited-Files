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
    public partial class frm_ErrorList : Form
    {
        public frm_ErrorList()
        {
            InitializeComponent();
        }

        private void lv_errorList_SelectedIndexChanged(object sender, EventArgs e)
        {
        }


        public void clearItemList ()
        {
            this.lv_errorList.Clear();
        }
        public void addItemToList(ListViewItem lvi)
        {
            lv_errorList.Items.Add(lvi);
        }

        private void lv_errorList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            txt_error.Text = lv_errorList.FocusedItem.Tag.ToString();
        }

        private void frm_ErrorList_Shown(object sender, EventArgs e)
        {
            if(lv_errorList.Items.Count > 0)
            {
                lv_errorList.FocusedItem = lv_errorList.Items[0];
                lv_errorList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                //MessageBox.Show(lv_errorList.Items[0].Tag.ToString());
            }
        }
    }
}
