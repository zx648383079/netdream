using NPoco;
namespace Modules.Bot.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class EditorTemplateEntity
    {
        internal const string ND_TABLE_NAME = "bot_editor_template";
        public int Id { get; set; }
        public byte Type { get; set; }
        [Column("cat_id")]
        public int CatId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
