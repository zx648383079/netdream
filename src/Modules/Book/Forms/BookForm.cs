namespace NetDream.Modules.Book.Forms
{
    public class BookForm
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Cover { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int AuthorId { get; set; }

        public int Classify { get; set; }

        public int CatId { get; set; }
    }
}
