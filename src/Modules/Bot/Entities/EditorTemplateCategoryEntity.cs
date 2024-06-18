using NPoco;
namespace Modules.Bot.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class EditorTemplateCategoryEntity
    {
        internal const string ND_TABLE_NAME = "bot_editor_template_category";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("parent_id")]
        public int ParentId { get; set; }
    }
}
