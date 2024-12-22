using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Shared.Providers.Entities
{
    public class CommentEntity : IIdEntity, ICreatedEntity
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string ExtraRule { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public int UserId { get; set; }
        public int TargetId { get; set; }
        public int AgreeCount { get; set; }
        public int DisagreeCount { get; set; }

        public byte Status { get; set; }
        public int CreatedAt { get; set; }
        
    }
}
