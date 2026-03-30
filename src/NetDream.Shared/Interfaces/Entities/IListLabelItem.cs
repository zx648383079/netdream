namespace NetDream.Shared.Interfaces.Entities
{
    /// <summary>
    /// 用在列表的附属标记
    /// </summary>
    public interface IListLabelItem
    {
        public int Id { get; }

        public string Name { get; }
    }

    public interface IListArticleItem
    {
        public int Id { get; }

        public string Title { get; }
    }
}
