//using System;
//using Microsoft.TeamFoundation.WorkItemTracking.Client;
//using System.Linq;
//namespace TFSDataSource
//{
//    public class WorkItemsDay
//    {
//        public DateTime Date { set; get; }
//        public System.Collections.Generic.List<WorkItemHours> WorkItems { set; get; }

//        public WorkItemsDay(DateTime mDate)
//        {
//            Date = mDate;
//            WorkItems = new System.Collections.Generic.List<WorkItemHours>();
//        }


//        public void CalculateHaxComHours(decimal TotalHoursPerDay)
//        {
//            if (WorkItems.Count > 0)
//            {
//                //TimeSpan tTotal = new TimeSpan(WorkItems.Sum(x => x.Hours.Ticks));
//                TimeSpan tTotal = new TimeSpan(WorkItems.Where(x => !x.FixedHours).Sum(x => x.Hours.Ticks));
//                TotalHoursPerDay = TotalHoursPerDay - ((decimal)new TimeSpan(WorkItems.Where(x => x.FixedHours).Sum(x => x.Hours.Ticks)).TotalHours);
//                foreach (WorkItemHours oWI in WorkItems.OrderBy(x => x.ID))
//                {
//                    if (oWI.FixedHours)
//                        oWI.HaxComHours = (decimal)oWI.Hours.TotalHours;
//                    else
//                        oWI.HaxComHours = (Math.Round((decimal)((TotalHoursPerDay * oWI.Hours.Ticks) / tTotal.Ticks) * 4, MidpointRounding.ToEven) / 4);
//                }
//                var diffHs = WorkItems.Where(x => !x.FixedHours).Sum(x => x.HaxComHours);
//                if (diffHs != TotalHoursPerDay)
//                {
//                    if (diffHs > TotalHoursPerDay)
//                    {
//                        WorkItems.Where(x => !x.FixedHours).OrderByDescending(x => x.HaxComHours).FirstOrDefault().HaxComHours += (diffHs - TotalHoursPerDay);
//                    }
//                    else
//                    {
//                        WorkItems.Where(x => !x.FixedHours).OrderBy(x => x.HaxComHours).FirstOrDefault().HaxComHours += (TotalHoursPerDay - diffHs);
//                    }
//                }
//            }

//        }


//    }
//    public class WorkItemHours
//    {
//        public string ID { set; get; }
//        public string Title { set; get; }
//        public TimeSpan Hours { set; get; }
//        public bool FixedHours { set; get; }
//        public decimal HaxComHours { set; get; }
//        //public 
//        public WorkItemHours() { }
//        public WorkItemHours(WorkItemRegister oRaw)
//        {
//            ID = oRaw.ID;
//            Title = oRaw.Title;
//            Hours = oRaw.timeSpan;
//        }
//        public override string ToString()
//        {
//            return "ID: " + ID.ToString() + "\tTime Span: " + Hours.ToString() + "\tTitle: " + Title;
//        }
//    }
//    public class WorkItemRegister
//    {
//        public string ID { set; get; }
//        public string Title { set; get; }
//        public string ChangedBy { set; get; }
//        public DateTime DateFrom { set; get; }
//        public DateTime DateTo { set; get; }
//        public TimeSpan timeSpan
//        {
//            get
//            {
//                return DateTo - DateFrom;
//            }
//        }
//        public WorkItemRegister()
//        { }
//        public WorkItemRegister(Revision oRaw)
//        {
//            ID = oRaw.Fields["ID"].Value.ToString();
//            Title = oRaw.Fields["Title"].Value.ToString();
//            ChangedBy = oRaw.Fields["Changed By"].Value.ToString();
//            //DateFrom = (DateTime)oRaw.Fields["Changed Date"].Value;
//        }
//        public override string ToString()
//        {
//            return "ID: " + ID + "\tFrom: " + DateFrom.ToString() + "\tTo: " + DateTo.ToString() + "\tTime Span: " + timeSpan.ToString() + "\tTitle: " + Title;
//        }
//        public WorkItemRegister DuplicateWithDates(DateTime dFrom, DateTime dTo)
//        {
//            WorkItemRegister oRetu = new WorkItemRegister();
//            oRetu.ID = ID;
//            oRetu.Title = Title;
//            oRetu.ChangedBy = ChangedBy;
//            oRetu.DateFrom = dFrom;
//            oRetu.DateTo = dTo;
//            return oRetu;
//        }
//    }
//}
