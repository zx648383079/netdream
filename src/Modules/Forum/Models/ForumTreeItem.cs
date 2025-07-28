using NetDream.Modules.Forum.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Forum.Models
{
    public class ForumTreeItem : ForumEntity, ILevelItem
    {
        public int Level {  get; set; }
    }
}
