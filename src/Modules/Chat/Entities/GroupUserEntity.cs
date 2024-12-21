
namespace NetDream.Modules.Chat.Entities
{
    
    public class GroupUserEntity
    {
        
        public int Id { get; set; }
        
        public int GroupId { get; set; }
        
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public int RoleId { get; set; }
        public int Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
