using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.SEO.Entities
{
    [TableName("seo_emoji_category")]
    public class EmojiCategoryEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Icon { get; set; }
    }
}
