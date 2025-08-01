namespace NetDream.Modules.OnlineDisk.Models
{
    public class DiskListItem : IWithFileModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public int FileId { get; set; }
        public int UserId { get; set; }
        public int ParentId { get; set; }
        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }

        public FileLabelItem? File { get; set; }
    }
}
