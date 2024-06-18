using NPoco;
namespace Modules.OnlineService.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class CategoryEntity
    {
        internal const string ND_TABLE_NAME = "service_category";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
