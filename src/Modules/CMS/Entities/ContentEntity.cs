using NPoco;
namespace NetDream.Modules.CMS.Entities
{
    public class ContentEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        [Column("cat_id")]
        public int CatId { get; set; }
        [Column("model_id")]
        public int ModelId { get; set; }
        [Column("parent_id")]
        public int ParentId { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        public string Keywords { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte Status { get; set; }
        [Column("view_count")]
        public int ViewCount { get; set; }
        [Column("updated_at")]
        public int UpdatedAt { get; set; }
        [Column("created_at")]
        public int CreatedAt { get; set; }
    }
}
