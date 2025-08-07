using MediatR;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;

namespace NetDream.Shared.Notifications
{
    public class FundChangeRequest : IFundRequest, IRequest<int>
    {
        public int Payee { get; init; }

        public int Payer { get; init; }

        public int SourceId { get; init; }

        public FundOperateType SourceType { get; init; }

        public decimal Value { get; init; }

        public string Remark { get; init; } = string.Empty;

        /// <summary>
        /// 支付/收款
        /// </summary>
        /// <returns></returns>
        public static FundChangeRequest Create(int user, decimal money, string remark, FundOperateType sourceType, int sourceId)
        {
            if (money < 0)
            {
                return CreateDeduct(user, money, remark, sourceType, sourceId);
            }
            return CreateReceive(user, money, remark, sourceType, sourceId);
        }

        /// <summary>
        /// 向其他账户付款/收款
        /// </summary>
        /// <param name="payer"></param>
        /// <param name="payee"></param>
        /// <param name="money"></param>
        /// <param name="remark"></param>
        /// <param name="sourceType"></param>
        /// <param name="sourceId"></param>
        /// <returns></returns>
        public static FundChangeRequest CreatePay(int payer, int payee, decimal money, string remark, FundOperateType sourceType, int sourceId)
        {
            if (money < 0)
            {
                return new FundChangeRequest
                {
                    Payer = payee,
                    Payee = payer,
                    Remark = remark,
                    SourceType = sourceType,
                    SourceId = sourceId,
                    Value = -money,
                };
            }
            return new FundChangeRequest
            {
                Payer = payer,
                Payee = payee,
                Remark = remark,
                SourceType = sourceType,
                SourceId = sourceId,
                Value = money,
            };
        }

        /// <summary>
        /// 收款
        /// </summary>
        /// <returns></returns>
        public static FundChangeRequest CreateReceive(int payee, decimal money, string remark, FundOperateType sourceType, int sourceId)
        {
            return new FundChangeRequest
            {
                Payer = 0,
                Payee = payee,
                Remark = remark,
                SourceType = sourceType,
                SourceId = sourceId,
                Value = money,
            };
        }

        /// <summary>
        /// 扣款
        /// </summary>
        /// <returns></returns>
        public static FundChangeRequest CreateDeduct(int payer, decimal money, string remark, FundOperateType sourceType, int sourceId)
        {
            return new FundChangeRequest
            {
                Payer = payer,
                Payee = 0,
                Remark = remark,
                SourceType = sourceType,
                SourceId = sourceId,
                Value = money,
            };
        }
    }
}
