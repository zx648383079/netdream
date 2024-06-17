using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class PermissionEntity
    {
        internal const string ND_TABLE_NAME = "rbac_permission";
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        [Column("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Column("updated_at")]
        public int UpdatedAt { get; set; }

        [Column("created_at")]
        public int CreatedAt { get; set; }

    }
}
