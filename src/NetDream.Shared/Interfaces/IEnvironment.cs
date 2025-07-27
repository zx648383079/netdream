namespace NetDream.Shared.Interfaces
{
    public interface IEnvironment
    {
        public string Root { get; }
        public string CacheRoot { get; }

        public string PublicRoot { get; }
        public string AssetRoot { get; }
        public string OnlineDiskRoot { get; }
        public string Deeplink { get; }
        /// <summary>
        /// 配置 cors 跨域的域名
        /// </summary>
        public string AllowedHosts { get; }

    }
}
