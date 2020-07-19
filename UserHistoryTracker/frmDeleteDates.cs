using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserHistoryTracker
{
    public partial class frmDeleteDates : Form
    {
        public DateTime? dateFrom
        {
            set
            {
                chkFrom.Checked = value.HasValue;
                if (chkFrom.Checked)
                {
                    dtpFrom.Value = value.Value;
                }
            }
            get
            {
                if (chkFrom.Checked)
                    return dtpFrom.Value;
                return null;
            }
        }
        public DateTime? dateTo
        {
            set
            {
                chkTo.Checked = value.HasValue;
                if (chkTo.Checked)
                {
                    dtpTo.Value = value.Value;
                }
            }
            get
            {
                if (chkTo.Checked)
                    return dtpTo.Value;
                return null;
            }
        }
        public frmDeleteDates()
        {
            InitializeComponent();
        }

        private void frmDeleteDates_Load(object sender, EventArgs e)
        {

        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
