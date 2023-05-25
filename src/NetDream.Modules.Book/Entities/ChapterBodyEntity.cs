using NPoco;
namespace NetDream.Modules.Book.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ChapterBodyEntity
    {
        internal const string ND_TABLE_NAME = "book_chapter_body";
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
