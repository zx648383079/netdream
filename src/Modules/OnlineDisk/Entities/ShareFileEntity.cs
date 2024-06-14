using NPoco;
namespace Modules.OnlineDisk.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ShareFileEntity
    {
        internal const string ND_TABLE_NAME = "disk_share_file";
        public int Id { get; set; }
        [Column("disk_id")]
        public int DiskId { get; set; }
        [Column("share_id")]
        public int ShareId { get; set; }
    }
}
