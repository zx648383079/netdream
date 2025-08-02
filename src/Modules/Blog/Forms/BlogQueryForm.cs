using NetDream.Shared.Models;

namespace NetDream.Modules.Blog.Forms
{
    public class BlogQueryForm : QueryForm
    {
        public int Category { get; set; }
        public int User { get; set; }
        public string Tag { get; set; }

        public int Status { get; set; }
        public int Type { get; set; }
        public string Language { get; set; }
        public string ProgrammingLanguage { get; set; }
        public string Pl { get; set; }
    }
}
