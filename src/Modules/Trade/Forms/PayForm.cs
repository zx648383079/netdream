using System;

namespace NetDream.Modules.Trade.Forms
{
    public class PayForm
    {
        /// <summary>
        /// 交易单号
        /// </summary>
        public string OutTradeNo { get; set; }
        public string Subject { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public float TotalAmount { get; set; }
        public string NotifyUrl { get; set; }
        public string ReturnUrl { get; set; }
        /// <summary>
        /// 操作员的id
        /// </summary>
        public string OperatorId { get; set; }
        /// <summary>
        /// m 分钟 h 小时 d 天 1c 当天
        /// </summary>
        public string TimeoutExpress { get; set; } = "90m";
        /// <summary>
        /// TimeoutExpress、TimeExpire 二选一
        /// </summary>
        public DateTime? TimeExpire { get; set; }
        public string Body { get; set; }
        public string BuyerId { get; set; }
        public string SellerId { get; set; }
        /// <summary>
        /// 原样返回值
        /// </summary>
        public string PassbackParams { get; set; }
    }
}
