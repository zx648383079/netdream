
namespace NetDream.Modules.Auth.Entities
{
    
    public class MailLogEntity
    {
        
        public int Id { get; set; }
        public string Ip { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        public int Type { get; set; }
        public string Code { get; set; } = string.Empty;
        public int Amount { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
