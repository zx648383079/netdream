using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Comment.Entities
{
    public class CommentEntity : IIdEntity, ICreatedEntity
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string ExtraRule { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public int UserId { get; set; }
        public int AgreeCount { get; set; }
        public int DisagreeCount { get; set; }

        public byte Score { get; set; }
        public int Position { get; set; }

        public int ItemId { get; set; }
        public byte ItemType { get; set; }

        public int FromId { get; set; }
        public byte FromType { get; set; }

        public string GuestName { get; set; } = string.Empty;
        public string GuestEmail { get; set; } = string.Empty;
        public string GuestUrl { get; set; } = string.Empty;

        public byte Status { get; set; }
        public int CreatedAt { get; set; }
    }
}
