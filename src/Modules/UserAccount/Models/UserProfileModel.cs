using NetDream.Shared.Converters;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NetDream.Modules.UserAccount.Models
{
    public class UserProfileModel : UserListItem
    {
        [JsonIgnore]
        public int BulletinCount => MetaItems?.TryGetValue("bulletin_count", out var i) == true ? (int)i : 0;

        [JsonMeta]
        public Dictionary<string, object>? MetaItems { get; set; }
    }
}
