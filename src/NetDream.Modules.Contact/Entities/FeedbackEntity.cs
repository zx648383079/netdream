using NPoco;
namespace NetDream.Modules.Contact.Entities
{
    [TableName(ND_TABLE_NAME)]
    public class FeedbackEntity
    {
        internal const string ND_TABLE_NAME = "cif_feedback";
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Status { get; set; }
        [Column("open_status")]
        public int OpenStatus { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
