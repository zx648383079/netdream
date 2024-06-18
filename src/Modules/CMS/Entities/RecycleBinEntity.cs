using NetDream.Core.Interfaces.Entities;
using NPoco;

namespace NetDream.Modules.CMS.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class RecycleBinEntity : IIdEntity, ICreatedEntity
    {
        internal const string ND_TABLE_NAME = "cms_recycle_bin";
        public int Id { get; set; }

        [Column("site_id")]
        public int SiteId { get; set; }

        [Column("model_id")]
        public int ModelId { get; set; }

        [Column("item_type")]
        public byte ItemType { get; set; }

        [Column("item_id")]
        public int ItemId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Remark { get; set; } = string.Empty;

        public string Data { get; set; } = string.Empty;

        [Column("created_at")]
        public int CreatedAt { get; set; }

    }
}
