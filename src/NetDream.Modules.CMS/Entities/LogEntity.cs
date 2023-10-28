using NPoco;
namespace NetDream.Modules.CMS.Entities
{
    public class LogEntity
    {
        public int Id { get; set; }
        [Column("model_id")]
        public int ModelId { get; set; }
        [Column("item_type")]
        public byte ItemType { get; set; }
        [Column("item_id")]
        public int ItemId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public int Action { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
