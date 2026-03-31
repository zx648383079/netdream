using NetDream.Modules.Comment.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using System.Text.Json;

namespace NetDream.Modules.Comment.Models
{
    public class CommentListItem : IWithUserModel, ICommentItem
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public LinkExtraRule[]? ExtraRule { get; set; }
        public int ParentId { get; set; }
        public int UserId { get; set; }
        public int AgreeCount { get; set; }
        public int DisagreeCount { get; set; }

        public byte Status { get; set; }
        public int CreatedAt { get; set; }
        public IUser? User { get; set; }

        public int ReplyCount { get; set; }

        public byte AgreeType { get; set; }
        public CommentListItem[] Replies { get; internal set; } = [];

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
            ParentId = entity.ParentId;
            AgreeCount = entity.AgreeCount;
            DisagreeCount = entity.DisagreeCount;
            CreatedAt = entity.CreatedAt;
            if (UserId == 0)
            {
                User = new GuestUser()
                {
                    Email = entity.GuestEmail,
                    Name = entity.GuestName,
                    Url = entity.GuestUrl
                };
            }
        }
    }
}
