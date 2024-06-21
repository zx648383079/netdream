using NetDream.Shared.Interfaces.Entities;
using NPoco;
namespace NetDream.Modules.Shop.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class ArticleCategoryEntity: IIdEntity
    {
        internal const string ND_TABLE_NAME = "shop_article_category";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Column("parent_id")]
        public int ParentId { get; set; }
        public int Position { get; set; }
    }
}
