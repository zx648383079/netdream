using NetDream.Modules.OnlineDisk.Entities;

namespace NetDream.Modules.OnlineDisk.Models
{
    public class FileListItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string Md5 { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public long Size { get; set; }

        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }

        public int DeletedAt { get; set; }

        public FileListItem()
        {
            
        }

        public FileListItem(FileEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Extension = entity.Extension;
            Md5 = entity.Md5;
            DeletedAt = entity.DeletedAt;
            Size = entity.Size;
            Thumb = entity.Thumb;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
        }

        public FileListItem(ClientFileEntity entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Extension = entity.Extension;
            Md5 = entity.Md5;
            Size = entity.Size;
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
        }
    }
}
