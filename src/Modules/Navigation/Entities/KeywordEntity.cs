using NPoco;
namespace Modules.Navigation.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class KeywordEntity
    {
        internal const string ND_TABLE_NAME = "search_keyword";
        public int Id { get; set; }
        public string Word { get; set; } = string.Empty;
        public byte Type { get; set; }
    }
}
