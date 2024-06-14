using NPoco;
namespace Modules.OnlineDisk.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class FileEntity
    {
        internal const string ND_TABLE_NAME = "disk_file";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string Md5 { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
        [Column("deleted_at")]
        public int DeletedAt { get; set; }
    }
}
