using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class WorkItemsDay
    {
        public DateTime Date { set; get; }
        public List<WorkItemHours> WorkItems { set; get; }

        public WorkItemsDay(DateTime mDate)
        {
            Date = mDate;
            WorkItems = new List<WorkItemHours>();
        }


        public void CalculateHaxComHours(decimal TotalHoursPerDay)
        {
            if (WorkItems.Count > 0)
            {
                //TimeSpan tTotal = new TimeSpan(WorkItems.Sum(x => x.Hours.Ticks));
                TimeSpan tTotal = new TimeSpan(WorkItems.Where(x => !x.FixedHours).Sum(x => x.Hours.Ticks));
                TotalHoursPerDay = TotalHoursPerDay - ((decimal)new TimeSpan(WorkItems.Where(x => x.FixedHours).Sum(x => x.Hours.Ticks)).TotalHours);
                foreach (WorkItemHours oWI in WorkItems.OrderBy(x => x.ID))
                {
                    if (oWI.FixedHours)
                        oWI.HaxComHours = (decimal)oWI.Hours.TotalHours;
                    else
                        oWI.HaxComHours = (Math.Round(((TotalHoursPerDay * oWI.Hours.Ticks) / tTotal.Ticks) * 4, MidpointRounding.ToEven) / 4);
                }
                var diffHs = WorkItems.Where(x => !x.FixedHours).Sum(x => x.HaxComHours);
                if (diffHs != TotalHoursPerDay)
                {
                    var NonFixedHourItems = WorkItems.Where(x => !x.FixedHours);
                    if (NonFixedHourItems.Any())
                    {
                        if (diffHs > TotalHoursPerDay)
                        {
                            NonFixedHourItems.OrderByDescending(x => x.HaxComHours).FirstOrDefault().HaxComHours += (diffHs - TotalHoursPerDay);
                        }
                        else
                        {
                            NonFixedHourItems.OrderBy(x => x.HaxComHours).FirstOrDefault().HaxComHours += (TotalHoursPerDay - diffHs);
                        }
                    }
                }
            }

        }


    }
    public enum BillingType
    {
        None,
        Billable,
        FixedCost,
        Capitalized,
        StartUpExpensed,
        NonBillable
    }
    public enum SendSelected
    {
        Selected,
        Days,
        All
    }
    public class WorkItemHours
    {
        public string ID { set; get; }
        public int TfsId { get; set; }
        public string TFSType { set; get; }
        public string Title { set; get; }
        public TimeSpan Hours { set; get; }
        public bool FixedHours { set; get; }
        public decimal HaxComHours { set; get; }
        public DataSource FromDatasource { set; get; }
        public BillingType BillingType { get; set; }
        public int ProjectId { get; set; }
        public string ResponseError { get; set; }
        public int ClientId { get; set; }
        public int SubClientId { get; set; }
        public int Function { get; set; }
        public string Description { get; set; }
        public long? HaxComID { set; get; }
        public WorkItemHours()
        { }
        public WorkItemHours(WorkItemRegister oRaw)
        {
            ID = oRaw.ID;
            TfsId = 0;
            if (!int.TryParse(oRaw.ID, out int tryParseResult))
            {
                TfsId = 0;
            }
            else
            {
                TfsId = tryParseResult;
            }
            TFSType = oRaw.Type;
            Title = oRaw.Title;
            Hours = oRaw.timeSpan;
            BillingType = oRaw.BillingType;
            ProjectId = oRaw.ProjectId;
            if (ProjectId == 0)
            {
                ClientId = 166; // HXG Internal
                SubClientId = 5; // Tampa
            }

            if (!string.IsNullOrWhiteSpace(ID))
            {
                Function = 25; // DevTime
                Description = $"TFS {ID} {Title}";
            }
            else
            {
                Function = 41; // Dev Meeting
                Description = $"Meeting {Title}";
            }
        }
        public override string ToString()
        {
            return "ID: " + ID.ToString() + "\tTime Span: " + Hours.ToString() + "\tTitle: " + Title;
        }
    }
    public class WorkItemRegister
    {
        public string ID { set; get; }
        public string Title { set; get; }
        public string ChangedBy { set; get; }
        public DateTime DateFrom { set; get; }
        public DateTime DateTo { set; get; }
        public int ProjectId { set; get; }
        public BillingType BillingType { get; set; }
        public string Type { set; get; }
        public TimeSpan timeSpan
        {
            get
            {
                return DateTo - DateFrom;
            }
        }
        public WorkItemRegister()
        { }
        public WorkItemRegister(string sID, string sTitle, string sChangedBy, string projectCode, string type)
        {
            ID = sID;
            Title = sTitle;
            ChangedBy = sChangedBy;
            Type = type;
            if (!string.IsNullOrWhiteSpace(projectCode))
            {
                var first = projectCode.Split(new[] { '-' }, 2)[0].Split(new[] { '.' }, 2)[0];
                var success = int.TryParse(first, out int tryParseResult);
                ProjectId = 0;
                if (success)
                {
                    ProjectId = tryParseResult;
                }
            }
            SetBillingType(type);
        }
        private void SetBillingType(string type)
        {
            if (!string.IsNullOrWhiteSpace(type))
            {
                if (type.ToUpperInvariant().Contains("BUG"))
                {
                    BillingType = BillingType.Capitalized;
                }
                else
                {
                    BillingType = BillingType.Billable;
                }
            }
        }
        public override string ToString()
        {
            return "ID: " + ID + "\tFrom: " + DateFrom.ToString() + "\tTo: " + DateTo.ToString() + "\tTime Span: " + timeSpan.ToString() + "\tTitle: " + Title;
        }
        public WorkItemRegister DuplicateWithDates(DateTime dFrom, DateTime dTo)
        {
            WorkItemRegister oRetu = new WorkItemRegister();
            oRetu.ID = ID;
            oRetu.Type = Type;
            oRetu.Title = Title;
            oRetu.ChangedBy = ChangedBy;
            oRetu.DateFrom = dFrom;
            oRetu.DateTo = dTo;
            oRetu.BillingType = BillingType;
            oRetu.ProjectId = ProjectId;
            return oRetu;
        }
    }
}
