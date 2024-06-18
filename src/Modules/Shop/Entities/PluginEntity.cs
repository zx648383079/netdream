using NetDream.Core.Interfaces.Entities;
using NPoco;

namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class PluginEntity : IIdEntity, ITimestampEntity
    {
        internal const string ND_TABLE_NAME = "shop_plugin";
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Setting { get; set; } = string.Empty;

        public byte Status { get; set; }

        [Column("updated_at")]
        public int UpdatedAt { get; set; }

        [Column("created_at")]
        public int CreatedAt { get; set; }

    }
}
