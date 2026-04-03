namespace NetDream.Shared.Models
{
    public enum PublishStatus: byte
    {
        /// <summary>
        /// 草稿
        /// </summary>
        Draft,
        /// <summary>
        /// 已发布
        /// </summary>
        Posted = 5,
        /// <summary>
        /// 自动保存
        /// </summary>
        AutoSave = 8,
        /// <summary>
        /// 垃圾箱
        /// </summary>
        Deleted = 9,
    }
}
