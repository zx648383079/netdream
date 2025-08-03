namespace NetDream.Modules.Chat.Models
{
    public class PingResult
    {
        public int MessageCount { get; set; }

        public int ApplyCount { get; set; }

        public int NextTime { get; set; }

        public MessageGroupItem[] Data { get; set; }
    }

    public class MessageGroupItem
    {
        public int Type { get; set; }
        public int Id { get; set; }

        public MessageListItem[] Items { get; set; }
    }

    public class MessageQueryResult
    {
        public int NextTime { get; set; }

        public MessageListItem[] Data { get; set; }
    }
}
