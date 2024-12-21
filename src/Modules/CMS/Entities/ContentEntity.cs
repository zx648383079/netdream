
namespace NetDream.Modules.CMS.Entities
{
    public class ContentEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        
        public int CatId { get; set; }
        
        public int ModelId { get; set; }
        
        public int ParentId { get; set; }
        
        public int UserId { get; set; }
        public string Keywords { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte Status { get; set; }
        
        public int ViewCount { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
