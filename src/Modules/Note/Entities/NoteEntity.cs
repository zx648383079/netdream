using NetDream.Core.Interfaces.Entities;
using NPoco;
namespace Modules.Note.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class NoteEntity: IIdEntity, ICreatedEntity
    {
        internal const string ND_TABLE_NAME = "note";
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("is_notice")]
        public byte IsNotice { get; set; }
        public byte Status { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
