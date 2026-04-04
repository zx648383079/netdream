using NetDream.Shared.Interfaces;
using System.Collections.Generic;

namespace NetDream.Modules.Article.Models
{
    public class ArchiveListItem
    {
        public int Year { get; set; }

        public IList<ArchiveLabelItem> Children { get; set; } = [];
    }

    public class ArchiveLabelItem : IListArticleItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int CreatedAt { get; set; }
    }
}
