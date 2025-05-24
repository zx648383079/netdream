using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.Document.Models
{
    public class PageTreeItem : ITreeItem
    {
        
        public int Level { get; set; }

        public int Id { get; set; }

        public int ParentId { get; set; }
        public string Name { get; set; }
        public byte Type { get; set; }

        public IList<ITreeItem> Children { get; set; }
    }
}
