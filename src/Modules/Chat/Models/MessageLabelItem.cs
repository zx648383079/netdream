namespace NetDream.Modules.Chat.Models
{
    public class MessageLabelItem
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Content { get; set; } = string.Empty;
        public int CreatedAt { get; set; }
    }
}
