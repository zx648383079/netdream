using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Finance.Forms
{
    public class LogForm
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
        public string OutTradeNo { get; set; } = string.Empty;
        public string TradingObject { get; set; } = string.Empty;
    }

    public class LogPartialForm
    {
        public float Money { get; set; }
        public string Remark { get; set; } = string.Empty;
        public string TradingObject { get; set; } = string.Empty;
        /// <summary>
        /// HH:mm:ss
        /// </summary>
        public string Time { get; set; }

    }
}
