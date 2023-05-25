using NPoco;
namespace NetDream.Modules.SEO.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class FileEntity
    {
        internal const string ND_TABLE_NAME = "base_file";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string Md5 { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public int Folder { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
