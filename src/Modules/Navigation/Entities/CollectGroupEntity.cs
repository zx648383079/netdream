using NPoco;
namespace Modules.Navigation.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class CollectGroupEntity
    {
        internal const string ND_TABLE_NAME = "search_collect_group";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("user_id")]
        public int UserId { get; set; }
        public byte Position { get; set; }
    }
}
