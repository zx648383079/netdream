using NetDream.Shared.Providers.Entities;

namespace NetDream.Modules.Blog.Models
{
    public class TagModel: TagEntity
    {
        public int BlogCount { get; set; }

        public string FontSize
        {
            get
            {
                return (BlogCount + 12) + "px";
            }
        }
    }
}
