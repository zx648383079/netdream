
namespace NetDream.Modules.Auth.Entities
{
    
    public class UserMetaEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
