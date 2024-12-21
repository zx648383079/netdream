using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Forum.Entities
{
    
    public class ForumModeratorEntity: IIdEntity
    {
        
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        public int ForumId { get; set; }
        
        public int RoleId { get; set; }
    }
}
