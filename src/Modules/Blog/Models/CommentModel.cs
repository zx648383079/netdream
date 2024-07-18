using NetDream.Modules.Blog.Entities;
using NetDream.Shared.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Blog.Models
{
    public class CommentModel: CommentEntity, IWithUserModel
    {
        public IUser? User { get; set; }
        public BlogEntity? Blog { get; set; }

        public IList<CommentModel>? Replies { get; set; }
    }
}
