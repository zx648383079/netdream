using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Providers.Entities;
using System.Text.Json;

namespace NetDream.Shared.Providers.Models
{
    public class CommentListItem : IWithUserModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public LinkExtraRule[]? ExtraRule { get; set; }
        public int ParentId { get; set; }
        public int UserId { get; set; }
        public int TargetId { get; set; }
        public int AgreeCount { get; set; }
        public int DisagreeCount { get; set; }

        public byte Status { get; set; }
        public int CreatedAt { get; set; }
        public IUser? User { get; set; }

        public int ReplyCount { get; set; }

        public byte AgreeType { get; set; }

        public CommentListItem()
        {
            
        }

        public CommentListItem(CommentEntity entity)
        {
            Id = entity.Id;
            Content = entity.Content;
            if (string.IsNullOrWhiteSpace(entity.ExtraRule))
            {
                ExtraRule = JsonSerializer.Deserialize<LinkExtraRule[]>(entity.ExtraRule);
            }
            UserId = entity.UserId;
            TargetId = entity.TargetId;
            ParentId = entity.ParentId;
            AgreeCount = entity.AgreeCount;
            DisagreeCount = entity.DisagreeCount;
            CreatedAt = entity.CreatedAt;
        }
    }
}
