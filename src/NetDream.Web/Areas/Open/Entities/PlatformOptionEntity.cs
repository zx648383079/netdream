using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetDream.Web.Areas.Open.Entities
{
    [TableName("open_platform_option")]
    public class PlatformOptionEntity
    {
        public int Id { get; set; }
        [Column("platform_id")]
        public int PlatformId { get; set; }
        public string Store { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
    }
}
