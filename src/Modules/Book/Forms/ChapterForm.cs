namespace NetDream.Modules.Book.Forms
{
    public class ChapterForm
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public int Price { get; set; }
        public int Status { get; set; }
    }
}
