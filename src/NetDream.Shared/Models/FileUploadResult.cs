namespace NetDream.Shared.Models
{
    public class FileUploadResult
    {
        public string Url { get; set; }
        public long Size { get; set; }
        public string Title { get; set; }
        public string Original { get; set; }

        public string Type { get; set; }

        public string Thumb { get; set; }
    }

    public class FileListItem
    {
        public string Url { get; set; }
        public long Size { get; set; }
        public string Title { get; set; }
        public string Thumb { get; set; }

        public int Mtime { get; set; }
    }
}
