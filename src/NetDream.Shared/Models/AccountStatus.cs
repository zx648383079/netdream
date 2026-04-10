namespace NetDream.Shared.Models
{
    public enum AccountStatus: byte
    {
        /// <summary>
        /// 已删除
        /// </summary>
        Deleted = 0,
        /// <summary>
        /// 账户已冻结
        /// </summary>
        Frozen = 2,
        /// <summary>
        /// 邮箱注册，未确认邮箱
        /// </summary>
        Unconfirm = 9,
        /// <summary>
        /// 账户正常
        /// </summary>
        Active = 10,
        /// <summary>
        /// 账户正常&实名认证了
        /// </summary>
        ActiveVerified = 15
    }
}
