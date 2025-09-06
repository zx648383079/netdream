namespace NetDream.Modules.Counter.Models
{
    public class ClientLabelItem : IUserAgentFormatted
    {
        public int Id { get; set; }
        public string Ip {  get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public string[] Device { get; set; }
        public string[] Browser { get; set; }
        public string[] Os { get; set; }
    }
}
