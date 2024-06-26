using NetDream.Modules.SEO.Entities;
using NPoco;

namespace NetDream.Modules.SEO.Models
{
    public class EmojiModel: EmojiEntity
    {
        [Ignore]
        public EmojiCategoryEntity? Category { get; set; }
    }
}
