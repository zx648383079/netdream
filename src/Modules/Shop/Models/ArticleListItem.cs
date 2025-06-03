using NetDream.Shared.Models;

namespace NetDream.Modules.Shop.Models
{
    public class ArticleListItem
    {
        public int Id { get; set; }

        public int CatId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }

        public ListLabelItem? Category { get; set; }
    }
}
