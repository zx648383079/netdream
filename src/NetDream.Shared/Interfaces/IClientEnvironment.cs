namespace NetDream.Shared.Interfaces
{
    public interface IClientEnvironment
    {
        public string Ip { get; }
        public string UserAgent { get; }

        public string Language { get; }

        public int PlatformId { get; }

        public int UserId { get; }

        public string ClientName { get; }

        public int Now { get; }
    }
}
