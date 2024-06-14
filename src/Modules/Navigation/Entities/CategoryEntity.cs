using NPoco;
namespace Modules.Navigation.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class CategoryEntity
    {
        internal const string ND_TABLE_NAME = "search_category";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        [Column("parent_id")]
        public int ParentId { get; set; }
    }
}
