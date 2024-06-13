using NetDream.Modules.Blog.Entities;
using NPoco;

namespace NetDream.Modules.Blog.Models
{
    public class TagModel: TagEntity
    {
        [Ignore]
        public string FontSize
        {
            get
            {
                return (BlogCount + 12) + "px";
            }
        }
    }
}
