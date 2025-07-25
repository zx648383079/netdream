namespace NetDream.Modules.Forum.Models
{
    internal interface IWithForumModel
    {
        public int ForumId { get; }
        public ForumLabelItem? Forum { set; }
    }
}