using NetDream.Modules.Document.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Document.Models
{
    public class FieldTreeItem : FieldEntity, ILevelItem
    {
        public int Level { get; set; }
    }
}
