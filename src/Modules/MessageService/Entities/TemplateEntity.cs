using NPoco;
namespace NetDream.Modules.MessageService.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class TemplateEntity
    {
        internal const string ND_TABLE_NAME = "ms_template";
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string Data { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        [Column("target_no")]
        public string TargetNo { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
