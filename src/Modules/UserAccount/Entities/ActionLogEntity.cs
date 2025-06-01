using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.UserAccount.Entities
{
    
    public class ActionLogEntity  : IIdEntity, ICreatedEntity
    {
        
        public int Id { get; set; }
        public string Ip { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
    }
}
