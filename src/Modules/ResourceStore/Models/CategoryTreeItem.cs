using NetDream.Modules.ResourceStore.Entities;
using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.ResourceStore.Models
{
    public class CategoryTreeItem : CategoryEntity, ITreeItem
    {
        public IList<ITreeItem> Children { get; set; } = [];
        public int Level { get; set; }
    }
}
