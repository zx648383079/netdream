using NetDream.Shared.Models;

namespace NetDream.Modules.MicroBlog.Forms
{
    public class PostQueryForm : QueryForm
    {
        public int User { get; set; }

        public int Topic { get; set; }
        public int Id { get; set; }
    }
}
