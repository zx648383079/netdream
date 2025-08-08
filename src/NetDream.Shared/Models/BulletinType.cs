namespace NetDream.Shared.Models
{
    public enum BulletinType : byte
    {
        Agree = 6,
        At = 7,
        Comment = 8,

        ChatAt = 88,
        Message = 96,
        /// <summary>
        /// 附加通知
        /// </summary>
        Additional = 98,
        Other = 99
    }
}
