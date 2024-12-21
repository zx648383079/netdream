
namespace NetDream.Modules.Bot.Entities
{
    
    public class UserEntity
    {
        
        public int Id { get; set; }
        public string Openid { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public byte Sex { get; set; }
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        
        public int SubscribeAt { get; set; }
        
        public string UnionId { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        
        public int GroupId { get; set; }
        
        public int UpdatedAt { get; set; }
        
        public int BotId { get; set; }
        
        public string NoteName { get; set; } = string.Empty;
        public byte Status { get; set; }
        
        public byte IsBlack { get; set; }
        
        public int CreatedAt { get; set; }
    }
}
