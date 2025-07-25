using NetDream.Shared.Models;

namespace NetDream.Modules.Forum.Models
{
    internal interface IWithThreadModel
    {
        public int ThreadId { get; }
        public ListArticleItem? Thread { set; }
    }
}