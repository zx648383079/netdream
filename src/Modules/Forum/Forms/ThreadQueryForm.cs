using NetDream.Shared.Models;

namespace NetDream.Modules.Forum.Forms
{
    public class ThreadQueryForm : QueryForm
    {
        public int Forum { get; set; }
        public int Classify { get; set; }
        public int Type { get; set; }
        public int User { get; set; }
    }

    public class PostQueryForm : QueryForm
    {
        public int Thread { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public int Post { get; set; }
        public int User { get; set; }
    }
}
