using NetDream.Modules.MicroBlog.Entities;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.MicroBlog.Models
{
    public class PostListItem : BlogEntity, IWithUserModel
    {
        public AttachmentEntity[] Attachments { get; set; }
        public IUser? User { get; set; }

        public bool IsCollected { get; set; }

        public PostListItem()
        {
            
        }

        public PostListItem(BlogEntity entity)
        {
            Id = entity.Id;
            Content = entity.Content;
            ExtraRule = entity.ExtraRule;
            CommentCount = entity.CommentCount;
            RecommendCount = entity.RecommendCount;
            CollectCount = entity.CollectCount;
            ForwardId = entity.ForwardId;
            ForwardCount = entity.ForwardCount;
            UserId = entity.UserId;
            Source = entity.Source;
            OpenType = entity.OpenType;
            UpdatedAt = entity.UpdatedAt;
            CreatedAt = entity.CreatedAt;
        }
    }
}
