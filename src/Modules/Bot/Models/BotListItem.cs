using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Bot.Models
{
    public class BotListItem : IWithUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Qrcode { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public byte Status { get; set; }

        public int UpdatedAt { get; set; }

        public int CreatedAt { get; set; }

        public int UserId { get; set; }
        public IUser? User { get; set; }
    }
}
