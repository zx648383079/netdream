using NPoco;
namespace Modules.Navigation.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class TagEntity
    {
        internal const string ND_TABLE_NAME = "search_tag";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
