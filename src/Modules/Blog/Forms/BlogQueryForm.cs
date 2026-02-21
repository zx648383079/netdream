using NetDream.Shared.Models;

namespace NetDream.Modules.Blog.Forms
{
    public class BlogQueryForm : QueryForm
    {
        public int Category { get; set; }
        public int User { get; set; }
        public string Tag { get; set; } = string.Empty;

        public int Status { get; set; }
        public int Type { get; set; }
        public string Language { get; set; } = string.Empty;
        public string ProgrammingLanguage { get; set; } = string.Empty;
        public string Pl { get; set; } = string.Empty;
    }
}
