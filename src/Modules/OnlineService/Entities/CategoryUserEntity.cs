using NPoco;
namespace Modules.OnlineService.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class CategoryUserEntity
    {
        internal const string ND_TABLE_NAME = "service_category_user";
        public int Id { get; set; }
        [Column("cat_id")]
        public int CatId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
