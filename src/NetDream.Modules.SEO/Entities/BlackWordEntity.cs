using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.SEO.Entities
{
    [TableName("seo_black_word")]
    public class BlackWordEntity
    {
        public int Id { get; set; }
        public string? Words { get; set; }
        [Column("replace_words")]
        public string? ReplaceWords { get; set; }
    }
}
