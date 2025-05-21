using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Finance.Models
{
    public class LogListItem
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public byte Type { get; set; }
        public float Money { get; set; }
        public float FrozenMoney { get; set; }
        public int AccountId { get; set; }
        public int ChannelId { get; set; }
        public int ProjectId { get; set; }
        public int BudgetId { get; set; }
        public string Remark { get; set; } = string.Empty;
        public DateTime HappenedAt { get; set; }
        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }
    }
}
