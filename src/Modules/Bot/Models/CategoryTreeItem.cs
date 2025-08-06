using NetDream.Modules.Bot.Entities;
using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.Bot.Models
{
    public class CategoryTreeItem : EditorCategoryEntity, ITreeItem
    {
        public IList<ITreeItem> Children { get; set; } = [];
        public int Level { get; set; }
    }
}
