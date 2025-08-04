namespace NetDream.Modules.OnlineService.Models
{
    public class MessageQueryResult
    {
        public MessageListItem[] Data { get; set; }
        public string SessionToken { get; set; }
        public int NextTime { get; set; }
    }
}
