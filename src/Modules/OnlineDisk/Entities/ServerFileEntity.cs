using NPoco;
namespace Modules.OnlineDisk.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ServerFileEntity
    {
        internal const string ND_TABLE_NAME = "disk_server_file";
        [Column("server_id")]
        public int ServerId { get; set; }
        [Column("file_id")]
        public int FileId { get; set; }
    }
}
