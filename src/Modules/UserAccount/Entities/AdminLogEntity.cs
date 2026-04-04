using NetDream.Shared.Interfaces;

namespace NetDream.Modules.UserAccount.Entities
{
    /// <summary>
    /// 管理员审核记录，删除用户、文章等
    /// </summary>
    public class AdminLogEntity : IIdEntity, ICreatedEntity
    {
        
        public int Id { get; set; }
        public string Ip { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        
        public int ItemType { get; set; }
        
        public int ItemId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        
        public int CreatedAt { get; set; }
    }
}
