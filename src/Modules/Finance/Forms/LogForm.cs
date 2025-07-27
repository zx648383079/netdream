using System;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Finance.Forms
{
    public class LogForm
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public byte Type { get; set; }
        public decimal Money { get; set; }
        public decimal FrozenMoney { get; set; }
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
        public decimal Money { get; set; }
        public string Remark { get; set; } = string.Empty;
        public string TradingObject { get; set; } = string.Empty;
        /// <summary>
        /// HH:mm:ss
        /// </summary>
        public string Time { get; set; }

    }

    public class BatchLogForm
    {
        public string Keywords { get; set; }

        public int AccountId { get; set; }
        public int ProjectId { get; set; }
        public int ChannelId { get; set; }
        public int BudgetId { get; set; }
    }

    public class DayLogForm
    {
        [Required]
        public string Day { get; set; }

        public int AccountId { get; set; }
        public int ProjectId { get; set; }
        public int ChannelId { get; set; }
        public int BudgetId { get; set; }

        public LogPartialForm? Breakfast { get; set; }
        public LogPartialForm? Lunch { get; set; }
        public LogPartialForm? Dinner { get; set; }
    }
}
