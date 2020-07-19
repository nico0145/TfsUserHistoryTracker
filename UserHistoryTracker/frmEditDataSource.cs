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
    public partial class frmEditDataSource : Form
    {
        public frmEditDataSource()
        {
            InitializeComponent();
        }
        public DataSource oDS { set; get; }
        private void frmEditDataSource_Load(object sender, EventArgs e)
        {
            cmbType.DataSource = Enum.GetValues(typeof(Datasourcetype));
            txtUser.Text = oDS.User;
            txtServer.Text = oDS.ServerUrl;
            txtProject.Text = oDS.ProjectName;
            cmbType.SelectedItem = oDS.Type;
            chkExcludeTasks.Checked = oDS.ExcludeTasks;
        }

        private void cmbType_SelectedValueChanged(object sender, EventArgs e)
        {
            Datasourcetype type;
            Enum.TryParse<Datasourcetype>(cmbType.SelectedValue.ToString(), out type);
            lblPrj.Visible = type == Datasourcetype.TFS;
            lblServer.Visible = type == Datasourcetype.TFS;
            lblUser.Visible = type == Datasourcetype.TFS;
            txtProject.Visible = type == Datasourcetype.TFS;
            txtServer.Visible = type == Datasourcetype.TFS;
            txtUser.Visible = type == Datasourcetype.TFS;
            chkExcludeTasks.Visible = type == Datasourcetype.TFS;
            this.Height = (type == Datasourcetype.TFS ? 214 : 115);
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            Datasourcetype type;
            Enum.TryParse<Datasourcetype>(cmbType.SelectedValue.ToString(), out type);
            oDS.Type = type;
            oDS.ServerUrl = txtServer.Text;
            oDS.ProjectName = txtProject.Text;
            oDS.User = txtUser.Text;
            oDS.ExcludeTasks = chkExcludeTasks.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
