using NetDream.Modules.Finance.Entities;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Finance.Models
{
    public class BudgetStatistics(BudgetEntity model)
    {
        public BudgetEntity Data => model;

        public SortedDictionary<int, float> LogList { get; set; } = [];

        public float Sum => LogList.Values.Sum();

        public float BudgetSum => LogList.Count * Data.Budget;

    }
}
