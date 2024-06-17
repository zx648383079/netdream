using NPoco;
namespace NetDream.Modules.Book.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class BookAuthorEntity
    {
        internal const string ND_TABLE_NAME = "book_author";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Column("user_id")]
        public int UserId { get; set; }
        public int Status { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }

        public BookAuthorEntity()
        {
            
        }

        public BookAuthorEntity(string name)
        {
            Name = name;
        }

        public BookAuthorEntity(string name, int status): this(name)
        {
            Status = status;
        }
    }
}
