using System;
using System.Collections.Generic;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Linq;
using Model;
namespace TFSDataSource
{
    public static class WorkItemMapper
    {
        public static List<WorkItemRegister> GetHistoricalWorkitemsByIDs(string tfsURL, string projectName, string userName, IEnumerable<int> lstIds)
        {
            Uri tfsUrl = new Uri(tfsURL);
            TfsTeamProjectCollection collection = new TfsTeamProjectCollection(tfsUrl);
            collection.EnsureAuthenticated();
            var workItemStore = collection.GetService<WorkItemStore>();
            var itemList = new List<WorkItemRegister>();
            foreach (int lId in lstIds.Where(x => x > 0))
            {
                WorkItem oWi = workItemStore.GetWorkItem(lId);
                if (oWi.Type.Name == "Task")
                {
                    oWi = workItemStore.GetWorkItem(oWi.WorkItemLinks[0].TargetId);//get the main TFS Item that the task is linked to
                }
                itemList.Add(new WorkItemRegister(oWi.Id.ToString(), oWi.Title, "", oWi.Fields["ProjectCode"].Value.ToString(), oWi.Type.Name));
            }
            return itemList;
        }
        public static List<WorkItemsDay> GetHistoricalWorkitemsByUser(string tfsURL, string projectName, string userName, DateTime dFilter, DataSource oDataSource)
        {
            try
            {
                Uri tfsUrl = new Uri(tfsURL);
                TfsTeamProjectCollection collection = new TfsTeamProjectCollection(tfsUrl);
                collection.EnsureAuthenticated();
                var workItemStore = collection.GetService<WorkItemStore>();
                var Ids = workItemStore.Query("select [System.Id] from WorkItems " +
                                              "where " +
                                              (projectName != "*" ? "[System.TeamProject] = '" + projectName + "' " + "and " : "") +
                                              "(ever [System.ChangedBy] ='" + userName + "' or [System.AssignedTo] ='" + userName + "')" +
                                              (oDataSource.ExcludeTasks ? " and [Work Item Type] <> 'Task'" : ""));
                var itemList = new List<WorkItemRegister>();
                DateTime dAcum;
                WorkItemRegister oWIR = new WorkItemRegister();
                foreach (WorkItem wi in Ids)
                {
                    dAcum = wi.CreatedDate;
                    foreach (Revision revision in wi.Revisions)
                    {
                        if (revision.Fields["Assigned To"].Value.ToString() == userName)
                        {
                            if (oWIR.ID == null)
                            {
                                string PrjCode = "";
                                if (wi.Fields.Contains("Project Code"))
                                    PrjCode = wi.Fields["Project Code"].Value.ToString();
                                else if (wi.Fields.Contains("ProjectCode"))
                                    PrjCode = wi.Fields["ProjectCode"].Value.ToString();
                                oWIR = new WorkItemRegister(revision.Fields["ID"].Value.ToString(),
                                                            revision.Fields["Title"].Value.ToString(),
                                                            revision.Fields["Changed By"].Value.ToString(),
                                                            PrjCode,
                                                            revision.Fields["Work Item Type"].Value.ToString())
                                {
                                    DateFrom = dAcum
                                };
                            }
                        }
                        else
                        {
                            if (oWIR.ID != null)
                            {
                                oWIR.DateTo = (DateTime)revision.Fields["Changed Date"].Value;
                                itemList.Add(oWIR);
                                oWIR = new WorkItemRegister();
                            }
                        }
                        if (revision.Index == wi.Revisions.Count - 1)
                            dAcum = DateTime.Now;
                        else
                            dAcum = (DateTime)revision.Fields["Changed Date"].Value;
                    }
                    if (oWIR.ID != null)
                    {
                        oWIR.DateTo = dAcum;
                        itemList.Add(oWIR);
                        oWIR = new WorkItemRegister();
                    }
                }

                var SplittedItemList = new List<WorkItemRegister>();
                DateTime dTempF;
                DateTime dTempT;
                foreach (WorkItemRegister oRow in itemList)
                {
                    dTempF = oRow.DateFrom;
                    while (dTempF < oRow.DateTo)
                    {
                        dTempT = dTempF.AddDays(1).Date;
                        if (dTempT > oRow.DateTo)
                            dTempT = oRow.DateTo;
                        SplittedItemList.Add(oRow.DuplicateWithDates(dTempF, dTempT));
                        dTempF = dTempT;
                    }
                }


                dTempF = dFilter;//SplittedItemList.Select(x => x.DateFrom).Min();
                dTempT = SplittedItemList.Select(x => x.DateFrom).Max();
                List<WorkItemsDay> groupedList = new List<WorkItemsDay>();
                WorkItemHours oTemp;
                for (var day = dTempF.Date; day.Date <= dTempT.Date; day = day.AddDays(1))
                {
                    WorkItemsDay oRow = new WorkItemsDay(day);
                    foreach (WorkItemRegister wi in SplittedItemList.Where(x => x.DateFrom.Date == day.Date))
                    {
                        oTemp = oRow.WorkItems.FirstOrDefault(x => x.ID == wi.ID);
                        if (oTemp == null)
                        {
                            oTemp = new WorkItemHours(wi)
                            {
                                FromDatasource = oDataSource
                            };
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
            catch (Exception ex)
            {
                Console.WriteLine("Exception retrieving data: {0}", ex.Message);
                return new List<WorkItemsDay>();
            }
        }
    }
}
