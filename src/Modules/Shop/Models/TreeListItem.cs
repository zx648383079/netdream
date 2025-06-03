using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.Shop.Models
{
    public class TreeListItem : ITreeItem
    {
        
        

        public int Id { get; set; }

        public int ParentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Level { get; set; }
        public IList<ITreeItem> Children { get; set; }
    }
}
