using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.SEO.Entities
{
    [TableName("base_file_quote")]
    public class FileQuoteEntity
    {
        public int Id { get; set; }
        [Column("file_id")]
        public int FileId { get; set; }
        [Column("item_type")]
        public int ItemType { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
