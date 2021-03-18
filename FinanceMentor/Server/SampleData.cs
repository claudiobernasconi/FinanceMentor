using FinanceMentor.Server.Storage;
using FinanceMentor.Shared;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FinanceMentor.Server
{
    public static class SampleData
    {
        public static void AddEarningsRepository(this IServiceCollection services)
        {
            var today = DateTime.Today;
            var lastMonth = DateTime.Today.AddMonths(-1);
            var previousMonth = DateTime.Today.AddMonths(-2);

            var earningRepository = new MemoryRepository<Earning>();

            earningRepository.Add(new Earning { Date = new DateTime(previousMonth.Year, previousMonth.Month, 25), Amount = 2480, Category = EarningCategory.Salary, Subject = "Monthly Salary" });
            earningRepository.Add(new Earning { Date = new DateTime(previousMonth.Year, previousMonth.Month, 12), Amount = 440, Category = EarningCategory.Freelancing, Subject = "Freelancing Client A" });
            earningRepository.Add(new Earning { Date = new DateTime(lastMonth.Year, lastMonth.Month, 25), Amount = 2480, Category = EarningCategory.Salary, Subject = "Monthly Salary" });
            earningRepository.Add(new Earning { Date = new DateTime(lastMonth.Year, lastMonth.Month, 12), Amount = 790, Category = EarningCategory.Freelancing, Subject = "Freelancing Client A" });
            earningRepository.Add(new Earning { Date = new DateTime(lastMonth.Year, lastMonth.Month, 4), Amount = 387, Category = EarningCategory.CapitalGain, Subject = "ETF Dividends" });
            earningRepository.Add(new Earning { Date = new DateTime(today.Year, today.Month, 25), Amount = 2480, Category = EarningCategory.Salary, Subject = "Monthly Salary" });
            earningRepository.Add(new Earning { Date = new DateTime(today.Year, today.Month, 14), Amount = 680, Category = EarningCategory.Freelancing, Subject = "Freelancing Client A" });
            earningRepository.Add(new Earning { Date = new DateTime(today.Year, today.Month, 12), Amount = 245, Category = EarningCategory.Flipping, Subject = "Old TV" });

            services.AddSingleton<IRepository<Earning>>(earningRepository);
        }
    }
}