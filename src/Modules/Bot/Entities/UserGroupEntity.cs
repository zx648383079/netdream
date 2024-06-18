using NPoco;
namespace Modules.Bot.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class UserGroupEntity
    {
        internal const string ND_TABLE_NAME = "bot_user_group";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("bot_id")]
        public int BotId { get; set; }
        [Column("tag_id")]
        public string TagId { get; set; } = string.Empty;
    }
}
