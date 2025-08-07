using NetDream.Shared.Models;

namespace NetDream.Shared.Interfaces
{
    public interface IFundAccount
    {
        /// <summary>
        /// 是否购买
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public bool IsBought(int sourceId, FundOperateType sourceType);
        /// <summary>
        /// 创建交易
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public IFundOperation<T> Trading<T>(T data) where T : IFundRequest;
    }

    public interface IFundOperation<T> 
        where T : IFundRequest
    {

        public T Source { get; }

        public IOperationResult Commit();

        public IOperationResult Rollback();
    }

    public interface IFundRequest
    {
        /// <summary>
        /// 收款人
        /// </summary>
        public int Payee { get; }
        /// <summary>
        /// 付款人
        /// </summary>
        public int Payer { get; }
        public int SourceId { get; }
        public FundOperateType SourceType { get; }
        /// <summary>
        /// 变动金额
        /// </summary>
        public decimal Value { get; }

        public string Remark { get; }
    }
}
