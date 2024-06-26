using NetDream.Shared.Interfaces.Entities;
using NPoco;
namespace NetDream.Modules.Contact.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class SubscribeEntity: IIdEntity, ITimestampEntity
    {
        internal const string ND_TABLE_NAME = "cif_subscribe";
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Status { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
