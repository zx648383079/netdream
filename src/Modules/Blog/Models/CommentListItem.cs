using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.Blog.Models
{
    public class CommentListItem : IWithUserModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;

        public string ExtraRule { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string Url { get; set; } = string.Empty;

        public int ParentId { get; set; }
        public int Position { get; set; }

        public int UserId { get; set; }

        public int BlogId { get; set; }

        public int AgreeCount { get; set; }

        public int DisagreeCount { get; set; }
        public int Approved { get; set; }

        public int CreatedAt { get; set; }

        public IUser? User { get; set; }
        public IListLabelItem? Blog { get; set; }

        public IList<CommentListItem>? Replies { get; set; }
    }
}
