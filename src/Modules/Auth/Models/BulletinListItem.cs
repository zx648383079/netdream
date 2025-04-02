using NetDream.Modules.Auth.Entities;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using System.Text.Json;

namespace NetDream.Modules.Auth.Models
{
    public class BulletinListItem : IWithUserModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;
        public LinkExtraRule[]? ExtraRule { get; set; }

        public byte Type { get; set; }
        public int UserId { get; set; }

        public int CreatedAt { get; set; }
        public IUser? User { get; set; }

        public BulletinListItem()
        {
            
        }

        public BulletinListItem(BulletinEntity entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            Content = entity.Content;
            Type = entity.Type;
            UserId = entity.UserId;
            CreatedAt = entity.CreatedAt;
            ExtraRule = JsonSerializer.Deserialize<LinkExtraRule[]>(entity.ExtraRule, LinkRule.SerializeOptions);
        }
    }

    public class BulletinUserListItem
    {
        public int Id { get; set; }
        public int BulletinId { get; set; }
        public byte Status { get; set; }
        public int CreatedAt { get; set; }
        public BulletinListItem? Bulletin { get; set; }

        public BulletinUserListItem()
        {
            
        }

        public BulletinUserListItem(BulletinUserEntity entity)
        {
            Id = entity.Id;
            BulletinId = entity.Id;
            Status = entity.Status;
            CreatedAt = entity.CreatedAt;
        }
    }
}
