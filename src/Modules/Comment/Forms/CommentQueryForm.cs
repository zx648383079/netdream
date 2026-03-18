using NetDream.Shared.Models;

namespace NetDream.Modules.Comment.Forms
{
    public class CommentQueryForm : QueryForm
    {
        public int User { get; set; }
        public int TargetType { get; set; }
        public int TargetId { get; set; }
        public int Parent { get; set; } = -1;
    }
}
