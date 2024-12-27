using NetDream.Shared.Converters;
using NetDream.Shared.Interfaces.Entities;
using System.Text.Json.Serialization;

namespace NetDream.Modules.Contact.Entities
{
    
    public class FriendLinkEntity: IIdEntity, ITimestampEntity
    {
        
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Brief { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Status { get; set; }
        
        public int UserId { get; set; }
        [JsonConverter(typeof(TimestampConverter))]
        public int UpdatedAt { get; set; }
        [JsonConverter(typeof(TimestampConverter))]
        public int CreatedAt { get; set; }
    }
}
