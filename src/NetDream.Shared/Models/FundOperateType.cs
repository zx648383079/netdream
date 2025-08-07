namespace NetDream.Shared.Models
{
    public enum FundOperateType : byte
    {
        /// <summary>
        /// 系统自动
        /// </summary>
        System = 1,
        /// <summary>
        /// 用户充值
        /// </summary>
        Recharge = 6,
        /// <summary>
        /// 管理员充值
        /// </summary>
        Admin = 9,
        /// <summary>
        /// 分销
        /// </summary>
        Affiliate = 15,
        PostBuy = 21,
        ForumBuy = 25,
        CheckIn = 30,
        Bank = 31,
        Game = 40,
        Shopping = 60,
        Default = 99,
    }
}
