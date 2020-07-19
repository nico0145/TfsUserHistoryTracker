using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Model;
namespace UserHistoryTracker
{
    public partial class frmEditHours : Form
    {
        public WorkItemHours oItem { set; get; }
        public frmEditHours()
        {
            InitializeComponent();
        }

        private void frmEditHours_Load(object sender, EventArgs e)
        {
            txtID.Text = oItem.ID.ToString();
            txtHours.Text = oItem.HaxComHours.ToString("0.##");
            txtTitle.Text = oItem.Title;
            chkFixed.Checked = oItem.FixedHours;
            cmbBillType.SelectedIndex = Convert.ToInt32(oItem.BillingType) - 1;
            txtProjectCode.Text = oItem.ProjectId.ToString();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            decimal dHours;
            bool bValid = decimal.TryParse(txtHours.Text, out dHours);
            if (bValid && dHours % (decimal)0.25 == 0)
            {
                if (int.TryParse(txtProjectCode.Text, out int iPrjCode))
                {
                    oItem.ID = txtID.Text;
                    oItem.HaxComHours = dHours;
                    oItem.Hours = TimeSpan.FromHours((double)dHours);
                    oItem.Title = txtTitle.Text;
                    oItem.FixedHours = chkFixed.Checked;
                    oItem.BillingType = (BillingType)(cmbBillType.SelectedIndex + 1);
                    oItem.ResponseError = "";
                    oItem.ProjectId = iPrjCode;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Incorrect Project Code, it must be an integer for some reason");
                }
            }
            else
            {
                MessageBox.Show("Incorrect Hours value, it must be a decimal divisible by 0.25");
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtHours_TextChanged(object sender, EventArgs e)
        {
            chkFixed.Checked = true;
        }
    }
}
