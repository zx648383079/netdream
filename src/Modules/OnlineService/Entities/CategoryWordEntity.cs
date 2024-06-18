using NPoco;
namespace Modules.OnlineService.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class CategoryWordEntity
    {
        internal const string ND_TABLE_NAME = "service_category_word";
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        [Column("cat_id")]
        public int CatId { get; set; }
    }
}
