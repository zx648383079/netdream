using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Bot.Entities
{
    
    public class BotEntity : IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        
        public string AccessToken { get; set; } = string.Empty;
        public string Account { get; set; } = string.Empty;
        public string Original { get; set; } = string.Empty;
        public byte Type { get; set; }
        
        public byte PlatformType { get; set; }
        public string Appid { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        
        public string AesKey { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Qrcode { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public byte Status { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int CreatedAt { get; set; }
        
        public int UserId { get; set; }
    }
}
