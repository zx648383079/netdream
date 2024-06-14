using NPoco;
namespace Modules.MessageService.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class LogEntity
    {
        internal const string ND_TABLE_NAME = "ms_log";
        public int Id { get; set; }
        [Column("template_id")]
        public int TemplateId { get; set; }
        [Column("target_type")]
        public byte TargetType { get; set; }
        public string Target { get; set; } = string.Empty;
        [Column("template_name")]
        public string TemplateName { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public byte Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Ip { get; set; } = string.Empty;
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
