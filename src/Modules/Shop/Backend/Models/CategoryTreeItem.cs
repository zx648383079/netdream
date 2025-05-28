using NetDream.Shared.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Shop.Backend.Models
{
    public class CategoryTreeItem : ITreeItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public IList<ITreeItem> Children { get; set; }
        public int Level { get; set; }
    }
}
