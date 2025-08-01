using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.OnlineDisk.Models
{
    public class ShareListItem : IWithUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte Mode { get; set; }
        public int UserId { get; set; }

        public int DeathAt { get; set; }

        public int ViewCount { get; set; }

        public int DownCount { get; set; }

        public int SaveCount { get; set; }

        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }
        public IUser? User { get; set; }
    }
}
