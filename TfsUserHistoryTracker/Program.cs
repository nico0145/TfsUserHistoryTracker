using System;
using System.Collections.Generic;
using model;
using System.Linq;
using System.Data;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TfsUserHistoryTracker
{
    class Program
    {
        private static readonly string _TfsUrl = "https://tfs.healthaxis.net/tfs/HXG";
        private static readonly string _ProjectName = "Adjudicator";
        private static readonly string _DefaultUser = "Nicolas Milossevich";
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter TFS Url [" + _TfsUrl + "]:");
            string tfsUrl = Console.ReadLine();
            Console.WriteLine("Please enter Project Name [" + _ProjectName + "]:");
            string projectName = Console.ReadLine();
            Console.WriteLine("Please enter User Name [" + _DefaultUser + "]:");
            string userName = Console.ReadLine();
            Console.WriteLine("Last X days of history [Empty for all]");
            string sHistoryDays = Console.ReadLine();
            int iHistoryDays = 0;
            while (!int.TryParse((sHistoryDays.Length > 0 ? sHistoryDays : "0"), out iHistoryDays))
            {
                Console.WriteLine("Incorrect value, please enter an integer\r\nLast X days of history [Empty for all]");
                sHistoryDays = Console.ReadLine();
            }
            var registers = GetHistoricalWorkitemsByUser((tfsUrl == "" ? _TfsUrl : tfsUrl), (projectName == "" ? _ProjectName : projectName), (userName == "" ? _DefaultUser : userName), iHistoryDays);

            Console.WriteLine("Displaying Individual Tasks:");
            DisplayOutput(registers);
            Console.WriteLine(Environment.NewLine + "Displaying tasks by day:");
            var splittedRegs = SplitRegistersInDays(registers);
            DisplayOutput(splittedRegs);
            Console.WriteLine(Environment.NewLine + "Displaying tasks grouped by day:");
            var groupedItems = GroupItemsPerDay(splittedRegs);
            DisplayByDay(groupedItems);
            Console.WriteLine(Environment.NewLine + "Displaying tasks grouped by day (HAXCOM):");
            DisplayByDayHaxcom(groupedItems);
            Console.ReadLine();
        }
        public static List<WorkItemRegister> GetHistoricalWorkitemsByUser(string tfsURL, string projectName, string userName, int historyDays)
        {
            try
            {
                DateTime dFilter = new DateTime();
                if (historyDays == 0)
                    dFilter = DateTime.MinValue;
                else
                    dFilter = DateTime.Today.AddDays(historyDays * -1);

                Uri tfsUrl = new Uri(tfsURL);
                TfsTeamProjectCollection collection = new TfsTeamProjectCollection(tfsUrl);
                collection.EnsureAuthenticated();
                var workItemStore = collection.GetService<WorkItemStore>();
                var Ids = workItemStore.Query("select [System.Id] from WorkItems " +
                                              "where " + (projectName != "*" ? "[System.TeamProject] = '" + projectName + "' " +
                                              "and " : "") + "(ever [System.ChangedBy] ='" + userName + "' or [System.AssignedTo] ='" + userName + "')");
                var itemList = new List<WorkItemRegister>();
                DateTime dAcum;
                WorkItemRegister oWIR = new WorkItemRegister(); ;
                foreach (WorkItem wi in Ids)
                {
                    dAcum = wi.CreatedDate;
                    foreach (Revision revision in wi.Revisions)
                    {
                        if (dAcum > dFilter)
                        {
                            if (revision.Fields["Assigned To"].Value.ToString() == userName)
                            {
                                if (oWIR.ID == 0)
                                {
                                    oWIR = new WorkItemRegister(revision);
                                    oWIR.DateFrom = dAcum;
                                }
                            }
                            else
                            {
                                if (oWIR.ID > 0)
                                {
                                    oWIR.DateTo = (DateTime)revision.Fields["Changed Date"].Value;
                                    itemList.Add(oWIR);
                                    oWIR = new WorkItemRegister();
                                }
                            }

                        }
                        if (revision.Index == wi.Revisions.Count - 1)
                            dAcum = DateTime.Now;
                        else
                            dAcum = (DateTime)revision.Fields["Changed Date"].Value;
                    }
                    if (oWIR.ID > 0)
                    {
                        oWIR.DateTo = dAcum;
                        itemList.Add(oWIR);
                        oWIR = new WorkItemRegister();
                    }
                }
                return itemList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception retrieving data: {0}", ex.Message);
                return null;
            }
        }
        public static List<WorkItemRegister> SplitRegistersInDays(List<WorkItemRegister> itemList)
        {
            var SplittedItemList = new List<WorkItemRegister>();
            foreach (WorkItemRegister oRow in itemList)
            {
                SplittedItemList.AddRange(SplitTaskInDays(oRow));
            }
            return SplittedItemList;
        }

        public static List<WorkItemsDay> GroupItemsPerDay(List<WorkItemRegister> itemList)
        {
            DateTime dateFrom = itemList.Select(x => x.DateFrom).Min();
            DateTime dateThru = itemList.Select(x => x.DateFrom).Max();
            List<WorkItemsDay> groupedList = new List<WorkItemsDay>();
            WorkItemsDay oRow;
            WorkItemHours oTemp;
            for (var day = dateFrom.Date; day.Date <= dateThru.Date; day = day.AddDays(1))
            {
                oRow = new WorkItemsDay(day);
                foreach (WorkItemRegister wi in itemList.Where(x => x.DateFrom.Date == day.Date))
                {
                    oTemp = oRow.WorkItems.Where(x => x.ID == wi.ID).FirstOrDefault();
                    if (oTemp == null)
                    {
                        oTemp = new WorkItemHours(wi);
                        oRow.WorkItems.Add(oTemp);
                    }
                    else
                    {
                        oTemp.Hours = oTemp.Hours.Add(wi.timeSpan);
                    }
                }
                groupedList.Add(oRow);
            }
            return groupedList;
        }

        public static List<WorkItemRegister> SplitTaskInDays(WorkItemRegister oRow)
        {
            List<WorkItemRegister> lRetu = new List<WorkItemRegister>();
            DateTime dTempF = oRow.DateFrom;
            DateTime dTempT;
            while (dTempF < oRow.DateTo)
            {
                dTempT = dTempF.AddDays(1).Date;
                if (dTempT > oRow.DateTo)
                    dTempT = oRow.DateTo;
                lRetu.Add(oRow.DuplicateWithDates(dTempF, dTempT));
                dTempF = dTempT;
            }
            return lRetu;
        }
        public static void DisplayOutput(List<WorkItemRegister> oList)
        {
            foreach (WorkItemRegister oReg in oList.OrderBy(x => x.DateFrom))
            {
                Console.WriteLine(oReg.ToString());
            }
        }

        public static void DisplayByDay(List<WorkItemsDay> oItems)
        {
            foreach (WorkItemsDay oDay in oItems)
            {
                Console.WriteLine(Environment.NewLine + oDay.Date.ToShortDateString());
                foreach (WorkItemHours oWI in oDay.WorkItems)
                {
                    Console.WriteLine(oWI.ToString());
                }
            }
        }

        public static void DisplayByDayHaxcom(List<WorkItemsDay> oItems)
        {
            decimal TotalHoursPerDay = 8;
            TimeSpan tTotal;
            string sIds;
            string sHs;
            foreach (WorkItemsDay oDay in oItems.Where(x => x.WorkItems.Count > 0))
            {
                Console.WriteLine(Environment.NewLine + oDay.Date.ToString("dddd, MM/dd/yyyy"));
                tTotal = new TimeSpan(oDay.WorkItems.Sum(x => x.Hours.Ticks));
                sIds = "";
                sHs = "";
                foreach (WorkItemHours oWI in oDay.WorkItems.OrderBy(x => x.ID))
                {
                    oWI.HaxComHours = (Math.Round((decimal)((TotalHoursPerDay * oWI.Hours.Ticks) / tTotal.Ticks) * 4, MidpointRounding.ToEven) / 4);
                }
                var diffHs = oDay.WorkItems.Sum(x => x.HaxComHours);
                if (diffHs != TotalHoursPerDay && oDay.WorkItems.Any())
                {
                    if (diffHs < TotalHoursPerDay)
                    {
                        oDay.WorkItems.OrderBy(x => x.HaxComHours).FirstOrDefault().HaxComHours += (TotalHoursPerDay - diffHs);
                    }
                    else
                    {
                        oDay.WorkItems.OrderByDescending(x => x.HaxComHours).FirstOrDefault().HaxComHours += (diffHs - TotalHoursPerDay);
                    }
                }
                foreach (WorkItemHours oWI in oDay.WorkItems.Where(y => y.HaxComHours > 0).OrderBy(x => x.HaxComHours))
                {
                    sIds = sIds + oWI.ID.ToString() + ",";
                    sHs = sHs + oWI.HaxComHours.ToString("0.##") + ",";
                }
                Console.WriteLine(sIds.TrimEnd(','));
                Console.WriteLine(sHs.TrimEnd(','));
            }
        }
    }

}
