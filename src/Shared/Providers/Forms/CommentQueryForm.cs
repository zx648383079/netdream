using NetDream.Shared.Models;

namespace NetDream.Shared.Providers.Forms
{
    public class CommentQueryForm : QueryForm
    {
        public int User { get; set; }
        public int Target { get; set; }
        public int Parent { get; set; } = -1;
    }
}
