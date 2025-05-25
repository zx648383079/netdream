using NetDream.Shared.Models;

namespace NetDream.Modules.Exam.Models
{
    public class MaterialListItem
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte Type { get; set; }

        public ListArticleItem[]? Question { get; set; }
    }
}
