namespace NetDream.Modules.Plan.Forms
{
    public class CommentForm
    {
        public int TaskId { get; set; }
        public string Content { get; set; } = string.Empty;
        public byte Type { get; set; }
    }
}
