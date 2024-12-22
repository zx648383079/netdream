using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Context
{
    public interface ICommentContext: ILogContext
    {

        public DbSet<CommentEntity> Comments { get; set; }
    }
}
