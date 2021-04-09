using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceMentor.Client.Services
{
    public class ThreeMonthsData
    {
        public MonthlyData CurrentMonth { get; set; }
        public MonthlyData LastMonth { get; set; }
        public MonthlyData PreviousMonth { get; set; }
    }
}
