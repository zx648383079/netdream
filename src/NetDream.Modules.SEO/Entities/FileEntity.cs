using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.SEO.Entities
{
    [TableName("base_file")]
    public class FileEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Extension { get; set; }
        public string? Md5 { get; set; }
        public string? Path { get; set; }
        public string? Size { get; set; }
        public int Folder { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
