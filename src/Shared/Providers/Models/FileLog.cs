using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Migrations;
using NPoco;

namespace NetDream.Shared.Providers.Models
{
    public class FileItem: IIdEntity, ITimestampEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Extension { get; set; }
        public string Md5 { get; set; }
        public string Path { get; set; }
        public byte Folder { get; set; }
        public long Size { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
    public class FileLog : IIdEntity, ICreatedEntity
    {
        public int Id { get; set; }

        [Column("file_id")]
        public int FileId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        [Column("item_type")]
        public byte ItemType { get; set; }

        public string Data { get; set; } = string.Empty;
        [Column(MigrationTable.COLUMN_CREATED_AT)]
        public int CreatedAt { get; set; }
    }

    public class FileQuoteLog : IIdEntity, ICreatedEntity
    {
        public int Id { get; set; }
        [Column("file_id")]
        public int FileId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        [Column("item_type")]
        public byte ItemType { get; set; }

        [Column(MigrationTable.COLUMN_CREATED_AT)]
        public int CreatedAt { get; set; }
    }

    public class FileOperationResult
    {

        public string Title { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public long Size { get; set; }

        public FileOperationResult()
        {
            
        }

        public FileOperationResult(FileItem item)
        {
            Title = item.Name;
            Extension = "." + item.Extension;
            Size = item.Size;
            Url = item.Path;
        }
    }
}
