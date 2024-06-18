using NPoco;
namespace Modules.Bot.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class TemplateEntity
    {
        internal const string ND_TABLE_NAME = "bot_template";
        public int Id { get; set; }
        [Column("bot_id")]
        public int BotId { get; set; }
        [Column("template_id")]
        public string TemplateId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Example { get; set; } = string.Empty;
    }
}
