using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class RolePermissionEntity
    {
        internal const string ND_TABLE_NAME = "rbac_role_permission";
        [Column("role_id")]
        public int RoleId { get; set; }

        [Column("permission_id")]
        public int PermissionId { get; set; }

    }
}
