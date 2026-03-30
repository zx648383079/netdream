using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.Article.Models
{
    public class CategoryTreeItem : ITreeItem, IListLabelItem
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IList<ITreeItem> Children { get; set; } = [];
        public int Level { get; set; }
    }
}
