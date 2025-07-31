using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OpenPlatform.Models
{
    public class PlatformListItem : IWithUserModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte Type { get; set; }
        public string Domain { get; set; } = string.Empty;
        public string Appid { get; set; } = string.Empty;
        public byte Status { get; set; }
        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }
        public IUser? User { get; set; }
    }
}
