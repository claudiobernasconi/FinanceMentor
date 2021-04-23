using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceMentor.Client.Services
{
    public interface IDataService
    {
        Task<ICollection<YearlyItem>> LoadCurrentYearExpenses();
        Task<ICollection<YearlyItem>> LoadCurrentYearEarnings();
        Task<ThreeMonthsData> LoadLast3MonthsExpenses();
        Task<ThreeMonthsData> LoadLast3MonthsEarnings();
    }
}
