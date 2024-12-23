using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.SEO.Entities
{
    
    public class EmojiCategoryEntity: IIdEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        public ICollection<EmojiEntity>? Items { get; set; }
    }
}
