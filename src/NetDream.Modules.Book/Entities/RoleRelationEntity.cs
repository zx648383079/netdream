using NPoco;
namespace NetDream.Modules.Book.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class RoleRelationEntity
    {
        internal const string ND_TABLE_NAME = "book_role_relation";
        public int Id { get; set; }
        [Column("role_id")]
        public int RoleId { get; set; }
        public string Title { get; set; } = string.Empty;
        [Column("role_link")]
        public int RoleLink { get; set; }
    }
}
