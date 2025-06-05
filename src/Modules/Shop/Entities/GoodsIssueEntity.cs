using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Shop.Entities
{
    
    public class GoodsIssueEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        
        public int GoodsId { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        
        public int AskId { get; set; }
        
        public int AnswerId { get; set; }
        public byte Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
