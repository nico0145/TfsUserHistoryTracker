using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;
using MoreLinq;
namespace OutlookDataSource
{
    public class Outlook
    {
        // This is faster and requires a lot less work. Repurposed from: https://stackoverflow.com/questions/90899/net-get-all-outlook-calendar-items
        public IEnumerable<OutlookData> GetAllCalendarItems(DateTimeOffset start, DateTimeOffset[] skipDays)
        {
            IEnumerable<OutlookData> results;
            DateTimeOffset end = DateTimeOffset.Now.Date.AddDays(1).AddTicks(-1);
            Application outlookApplication = new Application();
            Items outlookItems = outlookApplication.Session.GetDefaultFolder(OlDefaultFolders.olFolderCalendar).Items;
            outlookItems.IncludeRecurrences = true;
            outlookItems.Sort("[Start]");
            outlookItems = outlookItems.Restrict($"[Start] >= '{start.ToString("g")}' AND [End] < '{end.ToString("g")}'");
            results = outlookItems.Cast<AppointmentItem>().Where(x => IsItemValid(x.Start, x.Subject, skipDays) && !x.AllDayEvent).Select(x => new OutlookData()
            {
                Name = x.Subject,
                Date = x.Start,
                DurationInHours = ComputeDurationInHours(x.Duration),
            });
            results = results.DistinctBy(x => new { x.Name, x.Date, x.DurationInHours }).OrderBy(x => x.Date); // I didn't have dupes but it mentioned that there might be dupes, better safe than sorry.
            return results;
        }
        private static bool IsItemValid(DateTime start, string subject, DateTimeOffset[] skipDays)
        {
            bool result = false;
            if (!skipDays.Any(x => x.Date == start.Date) && (subject == null || !subject.StartsWith("Canceled: ")) && start < DateTime.Now)
            {
                result = true;
            }
            return result;
        }
        private decimal ComputeDurationInHours(int duration)
        {
            decimal result;
            int checkDuration = duration % 15;
            if (checkDuration > 0)
            {
                result = ((decimal)((duration / 15) * 15) / 60.0m) + 0.25m; // get rid of the remainder and round up
            }
            else
            {
                result = (decimal)duration / 60.0m;
            }
            return result;
        }
    }
}
