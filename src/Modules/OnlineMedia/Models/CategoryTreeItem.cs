using NetDream.Modules.OnlineMedia.Entities;
using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.OnlineMedia.Models
{
    public class CategoryTreeItem : CategoryEntity, ITreeItem
    {
        public IList<ITreeItem> Children { get; set; } = [];
        public int Level { get; set; }
    }
}
