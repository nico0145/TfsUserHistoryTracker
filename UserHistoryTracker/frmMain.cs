using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Model;
using System.Windows.Forms;
using TFSDataSource;
using OutlookDataSource;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace UserHistoryTracker
{
    public partial class frmMain : Form
    {
        private ListViewItem oSelectedHour;
        private DataSources oDataSources;
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        const int keyF9 = 1;
        private const string TimeKeeperBaseURL = "https://CompanySupportWebsite.net";
        private const string DevTimeAddress = "/time/index/devtime";

        public frmMain()
        {
            InitializeComponent();
            RegisterHotKey(this.Handle, keyF9, 0, (int)Keys.F9);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            XDocument oXml = null;
            try
            {
                if (!System.IO.File.Exists(@"DataSources.xml"))
                {
                    using (System.IO.StreamWriter sw = System.IO.File.AppendText(@"DataSources.xml"))
                    {
                        sw.Write(@"<DataSources></DataSources>");
                    }
                }
                oXml = XDocument.Load("DataSources.xml");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            if (oXml == null)
                oDataSources = new DataSources();
            else
                oDataSources = new DataSources(oXml.Root);

        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == keyF9 && lvwHistory.SelectedItems.Count > 0)
            {
                SendKeysLvwItemDay(lvwHistory.SelectedItems[0]);
            }

            base.WndProc(ref m);
        }
        private List<CsvReg> getHaxComTFSTime(string sessionId, DateTime dFrom, DateTime dTo, bool bAll, bool bDev, bool bDMT, bool bPrjLss, bool bNBlb, bool bCap)
        {
            List<CsvReg> lstRetu = new List<CsvReg>();
            using (WebClient client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.Cookie, $"PHPSESSID={sessionId}");
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                NameValueCollection vals = new NameValueCollection();
                vals.Add("dateFrom", dFrom.ToString("MM/dd/yyyy")); //for edit
                vals.Add("dateTo", dTo.ToString("MM/dd/yyyy"));
                byte[] webClientResult = client.UploadValues($"{TimeKeeperBaseURL}/time/index/list-my-time", vals);
                string resultantString = Encoding.UTF8.GetString(webClientResult);
                var Rawlines = resultantString.Split(new string[] { "<tr><td>" }, StringSplitOptions.RemoveEmptyEntries).Skip(1)
                                                    .Select(x => x.Replace("</td><td>", ",")
                                                    .Split(new string[] { "<a href=" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()
                                                    .Split(new string[] { "<span class" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault());
                foreach (var sLine in Rawlines)
                {
                    var oCols = sLine.Split(',');
                    if ((bAll || (oCols[6] == "DEV" && bDev) || (oCols[6] == "DMT" && bDMT)) && ((!bPrjLss && !bNBlb && !bCap) || (bPrjLss && string.IsNullOrWhiteSpace(oCols[2])) || (bNBlb && oCols[oCols.Count() - 4] == "N") || (bCap && oCols[oCols.Count() - 4] == "C")))
                    {
                        lstRetu.Add(GetCSvRegFromCols(oCols));
                    }
                }
            }
            return lstRetu;
        }
        private CsvReg GetCSvRegFromCols(string[] oCols)
        {
            StringBuilder sTitle = new StringBuilder();
            for (int i = 9; i < oCols.Count() - 4; i++)
            {
                sTitle.Append(oCols[i]);
            }
            return new CsvReg($"{oCols[0]},{oCols[1]},{oCols[3]},{oCols[oCols.Count() - 3]}", sTitle.ToString());
        }
        private void LoadTFSCSV()
        {

            frmLoginSendHaxCom oLogin = new frmLoginSendHaxCom { LoginURL = TimeKeeperBaseURL };
            oLogin.openMode = frmLoginSendHaxCom.OpenMode.Search;
            oLogin.optDays.Enabled = false;
            oLogin.optSel.Enabled = false;
            if (oLogin.ShowDialog() == DialogResult.OK)
            {
                List<WorkItemsDay> oItems = new List<WorkItemsDay>();
                List<CsvReg> regs = getHaxComTFSTime(oLogin.PHPSession, dtpFrom.Value, DateTime.Today, oLogin.chkAll.Checked, (oLogin.CHKBug.Checked || oLogin.chkPBI.Checked || oLogin.chkOther.Checked), oLogin.chkMeeting.Checked, oLogin.chkPrjtLess.Checked, oLogin.chkNonBillable.Checked, oLogin.chkCapitalized.Checked);
                var oDataSource = oDataSources.First(x => x.Type == Datasourcetype.TFS);
                var outlookDataSource = oDataSources.FirstOrDefault(x => x.Type == Datasourcetype.Outlook);
                //gets rest of TFS data using the TFS IDS
                var tfsItems = WorkItemMapper.GetHistoricalWorkitemsByIDs(oDataSource.ServerUrl, oDataSource.ProjectName, oDataSource.User, regs.Select(x => x.TFSId).Distinct());
                //assembles the listview
                foreach (DateTime dDate in regs.Select(x => x.Date).Distinct())
                {
                    var oTemp = new WorkItemsDay(dDate);
                    foreach (CsvReg oReg in regs.Where(x => x.Date == dDate))
                    {
                        if (oReg.TFSId == 0) //meetings
                        {
                            var oWI = new WorkItemHours();
                            oWI.HaxComID = oReg.EntryID;
                            oWI.ID = "meeting";
                            oWI.ProjectId = 0;
                            oWI.ClientId = 166;
                            oWI.SubClientId = 5;
                            oWI.Function = 41; //dev meeting
                            oWI.Description = oReg.Title;
                            oWI.Title = oReg.Title;
                            oWI.FromDatasource = outlookDataSource;
                            oWI.BillingType = BillingType.Capitalized;
                            oWI.HaxComHours = oReg.Hours;
                            oTemp.WorkItems.Add(oWI);
                        }
                        else
                        {
                            var oTFSI = tfsItems.FirstOrDefault(x => x.ID == oReg.TFSId.ToString());
                            if (oTFSI != null)
                            {
                                string sType = oTFSI.Type.ToUpperInvariant();
                                if (oLogin.chkAll.Checked || (sType == "BUG" && oLogin.CHKBug.Checked) || (sType == "USER STORY" && oLogin.chkPBI.Checked)
                                    || (sType != "BUG" && sType != "PRODUCT BACKLOG ITEM" && oLogin.chkOther.Checked))
                                {
                                    oTemp.WorkItems.Add(WorkItemToWIHours(oTFSI, oReg.EntryID, oDataSource, oReg.Hours));
                                }
                            }
                        }
                    }
                    oItems.Add(oTemp);
                }
                UpdateListview(oItems, false);
            }
        }
        private WorkItemHours WorkItemToWIHours(WorkItemRegister oTFSI, long? haxcomId, DataSource oDataSource, decimal dHours)
        {
            var oWI = new WorkItemHours();
            oWI.HaxComID = haxcomId;
            if (int.TryParse(oTFSI.ID, out int iID))
                oWI.TfsId = iID;
            oWI.ID = oTFSI.ID;
            oWI.ProjectId = CheckReplacesProject(oTFSI.ProjectId);
            oWI.Function = 25; //dev
            oWI.Description = $"TFS {oWI.TfsId.ToString()} {oTFSI.Title}";
            oWI.Title = oTFSI.Title;
            oWI.FromDatasource = oDataSource;
            oWI.TFSType = oTFSI.Type;
            if (!string.IsNullOrWhiteSpace(oTFSI.Type))
            {
                if (oTFSI.Type.ToUpperInvariant().Contains("BUG") || oTFSI.Title.Contains("3.0"))
                {
                    oWI.BillingType = BillingType.Capitalized;
                }
                else
                {
                    oWI.BillingType = BillingType.Billable;
                }
            }
            oWI.HaxComHours = dHours;
            return oWI;
        }
        private int CheckReplacesProject(int iProjectId)
        {
            int iRetu = iProjectId;
            switch (iRetu)
            {
                case 3647:
                    iRetu = 3449;
                    break;
                case 12345:
                    iRetu = 3441;
                    break;
                case 23116:
                    iRetu = 21554;
                    break;
            }
            return iRetu;
        }
        private void LoadNewItems()
        {

            List<WorkItemsDay> oItems = new List<WorkItemsDay>();
            WorkItemsDay oTemp;
            foreach (DataSource oDataSource in oDataSources)
            {
                switch (oDataSource.Type)
                {
                    case Datasourcetype.Outlook:
                        Outlook oDS = new Outlook();
                        var oMeetings = oDS.GetAllCalendarItems(dtpFrom.Value, new DateTimeOffset[] { });
                        foreach (DateTime oDate in oMeetings.Select(x => x.Date.Date).Distinct())
                        {
                            oTemp = oItems.FirstOrDefault(x => x.Date == oDate);
                            if (oTemp == null)
                            {
                                oTemp = new WorkItemsDay(oDate);
                                oItems.Add(oTemp);
                            }
                            oTemp.WorkItems.AddRange(
                                oMeetings.Where(x => x.Date.Date == oDate)
                                            .Select(x => new WorkItemHours()
                                            {
                                                FixedHours = true,
                                                Hours = TimeSpan.FromHours((double)x.DurationInHours),
                                                HaxComHours = x.DurationInHours,
                                                Title = x.Name,
                                                ID = "meeting",
                                                TfsId = 0,
                                                FromDatasource = oDataSource,
                                                ProjectId = 0,
                                                ClientId = 166,
                                                SubClientId = 5,
                                                Function = 41, // meeting
                                                BillingType = BillingType.Capitalized,
                                                Description = x.Name,
                                            }));
                        }
                        break;
                    case Datasourcetype.TFS:
                        var oTFSItems = WorkItemMapper.GetHistoricalWorkitemsByUser(oDataSource.ServerUrl, oDataSource.ProjectName, oDataSource.User, dtpFrom.Value, oDataSource);
                        foreach (DateTime oDate in oTFSItems.Select(x => x.Date.Date).Distinct())
                        {
                            oTemp = oItems.FirstOrDefault(x => x.Date == oDate);
                            if (oTemp == null)
                            {
                                oTemp = new WorkItemsDay(oDate);
                                oItems.Add(oTemp);
                            }
                            oTemp.WorkItems.AddRange(oTFSItems.Where(x => x.Date.Date == oDate).Select(x => x.WorkItems).FirstOrDefault());
                        }
                        break;
                }
            }
            UpdateListview(oItems, true);
        }
        private void cmdSearch_Click(object sender, EventArgs e)
        {
            if (optNew.Checked)
                LoadNewItems();
            else
                LoadTFSCSV();
        }
        private void UpdateListview(List<WorkItemsDay> oItems, bool bNew)
        {
            ListViewGroup oLvg;
            lvwHistory.Items.Clear();
            foreach (WorkItemsDay oDay in oItems.Where(x => chkWeekends.Checked || (x.Date.DayOfWeek != DayOfWeek.Sunday && x.Date.DayOfWeek != DayOfWeek.Saturday)).OrderBy(x => x.Date))
            {
                if (bNew)
                    oDay.CalculateHaxComHours(8);
                oLvg = new ListViewGroup(oDay.Date.ToString("dddd, MM/dd/yyyy"), oDay.Date.ToString("dddd, MM/dd/yyyy"))
                {
                    Tag = oDay
                };
                lvwHistory.Groups.Add(oLvg);
                foreach (WorkItemHours oHours in oDay.WorkItems)
                {
                    lvwHistory.Items.Add(WorkItemHoursToLVWItem(oLvg, oHours));
                }
            }
        }
        private string BillTypeToStr(BillingType BillTypeToStr)
        {
            switch (BillTypeToStr)
            {
                case BillingType.Billable:
                    return "Billable";
                case BillingType.Capitalized:
                    return "Capitalized";
                case BillingType.FixedCost:
                    return "Fixed Cost";
                case BillingType.NonBillable:
                    return "Non-Billable";
                case BillingType.StartUpExpensed:
                    return "Start-Up Expensed";
            }
            return "";
        }
        private ListViewItem WorkItemHoursToLVWItem(ListViewGroup oGroup, WorkItemHours oItem)
        {
            ListViewItem oReturn = new ListViewItem(new string[] { oItem.ID.ToString(),
                                                                   oItem.TFSType,
                                                                   oItem.ProjectId.ToString(),
                                                                   oItem.Hours.ToString(),
                                                                   oItem.HaxComHours.ToString("0.##"),
                                                                   oItem.Title.ToString(),
                                                                   BillTypeToStr(oItem.BillingType),
                                                                   oItem.ResponseError
                                                                 })
            {
                Tag = oItem,
                Group = oGroup,
                UseItemStyleForSubItems = false
            };
            if (oItem.ProjectId == 0 && oItem.ID.ToLower() != "meeting")
            {
                oReturn.SubItems[2].BackColor = Color.Red;
            }
            return oReturn;
        }
        private void lvwHistory_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SetSelectedItem(e);
            if (((WorkItemHours)oSelectedHour.Tag).FromDatasource.Type == Datasourcetype.TFS)
            {
                OpenInWebBrowser();
            }
        }
        private void EditListviewItem(ListViewItem oItem)
        {
            frmEditHours oEdit = new frmEditHours
            {
                oItem = (WorkItemHours)oItem.Tag,
                Location = Cursor.Position
            };
            if (oEdit.ShowDialog() == DialogResult.OK)
            {
                ListViewItem oLvwEdit = WorkItemHoursToLVWItem(oItem.Group, oEdit.oItem);
                lvwHistory.Items.Remove(oItem);
                lvwHistory.Items.Add(oLvwEdit);
            }
        }
        private void SetSelectedItem(MouseEventArgs e)
        {
            ListViewHitTestInfo lvwHti = lvwHistory.HitTest(e.Location);
            oSelectedHour = lvwHti.Item;
        }
        private void lvwHistory_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && lvwHistory.FocusedItem != null && lvwHistory.FocusedItem.Bounds.Contains(e.Location))
            {
                SetSelectedItem(e);
                openInBrowserToolStripMenuItem.Visible = ((WorkItemHours)oSelectedHour.Tag).FromDatasource.Type == Datasourcetype.TFS;
                deleteThisItemFromDatesToolStripMenuItem.Visible = lvwHistory.SelectedItems.Count == 1;
                highlightItemToolStripMenuItem.Visible = lvwHistory.SelectedItems.Count == 1;
                toggleVerifiedToolStripMenuItem.Visible = lvwHistory.SelectedItems.Count == 1;
                mnuHour.Show(Cursor.Position);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you wish to delete the selected items?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                foreach (ListViewItem oItem in lvwHistory.SelectedItems)
                {
                    lvwHistory.Items.Remove(oItem);
                }
                oSelectedHour = null;
            }
        }

        private void recalculateDaysHoursToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecalculateSelectedDay();
        }
        private void RecalculateSelectedDay()
        {
            frmRecalculateHours oRec = new frmRecalculateHours
            {
                Location = Cursor.Position
            };
            oRec.openType = frmRecalculateHours.OpenType.Hours;
            if (oRec.ShowDialog() == DialogResult.OK)
            {
                ListViewGroup oLvg = oSelectedHour.Group;
                WorkItemsDay oDay = new WorkItemsDay(((WorkItemsDay)oLvg.Tag).Date);
                List<ListViewItem> lstRemove = new List<ListViewItem>();
                foreach (ListViewItem oItem in oLvg.Items)
                {
                    oDay.WorkItems.Add((WorkItemHours)oItem.Tag);
                    lstRemove.Add(oItem);
                }
                lstRemove.ForEach(x => lvwHistory.Items.Remove(x));
                oDay.CalculateHaxComHours(oRec.dHours);
                foreach (WorkItemHours oHours in oDay.WorkItems)
                {
                    lvwHistory.Items.Add(WorkItemHoursToLVWItem(oLvg, oHours));
                }
            }
        }

        private void removeAllTasksThatAreOneDayLongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewGroup oLvg = oSelectedHour.Group;
            List<ListViewItem> lstRemove = new List<ListViewItem>();
            foreach (ListViewItem oItem in oLvg.Items)
            {
                if (((WorkItemHours)oItem.Tag).Hours.Days == 1)
                {
                    lstRemove.Add(oItem);
                }
            }
            lstRemove.ForEach(x => lvwHistory.Items.Remove(x));
            if (oLvg.Items.Count > 0)
            {
                oSelectedHour = oLvg.Items[0];
                RecalculateSelectedDay();
            }
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditListviewItem(oSelectedHour);
        }
        private void sendKeysToDevTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(10000);
            SendKeysLvwItemDay(oSelectedHour);
        }
        private void SendKeysLvwItemDay(ListViewItem oSelItem)
        {

            ListViewGroup oLvg = oSelItem.Group;
            StringBuilder sIds = new StringBuilder("");
            StringBuilder sHs = new StringBuilder("");
            foreach (ListViewItem oItem in oLvg.Items)
            {
                if (((WorkItemHours)oItem.Tag).HaxComHours > 0)
                {
                    sIds.Append(((WorkItemHours)oItem.Tag).ID + ",");
                    sHs.Append(((WorkItemHours)oItem.Tag).HaxComHours.ToString("0.##") + ",");
                }
            }
            SendKeys.SendWait((((WorkItemsDay)oLvg.Tag).Date).ToString("MM/dd/yyyy") + "\t" + sIds.ToString().TrimEnd(',') + "\t" + sHs.ToString().TrimEnd(','));
        }

        private ListViewItem SendItemToHaxCom(ListViewItem oSelItem, string PHPSessionId, List<HaxComClient.Project> clients, ListViewGroup group)
        {
            var item = (WorkItemHours)oSelItem.Tag;
            if (item.HaxComHours > 0 && item.ResponseError != "Success!")
            {
                item.ResponseError = TrySubmitTime(((WorkItemsDay)group.Tag).Date, item, PHPSessionId, clients);
            }
            return WorkItemHoursToLVWItem(group, item);
        }
        private static List<HaxComClient.Project> GetHaxcomClients(string sessionId)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.Cookie, $"PHPSESSID={sessionId}");
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                string sClients = client.DownloadString($"{TimeKeeperBaseURL}/time/index/get-all-projects-billtypes-clients-for-time");
                var jRet = JsonConvert.DeserializeObject<HaxComClient.RootObject>(sClients);
                return jRet.projects;
            }
        }
        private static HaxconProject.Project GetProjectInfo(WebClient client, string ProjId)
        {
            NameValueCollection vals = new NameValueCollection();
            vals.Add("projID", ProjId);
            byte[] webClientResult = client.UploadValues($"{TimeKeeperBaseURL}/time/index/getprojectinfo", vals);
            string resultantString = Encoding.UTF8.GetString(webClientResult);
            var jRet = JsonConvert.DeserializeObject<HaxconProject.RootObject>(resultantString);
            return jRet.project.FirstOrDefault();
        }
        /*private static string TrySubmitTime(DateTime date, WorkItemHours input, string sessionId, List<HaxComClient.Project> clients)
        {
            return "Success!";  //This function is for testing purposes
        }*/
        private static string TrySubmitTime(DateTime date, WorkItemHours input, string sessionId, List<HaxComClient.Project> clients)
        {
            string result = "Success!";
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add(HttpRequestHeader.Cookie, $"PHPSESSID={sessionId}");
                    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    NameValueCollection vals = new NameValueCollection();
                    if (input.HaxComID.HasValue)//entryId
                        vals.Add("entryId", input.HaxComID.ToString()); //for edit
                    vals.Add("date", date.ToString("MM/dd/yyyy"));
                    vals.Add("tfsId", ConvertZeroToEmptyString(input.TfsId));
                    vals.Add("ticketId", "");
                    bool bClientFromDb = false;
                    if (input.ProjectId > 0)
                    {
                        var HCCLient = clients.FirstOrDefault(x => x.name.Contains(input.ProjectId.ToString()));
                        if (HCCLient == null)
                        {
                            HCCLient = clients.FirstOrDefault(x => x.id.Trim() == input.ProjectId.ToString().Trim());
                            if (HCCLient != null)
                                result = $"Couldn't find Project Code {input.ProjectId.ToString()} on drop down list description, using it as ID instead, {HCCLient.name} selected";
                        }
                        if (HCCLient != null)
                        {
                            vals.Add("projectId", HCCLient.id);
                            var ProjInfo = GetProjectInfo(client, HCCLient.id);
                            if (ProjInfo != null)
                            {
                                vals.Add("clientId", ProjInfo.clientID);
                                vals.Add("subclientId", ProjInfo.subclientID);
                                bClientFromDb = true;
                            }
                        }
                        else
                        {
                            vals.Add("projectId", ConvertZeroToEmptyString(input.ProjectId));
                        }
                    }
                    if (!bClientFromDb)
                    {
                        if (input.ID != "meeting")
                            result = "Client and Project not populated from API";
                        vals.Add("clientId", ConvertZeroToEmptyString(input.ClientId));
                        vals.Add("subclientId", ConvertZeroToEmptyString(input.SubClientId));
                    }
                    vals.Add("funct", input.Function.ToString());
                    vals.Add("description", input.Title);
                    vals.Add("billingType", ((int)input.BillingType).ToString());
                    vals.Add("hours", input.HaxComHours.ToString("#.##"));
                    vals.Add("fromPage", "my");
                    byte[] webClientResult = client.UploadValues($"{TimeKeeperBaseURL}/time/index/save-update", vals);
                    string resultantString = Encoding.UTF8.GetString(webClientResult);
                    Console.WriteLine(resultantString);
                    if (resultantString.ToUpperInvariant().Contains("ERROR"))
                    {
                        if (resultantString.ToUpperInvariant().Contains("TFS"))
                        {
                            input.Description += $" TFS: {input.TfsId}";
                            input.TfsId = 0;
                            result = TrySubmitTime(date, input, sessionId, clients);
                        }
                        else
                        {
                            result = resultantString;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                result = err.Message;
            }

            return result;
        }
        private static string ConvertZeroToEmptyString(int number)
        {
            string result = string.Empty;
            if (number != 0)
            {
                result = number.ToString();
            }
            return result;
        }
        private void cmdOpenDevtime_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start($"{TimeKeeperBaseURL}{DevTimeAddress}");
        }
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                var oXml = new XDocument(oDataSources.toXMLElement());
                oXml.Save("DataSources.xml");
                UnregisterHotKey(this.Handle, keyF9);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void cmdEditDS_Click(object sender, EventArgs e)
        {
            frmDataSources oEdit = new frmDataSources
            {
                lstDs = oDataSources,
                Location = Cursor.Position
            };
            oEdit.ShowDialog();
            oDataSources = oEdit.lstDs;
        }
        private void OpenInWebBrowser()
        {
            WorkItemHours oWIH = ((WorkItemHours)oSelectedHour.Tag);
            System.Diagnostics.Process.Start(GetURL(oWIH));
        }
        private void openInBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenInWebBrowser();
        }
        private string GetURL(WorkItemHours oWIH)
        {
            return oWIH.FromDatasource.ServerUrl + @"/" + oWIH.FromDatasource.ProjectName + "/_workitems?id=" + oWIH.ID;
        }
        private void deleteThisItemFromDatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDeleteDates oDel = new frmDeleteDates();
            WorkItemHours oselItem = (WorkItemHours)oSelectedHour.Tag;
            List<WorkItemsDay> oItems = new List<WorkItemsDay>();
            WorkItemsDay oItg;
            foreach (ListViewGroup oLvg in lvwHistory.Groups)
            {
                oItg = (WorkItemsDay)oLvg.Tag;
                if (oItg.WorkItems.Any(x => x.ID == oselItem.ID && x.Title == oselItem.Title))
                {
                    oItems.Add(oItg);
                }
            }
            oDel.dateFrom = oItems.Min(x => x.Date).Date;
            oDel.dateTo = oItems.Max(x => x.Date).Date;
            if (oDel.ShowDialog() == DialogResult.OK)
            {
                List<ListViewItem> lstLviDel = FindListItemsfromWorkItem(oselItem, oDel.dateFrom, oDel.dateTo);
                foreach (ListViewItem oLvi in lstLviDel)
                {
                    lvwHistory.Items.Remove(oLvi);
                }
            }
        }
        private List<ListViewItem> FindListItemsfromWorkItem(WorkItemHours oselItem, DateTime? dateFrom, DateTime? dateTo)
        {
            List<ListViewItem> lstLvi = new List<ListViewItem>();
            foreach (ListViewGroup oLvg in lvwHistory.Groups)
            {
                DateTime dateGroup = DateTime.ParseExact(oLvg.Name, "dddd, MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                if (dateGroup >= (dateFrom ?? dateGroup) && dateGroup <= (dateTo ?? dateGroup))
                {
                    foreach (ListViewItem oLvi in oLvg.Items)
                    {
                        if (((WorkItemHours)oLvi.Tag).ID == oselItem.ID && ((WorkItemHours)oLvi.Tag).Title == oselItem.Title)
                            lstLvi.Add(oLvi);
                    }
                }
            }
            return lstLvi;
        }
        private void highlightItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem oLVI in lvwHistory.Items)
            {
                oLVI.BackColor = Color.White;
            }
            WorkItemHours oselItem = (WorkItemHours)oSelectedHour.Tag;
            List<ListViewItem> lstLviH = FindListItemsfromWorkItem(oselItem, null, null);
            foreach (ListViewItem oLVI in lstLviH)
            {
                oLVI.BackColor = Color.Yellow;
            }
        }

        private void toggleVerifiedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool bVerified = oSelectedHour.SubItems[1].BackColor == Color.Green;
            WorkItemHours oselItem = (WorkItemHours)oSelectedHour.Tag;
            List<ListViewItem> lstLviH = FindListItemsfromWorkItem(oselItem, null, null);
            foreach (ListViewItem oLVI in lstLviH)
            {
                oLVI.SubItems[1].BackColor = bVerified ? Color.White : Color.Green;
            }
        }

        private void copyToStandupEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string sContextStart = "<HTML><BODY><!--StartFragment -->";
            const string sContextEnd = "<!--EndFragment --></BODY></HTML>";
            string m_sDescription = "Version:0.9" + Environment.NewLine + "StartHTML:aaaaaaaaaa" + Environment.NewLine + "EndHTML:bbbbbbbbbb" + Environment.NewLine + "StartFragment:cccccccccc" + Environment.NewLine + "EndFragment:dddddddddd" + Environment.NewLine;

            StringBuilder sClipboard = new StringBuilder("");
            string sDate = "";
            List<KeyValuePair<DateTime, WorkItemHours>> lstWIH = new List<KeyValuePair<DateTime, WorkItemHours>>();
            foreach (ListViewItem oItem in lvwHistory.SelectedItems)
            {
                lstWIH.Add(new KeyValuePair<DateTime, WorkItemHours>(((WorkItemsDay)oItem.Group.Tag).Date, (WorkItemHours)oItem.Tag));
            }
            foreach (KeyValuePair<DateTime, WorkItemHours> oWIHD in lstWIH.OrderBy(x => x.Key))
            {
                if (sDate != oWIHD.Key.ToLongDateString())
                {
                    sDate = oWIHD.Key.ToLongDateString();
                    sClipboard.Append("</ul>" + Environment.NewLine + "<h2>" + sDate + "</h2>" + Environment.NewLine + "<ul>");
                }
                sClipboard.Append(@"<li>" + (oWIHD.Value.FromDatasource.Type == Datasourcetype.TFS ? (@"<a title=""" + oWIHD.Value.Title + @""" href=""" + GetURL(oWIHD.Value) + @""">" + oWIHD.Value.ID + "</a> - ") : "") + oWIHD.Value.Title + "</li>");
            }
            sClipboard = new StringBuilder(sClipboard.ToString().TrimStart(@"</ul>".ToCharArray()) + "</ul>");
            string sData = m_sDescription + sContextStart + sClipboard.ToString() + sContextEnd;
            sData = sData.Replace("aaaaaaaaaa", m_sDescription.Length.ToString().PadLeft(10, '0'));
            sData = sData.Replace("bbbbbbbbbb", sData.Length.ToString().PadLeft(10, '0'));
            sData = sData.Replace("cccccccccc", (m_sDescription + sContextStart).Length.ToString().PadLeft(10, '0'));
            sData = sData.Replace("dddddddddd", (m_sDescription + sContextStart + sClipboard.ToString()).Length.ToString().PadLeft(10, '0'));
            Clipboard.SetDataObject(new DataObject(DataFormats.Html, sData), true);
        }
        private void cmdSend_Click(object sender, EventArgs e)
        {
            frmLoginSendHaxCom oLogin = new frmLoginSendHaxCom { LoginURL = TimeKeeperBaseURL };
            oLogin.openMode = frmLoginSendHaxCom.OpenMode.Submit;
            oLogin.optDays.Enabled = lvwHistory.SelectedItems.Count > 0;
            oLogin.optSel.Enabled = oLogin.optDays.Enabled;
            if (oLogin.ShowDialog() == DialogResult.OK)
            {
                var Clients = GetHaxcomClients(oLogin.PHPSession);
                List<ListViewItem> Items;
                switch (oLogin.SendSelected)
                {
                    case SendSelected.Selected:
                        Items = lvwHistory.SelectedItems.Cast<ListViewItem>().ToList();
                        ListViewGroup grp;
                        foreach (ListViewItem item in Items)
                        {
                            grp = item.Group;
                            lvwHistory.Items.Remove(item);
                            lvwHistory.Items.Add(SendItemToHaxCom(item, oLogin.PHPSession, Clients, grp));
                        }
                        break;
                    case SendSelected.Days:
                        List<ListViewGroup> oLvgs = new List<ListViewGroup>();
                        foreach (ListViewItem item in lvwHistory.SelectedItems)
                        {
                            if (!oLvgs.Any(x => x == item.Group))
                            {
                                oLvgs.Add(item.Group);
                            }
                        }
                        foreach (var oLvg in oLvgs)
                        {
                            Items = oLvg.Items.Cast<ListViewItem>().ToList();
                            foreach (ListViewItem oItem in Items)
                            {
                                lvwHistory.Items.Remove(oItem);
                                lvwHistory.Items.Add(SendItemToHaxCom(oItem, oLogin.PHPSession, Clients, oLvg));
                            }
                        }
                        break;
                    case SendSelected.All:
                        Items = lvwHistory.Items.Cast<ListViewItem>().ToList();
                        foreach (ListViewItem item in Items)
                        {
                            grp = item.Group;
                            lvwHistory.Items.Remove(item);
                            lvwHistory.Items.Add(SendItemToHaxCom(item, oLogin.PHPSession, Clients, grp));
                        }
                        break;
                }

            }
        }

        private void AddTFSItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRecalculateHours oRec = new frmRecalculateHours
            {
                Location = Cursor.Position
            };
            oRec.openType = frmRecalculateHours.OpenType.TFS;
            if (oRec.ShowDialog() == DialogResult.OK)
            {
                if (int.TryParse(oRec.dHours.ToString(), out int iId))
                {
                    WorkItemRegister oWi = null;
                    foreach (var oDataSource in oDataSources.Where(x => x.Type == Datasourcetype.TFS))
                    {
                        if (oWi == null)
                        {
                            oWi = WorkItemMapper.GetHistoricalWorkitemsByIDs(oDataSource.ServerUrl, oDataSource.ProjectName, oDataSource.User, new int[] { iId }).FirstOrDefault();
                            if (oWi != null)
                            {
                                lvwHistory.Items.Add(WorkItemHoursToLVWItem(lvwHistory.SelectedItems[0].Group, WorkItemToWIHours(oWi, null, oDataSource, 0)));
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Coulnd't parse the TFS ID, please try again with an integer");
                }
            }
        }
    }
    class CsvReg
    {
        public CsvReg(string sLine)
        {
            Parse(sLine);
        }
        private void Parse(string sLine)
        {
            string[] sCols = sLine.Split(',');
            if (long.TryParse(sCols[0], out long lEntry))
                EntryID = lEntry;
            if (DateTime.TryParse(sCols[1], out DateTime dDate))
                Date = dDate;
            if (int.TryParse(sCols[2], out int lTFS))
                TFSId = lTFS;
            else
                TFSId = 0;
            if (decimal.TryParse(sCols[3], out decimal dHours))
                Hours = dHours;
        }
        public CsvReg(string sLine, string sTitle)
        {
            Parse(sLine);
            Title = sTitle;
        }
        public long EntryID { set; get; }
        public DateTime Date { set; get; }
        public int TFSId { set; get; }
        public decimal Hours { set; get; }
        public string Title { set; get; }
    }
    namespace HaxComClient
    {
        public class Client
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Project
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class BillType
        {
            public string id { get; set; }
            public string name { get; set; }
        }

        public class RootObject
        {
            public List<Client> clients { get; set; }
            public List<Project> projects { get; set; }
            public List<BillType> billTypes { get; set; }
        }
    }
    namespace HaxconProject
    {
        public class Project
        {
            public string ID { get; set; }
            public string name { get; set; }
            public string active { get; set; }
            public string clientID { get; set; }
            public string subclientID { get; set; }
            public string clientName { get; set; }
            public bool clientIsInternal { get; set; }
            public string projCode { get; set; }
            public object needBy { get; set; }
            public string billType { get; set; }
            public string billTypeName { get; set; }
        }

        public class RootObject
        {
            public string result { get; set; }
            public List<Project> project { get; set; }
        }
    }
}
