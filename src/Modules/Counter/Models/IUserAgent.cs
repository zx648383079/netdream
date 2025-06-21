namespace NetDream.Modules.Counter.Models
{
    public interface IUserAgent
    {
        public string UserAgent { get; }
    }

    public interface IUserAgentFormatted : IUserAgent
    {
        /// <summary>
        /// [name, version]
        /// </summary>
        public string[] Device { get; set; }
        public string[] Browser { get; set; }
        public string[] Os { get; set; }
    }
}
