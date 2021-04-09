using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceMentor.Client.Services
{
    public class MonthlyData
    {
        public ICollection<MonthlyItem> Data { get; set; }
        public string Label { get; set; }
    }
}
