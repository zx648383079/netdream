
namespace NetDream.Modules.Shop.Entities
{
    
    public class TrialReportEntity
    {
        
        public int Id { get; set; }
        
        public int ActId { get; set; }
        
        public int UserId { get; set; }
        
        public int GoodsId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
