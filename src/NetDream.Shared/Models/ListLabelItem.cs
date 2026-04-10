using NetDream.Shared.Interfaces;

namespace NetDream.Shared.Models
{
    public class ListLabelItem : IListLabelItem
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ListLabelItem()
        {
            
        }

        public ListLabelItem(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class ListStatisticsItem : ListLabelItem, IListStatisticsItem
    {
        public int Count { get; set; }

        public ListStatisticsItem()
        {
            
        }

        public ListStatisticsItem(int id, string name): base(id, name)
        {
        }
    }

    public class ListArticleItem : IListArticleItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public ListArticleItem()
        {

        }

        public ListArticleItem(int id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
