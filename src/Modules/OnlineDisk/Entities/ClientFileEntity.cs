using NPoco;
namespace NetDream.Modules.OnlineDisk.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ClientFileEntity
    {
        internal const string ND_TABLE_NAME = "disk_client_file";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string Md5 { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int Size { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
