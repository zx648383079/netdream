using NetDream.Core.Interfaces.Entities;
using NPoco;

namespace NetDream.Modules.OnlineDisk.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class DiskEntity : IIdEntity, ITimestampEntity
    {
        internal const string ND_TABLE_NAME = "disk";
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Extension { get; set; } = string.Empty;

        [Column("file_id")]
        public int FileId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("left_id")]
        public int LeftId { get; set; }

        [Column("right_id")]
        public int RightId { get; set; }

        [Column("parent_id")]
        public int ParentId { get; set; }

        [Column("deleted_at")]
        public int DeletedAt { get; set; }

        [Column("updated_at")]
        public int UpdatedAt { get; set; }

        [Column("created_at")]
        public int CreatedAt { get; set; }

    }
}
