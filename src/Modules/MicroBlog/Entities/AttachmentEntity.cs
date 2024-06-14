using NPoco;
namespace Modules.MicroBlog.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class AttachmentEntity
    {
        internal const string ND_TABLE_NAME = "micro_attachment";
        public int Id { get; set; }
        [Column("micro_id")]
        public int MicroId { get; set; }
        public string Thumb { get; set; } = string.Empty;
        public string File { get; set; } = string.Empty;
    }
}
