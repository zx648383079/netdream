using NPoco;
namespace NetDream.Modules.Book.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class CategoryEntity
    {
        internal const string ND_TABLE_NAME = "book_category";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("created_at")]
        public int CreatedAt { get; set; }

        public CategoryEntity()
        {
            
        }

        public CategoryEntity(string name)
        {
            Name = name;
        }
    }
}
