namespace NetDream.Modules.Bot.Models
{
    public class MediaListItem
    {
        public int Id { get; set; }

        public int BotId { get; set; }
        public string Type { get; set; } = string.Empty;

        public byte MaterialType { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;

        public int ParentId { get; set; }

        public string MediaId { get; set; } = string.Empty;

        public int CreatedAt { get; set; }
    }
}
