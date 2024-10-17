using NetDream.Modules.Blog.Entities;
using NetDream.Shared.Interfaces.Entities;
using NPoco;

namespace NetDream.Modules.Blog.Models
{
    public class BlogModel: BlogEntity
    {
        [Ignore]
        public CategoryEntity? Term { get; set; }
        [Ignore]
        public IUser? User { get; set; }

        [Ignore]
        public bool IsLocalization { get; set; }
    }
}
