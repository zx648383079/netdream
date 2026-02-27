using NetDream.Modules.Finance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Finance.Models
{
    public class BudgetStatistics(BudgetEntity model)
    {
        public BudgetEntity Data => model;

        public SortedDictionary<int, decimal> LogList { get; set; } = [];

        public decimal Sum => LogList.Values.Sum();

        public decimal BudgetSum => LogList.Count * Data.Budget;

    }

    public class BudgetMonthStatistics(int id, string name)
    {
        public BudgetMonthStatistics(BudgetEntity model)
            : this(model.Id, model.Name)
        {
            
        }
        public int Id => id;
        public string Name => name;

        public LogSutotalItem[] Items { get; set; } = [];

        public decimal Total => Items.Select(i => i.Money).Sum();

    }

    public class LogSutotalItem
    {
        public string Date { get; set; }
        public decimal Money { get; set; }
    }
}
