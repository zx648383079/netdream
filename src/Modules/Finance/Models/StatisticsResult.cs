using NetDream.Modules.Finance.Entities;
using NetDream.Modules.Finance.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Finance.Models
{
    public class StatisticsResult
    {
        public decimal MoneyTotal { get; set; }
        public decimal ExpenditureTotal { get; set; }
        public decimal ExpenditureCurrent { get; set; }
        public decimal ExpenditureLast { get; set; }
        public int ExpenditureCount { get; set; }

        public decimal IncomeTotal { get; set; }
        public decimal IncomeCurrent { get; set; }
        public decimal IncomeLast { get; set; }
        public int IncomeCount { get; set; }
        public LogStageItem[] StageItems { get; set; }
    }

    public class LogStageItem
    {
        public string Date { get; set; }
        public decimal Income { get; set; }
        public decimal Expenditure { get; set; }
    }

    public class MoneyStatisticsResult
    {
        public MoneyAccountEntity[] AccountList { get; set; }

        public FinancialProductEntity[] ProductList { get; set; }
        public FinancialProjectEntity[] ProjectList { get; set; }

        public decimal Total => AccountList.Sum(i => i.Money + i.FrozenMoney);
    }
    public class IncomeStatisticsResult
    {
        public string Month { get; set; }

       

        public LogEntity[] LogList { get; set; }

        public int DayLength { get; set; }

        public LogEntity[] IncomeList => LogList.Where(i => i.Type == LogRepository.TYPE_INCOME).ToArray();

        public LogEntity[] ExpenditureList => LogList.Where(i => i.Type == LogRepository.TYPE_EXPENDITURE).ToArray();

        public IDictionary<int, decimal> IncomeDays => LogRepository.GetMonthLogs(IncomeList, DayLength);
        public IDictionary<int, decimal> ExpenditureDays => LogRepository.GetMonthLogs(ExpenditureList, DayLength);



    }
}
