using NPoco;
namespace NetDream.Modules.Book.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BookRoleEntity
    {
        internal const string ND_TABLE_NAME = "book_role";
        public int Id { get; set; }
        [Column("book_id")]
        public int BookId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Character { get; set; } = string.Empty;
        public string X { get; set; } = string.Empty;
        public string Y { get; set; } = string.Empty;
    }
}
