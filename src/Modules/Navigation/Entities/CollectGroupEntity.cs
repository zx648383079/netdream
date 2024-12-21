
namespace NetDream.Modules.Navigation.Entities
{
    
    public class CollectGroupEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        public byte Position { get; set; }
    }
}
