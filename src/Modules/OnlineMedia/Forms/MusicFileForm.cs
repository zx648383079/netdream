namespace NetDream.Modules.OnlineMedia.Forms
{
    public class MusicFileForm
    {
        public int Id { get; set; }
        public int MusicId { get; set; }
        public byte FileType { get; set; }
        public string File { get; set; } = string.Empty;
    }

    public class MovieFileForm
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public byte FileType { get; set; }
        public string File { get; set; } = string.Empty;
    }
}
