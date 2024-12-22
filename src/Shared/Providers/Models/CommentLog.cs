using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Models
{
    public class CommentLog : CommentEntity
    {
        public byte AgreeType { get; set; }

        public CommentLog()
        {
            
        }

        public CommentLog(CommentEntity entity)
        {
            Id = entity.Id;
            Content = entity.Content;
            ParentId = entity.ParentId;
            TargetId = entity.TargetId;
            CreatedAt = entity.CreatedAt;
            ExtraRule = entity.ExtraRule;
            UserId = entity.UserId;
            AgreeCount = entity.AgreeCount;
            DisagreeCount = entity.DisagreeCount;
        }
    }
}
