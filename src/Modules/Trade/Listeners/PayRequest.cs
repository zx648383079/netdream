using MediatR;
using System;

namespace NetDream.Modules.Trade.Listeners
{
    public class PayRequest : IRequest<PayResult>
    {
        /// <summary>
        /// 支付方式 code
        /// </summary>
        public string Payment { get; set; } = string.Empty;
        public byte SourceType { get; set; }
        /// <summary>
        /// 订单编号
        /// </summary>
        public int SourceId { get; set; }

        public int BuyerId { get; set; }

        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }
        /// <summary>
        /// web app
        /// </summary>
        public string ReferenceType { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
