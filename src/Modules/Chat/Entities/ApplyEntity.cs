
namespace NetDream.Modules.Chat.Entities
{
    
    public class ApplyEntity
    {
        
        public int Id { get; set; }
        
        public int ItemType { get; set; }
        
        public int ItemId { get; set; }
        public string Remark { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        public int Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
