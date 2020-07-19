using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookDataSource
{
    public class OutlookData
    {
        public string Name { get; set; }
        public DateTimeOffset Date { get; set; }
        public decimal DurationInHours { get; set; }
    }
}
