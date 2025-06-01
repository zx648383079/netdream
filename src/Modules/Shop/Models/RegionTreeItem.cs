using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.Shop.Models
{
    public class RegionTreeItem : ITreeItem
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }
        public IList<ITreeItem> Children { get; set; }
    }

    public class RegionTreeMapItem : ITreeGroupItem
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        public string Name { get; set; }

        public int Level { get; set; }
        public ITreeGroup Children { get; set; }
    }
}
