using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Model;
using System.Windows.Forms;

namespace UserHistoryTracker
{
    public partial class frmDataSources : Form
    {
        public frmDataSources()
        {
            InitializeComponent();
        }
        public DataSources lstDs { set; get; }
        private void frmDataSources_Load(object sender, EventArgs e)
        {
            UpdateListView();
        }
        private void UpdateListView()
        {
            foreach (DataSource oRow in lstDs)
            {
                lvwDS.Items.Add(DatasourceToLVI(oRow));
            }
        }
        private ListViewItem DatasourceToLVI(DataSource oRow)
        {
            ListViewItem oTemp = new ListViewItem(new string[] { (oRow.Type == Datasourcetype.Outlook ? "Outlook" : "TFS"),
                                                         oRow.ServerUrl,
                                                         oRow.ProjectName,
                                                         oRow.User
                                                       });
            oTemp.Tag = oRow;
            return oTemp;
        }
        private void lvwDS_DoubleClick(object sender, EventArgs e)
        {
            Point mousePosition = lvwDS.PointToClient(Control.MousePosition);
            ListViewHitTestInfo hit = lvwDS.HitTest(mousePosition);
            EditListviewItem(hit.Item);
        }
        private void EditListviewItem(ListViewItem oItem)
        {
            frmEditDataSource oEdit = new frmEditDataSource();
            oEdit.oDS = (DataSource)oItem.Tag;
            oEdit.Location = Cursor.Position;
            if (oEdit.ShowDialog() == DialogResult.OK)
            {
                ListViewItem oLvwEdit = DatasourceToLVI(oEdit.oDS);
                lvwDS.Items.Remove(oItem);
                lvwDS.Items.Add(oLvwEdit);
            }
        }

        private void cmdEdit_Click(object sender, EventArgs e)
        {
            if (lvwDS.SelectedItems.Count > 0)
                EditListviewItem(lvwDS.SelectedItems[0]);
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            frmEditDataSource oEdit = new frmEditDataSource();
            oEdit.oDS = new DataSource();
            oEdit.Location = Cursor.Position;
            if (oEdit.ShowDialog() == DialogResult.OK)
            {
                ListViewItem oLvwEdit = DatasourceToLVI(oEdit.oDS);
                lvwDS.Items.Add(oLvwEdit);
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you wish to delete the selected item?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                if (lvwDS.SelectedItems.Count > 0)
                    lvwDS.Items.Remove(lvwDS.SelectedItems[0]);
            }
        }

        private void frmDataSources_FormClosed(object sender, FormClosedEventArgs e)
        {
            lstDs = new DataSources();
            foreach (ListViewItem oRow in lvwDS.Items)
            {
                lstDs.Add((DataSource)oRow.Tag);
            }
        }
    }
}
