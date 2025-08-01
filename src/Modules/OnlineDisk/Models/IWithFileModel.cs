namespace NetDream.Modules.OnlineDisk.Models
{
    public interface IWithFileModel
    {
        public int FileId { get; }
        public FileLabelItem? File { set; }
    }
}