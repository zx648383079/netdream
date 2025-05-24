using NetDream.Modules.Document.Entities;
using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.Document.Models
{
    public class CategoryTreeItem : CategoryEntity, ITreeItem
    {
        public IList<ITreeItem> Children { get; set; } = [];
        public int Level { get; set; }
    }
}
