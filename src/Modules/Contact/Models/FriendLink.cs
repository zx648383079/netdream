using NetDream.Modules.Contact.Entities;
using NetDream.Shared.Converters;
using System.Text.Json.Serialization;

namespace NetDream.Modules.Contact.Models
{
    public class FriendLink
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Brief { get; set; } = string.Empty;

        [JsonConverter(typeof(TimestampConverter))]
        public int CreatedAt { get; set; }

        public FriendLink()
        {
            
        }

        public FriendLink(FriendLinkEntity entity)
        {
            Name = entity.Name;
            Url = entity.Url;
            Logo = entity.Logo;
            Brief = entity.Brief;
            CreatedAt = entity.CreatedAt;
        }
    }
}
