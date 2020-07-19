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
    public partial class frmRecalculateHours : Form
    {
        public decimal dHours { set; get; }
        public frmRecalculateHours()
        {
            InitializeComponent();
        }
        public enum OpenType
        {
            Hours = 0,
            TFS = 1
        }
        public OpenType openType { set; get; }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            decimal dTemp;
            bool bValid = decimal.TryParse(txtHours.Text, out dTemp);
            if (bValid && (dTemp % (decimal)0.25 == 0))
            {
                dHours = dTemp;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FrmRecalculateHours_Load(object sender, EventArgs e)
        {
            switch (openType)
            {
                case OpenType.Hours:
                    label1.Text = "Hours:";
                    break;
                case OpenType.TFS:
                    label1.Text = "TFS Id:";
                    break;
            }
        }
    }
}
