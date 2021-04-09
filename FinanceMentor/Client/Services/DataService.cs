using FinanceMentor.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FinanceMentor.Client.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _httpClient;
        private readonly int _currentYear = DateTime.Today.Year;

        public DataService(HttpClient http)
        {
            _httpClient = http;
        }

        public async Task<ICollection<YearlyItem>> LoadCurrentYearEarnings()
        {
            var data = await _httpClient.GetFromJsonAsync<Earning[]>("api/Earnings");
            return data.Where(earning => earning.Date >= new DateTime(_currentYear, 1, 1)
                && earning.Date <= new DateTime(_currentYear, 12, 31))
                .GroupBy(earning => earning.Date.Month)
                .OrderBy(earning => earning.Key)
                .Select(earning => new YearlyItem
                {
                    Month = GetMonthAsText(earning.Key, _currentYear),
                    Amount = earning.Sum(item => item.Amount)
                })
                .ToList();
        }

        public async Task<ICollection<YearlyItem>> LoadCurrentYearExpenses()
        {
            var data = await _httpClient.GetFromJsonAsync<Expense[]>("api/Expenses");
            return data.Where(expense => expense.Date >= new DateTime(_currentYear, 1, 1)
                && expense.Date <= new DateTime(_currentYear, 12, 31))
                .GroupBy(expense => expense.Date.Month)
                .OrderBy(expense => expense.Key)
                .Select(expense => new YearlyItem
                {
                    Month = GetMonthAsText(expense.Key, _currentYear),
                    Amount = expense.Sum(item => item.Amount)
                })
                .ToList();
        }

        public async Task<ThreeMonthsData> LoadLast3MonthsEarnings()
        {
            var currentMonth = DateTime.Today.Month;
            var lastMonth = DateTime.Today.AddMonths(-1);
            var previousMonth = DateTime.Today.AddMonths(-2);

            return new ThreeMonthsData
            {
                CurrentMonth = new MonthlyData
                {
                    Data = await GetMonthlyEarnings(currentMonth, _currentYear),
                    Label = GetMonthAsText(currentMonth, _currentYear)
                },
                LastMonth = new MonthlyData
                {
                    Data = await GetMonthlyEarnings(lastMonth.Month, lastMonth.Year),
                    Label = GetMonthAsText(lastMonth.Month, lastMonth.Year)
                },
                PreviousMonth = new MonthlyData
                {
                    Data = await GetMonthlyEarnings(previousMonth.Month, previousMonth.Year),
                    Label = GetMonthAsText(previousMonth.Month, previousMonth.Year)
                }
            };
        }

        private async Task<ICollection<MonthlyItem>> GetMonthlyEarnings(int month, int year)
        {
            var data = await _httpClient.GetFromJsonAsync<Earning[]>("api/Earnings");
            return data.Where(earning => earning.Date >= new DateTime(year, month, 1)
                && earning.Date <= new DateTime(year, month, LastDayOfMonth(month, year)))
                .GroupBy(earning => earning.Category)
                .Select(earning => new MonthlyItem
                {
                    Amount = earning.Sum(item => item.Amount),
                    Category = earning.Key.ToString()
                })
                .ToList();
        }

        public async Task<ThreeMonthsData> LoadLast3MonthsExpenses()
        {
            var currentMonth = DateTime.Today.Month;
            var lastMonth = DateTime.Today.AddMonths(-1);
            var previousMonth = DateTime.Today.AddMonths(-2);

            return new ThreeMonthsData
            {
                CurrentMonth = new MonthlyData
                {
                    Data = await GetMonthlyExpenses(currentMonth, _currentYear),
                    Label = GetMonthAsText(currentMonth, _currentYear)
                },
                LastMonth = new MonthlyData
                {
                    Data = await GetMonthlyExpenses(lastMonth.Month, lastMonth.Year),
                    Label = GetMonthAsText(lastMonth.Month, lastMonth.Year)
                },
                PreviousMonth = new MonthlyData
                {
                    Data = await GetMonthlyExpenses(previousMonth.Month, previousMonth.Year),
                    Label = GetMonthAsText(previousMonth.Month, previousMonth.Year)
                }
            };
        }

        private async Task<ICollection<MonthlyItem>> GetMonthlyExpenses(int month, int year)
        {
            var data = await _httpClient.GetFromJsonAsync<Expense[]>("api/Expenses");
            return data.Where(expense => expense.Date >= new DateTime(year, month, 1)
                && expense.Date <= new DateTime(year, month, LastDayOfMonth(month, year)))
                .GroupBy(expense => expense.Category)
                .Select(expense => new MonthlyItem
                {
                    Amount = expense.Sum(item => item.Amount),
                    Category = expense.Key.ToString()
                })
                .ToList();
        }

        private static int LastDayOfMonth(int month, int year)
        {
            return DateTime.DaysInMonth(year, month);
        }

        private static string GetMonthAsText(int month, int year)
        {
            return month switch
            {
                1 => $"January {year}",
                2 => $"February {year}",
                3 => $"March {year}",
                4 => $"April {year}",
                5 => $"May {year}",
                6 => $"June {year}",
                7 => $"July {year}",
                8 => $"August {year}",
                9 => $"September {year}",
                10 => $"October {year}",
                11 => $"November {year}",
                12 => $"December {year}",
                _ => throw new NotImplementedException(),
            };
        }
    }
}
