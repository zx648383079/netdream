namespace NetDream.Modules.OnlineService.Forms
{
    public class MessageQueryForm
    {
        public int SessionId { get; set; }
        public string SessionToken { get; set; }
        public int StartTime { get; set; }
        public int LastId { get; set; }
    }
}
