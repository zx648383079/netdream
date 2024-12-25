using NetDream.Modules.Blog.Entities;
using NetDream.Shared.Interfaces.Entities;
using System.Collections.Generic;

namespace NetDream.Modules.Blog.Models
{
    public class CommentModel: CommentEntity, IWithUserModel
    {
        public IUser? User { get; set; }
        public BlogEntity? Blog { get; set; }

        public IList<CommentModel>? Replies { get; set; }
    }
}
