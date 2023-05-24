using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.SEO.Entities
{
    [TableName("seo_emoji")]
    public class EmojiEntity
    {
        public int Id { get; set; }
        [Column("cat_id")]
        public int CatId { get; set; }
        public string? Name { get; set; }
        public int Type { get; set; }
        public string? Content { get; set; }
    }
}
