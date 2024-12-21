
namespace NetDream.Modules.Navigation.Entities
{
    
    public class CollectEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        
        public int GroupId { get; set; }
        
        public int UserId { get; set; }
        public byte Position { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
