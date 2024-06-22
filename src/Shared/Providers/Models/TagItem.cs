using NetDream.Shared.Interfaces.Entities;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Shared.Providers.Models
{
    public class TagItem: IIdEntity
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }

    public class TagLinkItem
    {
        [Column("tag_id")]
        public int TagId { get; set; }
        [Column("target_id")]
        public int TargetId { get; set; }
    }
}
