using NetDream.Modules.Book.Entities;
using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.Book.Models
{
    public class ChapterModel: ChapterEntity
    {
        public string Content { get; set; } = string.Empty;

        public bool IsBought { get; set; }

        public ChapterEntity? Previous { get; set; }
        public ChapterEntity? Next { get; set; }
    }

    public class ChapterTreeItem : ChapterEntity, ITreeItem
    {
        public IList<ITreeItem> Children { get; set; } = [];
        public int Level { get; set; }
    }
}
