namespace NetDream.Shared.Interfaces
{
    public interface ISystemFeedback
    {
        /// <summary>
        /// 举报内容
        /// </summary>
        /// <param name="itemType"></param>
        /// <param name="itemId"></param>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public int Report(byte itemType, int itemId, string content, string title);
    }
}
