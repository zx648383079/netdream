using NetDream.Modules.Forum.Entities;

namespace NetDream.Modules.Forum.Models
{
    internal interface IWithClassifyModel
    {
        public int ClassifyId { get; }
        public ForumClassifyEntity? Classify { set; }
    }
}