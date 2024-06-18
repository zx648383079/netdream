using NPoco;
namespace NetDream.Modules.OnlineDisk.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ServerEntity
    {
        internal const string ND_TABLE_NAME = "disk_server";
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        [Column("can_upload")]
        public byte CanUpload { get; set; }
        [Column("upload_url")]
        public string UploadUrl { get; set; } = string.Empty;
        [Column("download_url")]
        public string DownloadUrl { get; set; } = string.Empty;
        [Column("ping_url")]
        public string PingUrl { get; set; } = string.Empty;
        [Column("file_count")]
        public int FileCount { get; set; }
        public byte Status { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
