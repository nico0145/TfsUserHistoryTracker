using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserHistoryTracker
{
    public partial class frmLoginSendHaxCom : Form
    {
        public string LoginURL { set; get; }
        public string PHPSession { set; get; }
        public enum OpenMode
        {
            Search = 0,
            Submit = 1
        }
        public OpenMode openMode { set; get; }
        public Model.SendSelected SendSelected
        {
            get
            {
                if (optAll.Checked)
                    return Model.SendSelected.All;
                if (optDays.Checked)
                    return Model.SendSelected.Days;
                if (optSel.Checked)
                    return Model.SendSelected.Selected;
                return Model.SendSelected.Selected;
            }
        }
        public frmLoginSendHaxCom()
        {
            InitializeComponent();
        }

        private void CmdSend_Click(object sender, EventArgs e)
        {
            SendSubmit();
        }
        private void SendSubmit()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void FrmLoginSendHaxCom_Load(object sender, EventArgs e)
        {
            pnlSubmit.Visible = openMode != OpenMode.Search;
            pnlSearch.Visible = openMode == OpenMode.Search;
            cmdSend.Visible = false;
            webBrowser1.Navigate(LoginURL);
        }

        private void WebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.Document.Body.InnerHtml.Contains("View Published Notifiations"))
            {
                PHPSession = webBrowser1.Document.Cookie.Split(';').FirstOrDefault(x => x.Split('=').Any(y => y.Trim().Equals("PHPSESSID")))?.Split('=').LastOrDefault();
                cmdSend.Visible = !String.IsNullOrWhiteSpace(PHPSession);
                webBrowser1.Visible = !cmdSend.Visible;
                if (cmdSend.Visible)
                {
                    this.Width = 476;
                    this.Height = 136;
                }
            }
        }

        private void CmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
