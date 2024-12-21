
namespace NetDream.Modules.Book.Entities
{
    
    public class RoleRelationEntity
    {
        
        public int Id { get; set; }
        
        public int RoleId { get; set; }
        public string Title { get; set; } = string.Empty;
        
        public int RoleLink { get; set; }
    }
}
