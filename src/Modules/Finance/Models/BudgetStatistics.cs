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
}
