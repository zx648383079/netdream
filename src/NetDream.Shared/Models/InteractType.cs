namespace NetDream.Shared.Models
{
    public enum InteractType: byte
    {
        None = 0,
        Agree,
        Disagree,
        Collect,
        Like,
        Dislike,
        /// <summary>
        /// 打赏
        /// </summary>
        Reward,
        /// <summary>
        /// 购买
        /// </summary>
        Bought,
    }

    public enum RecordToggleType : byte
    {
        Deleted,
        Updated,
        Added,
    }
}
