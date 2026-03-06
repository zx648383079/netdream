namespace NetDream.Shared.Models
{
    public enum ReviewStatus: byte
    {
        None,
        Approved = 1,
        /// <summary>
        /// 已读不处理
        /// </summary>
        Seen = 7,
        Rejected = 9,
        Deleted = 99,
    }
}
