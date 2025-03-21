using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.Blog.Models
{
    public class BlogListItem : IWithUserModel, IWithCategoryModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public string Description { get; set; } = string.Empty;

        public string ProgrammingLanguage { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;

        public int UserId { get; set; }

        public int TermId { get; set; }
        public int OpenType { get; set; }

        public int RecommendCount { get; set; }

        public int CommentCount { get; set; }

        public int ClickCount { get; set; }
        public int CreatedAt { get; set; }

        public IListLabelItem? Term { get; set; }
        public IUser? User { get; set; }

        public bool IsLocalization { get; set; }
    }
}
