namespace NetDream.Shared.Interfaces
{
    public interface IEnvironment
    {
        public string Root { get; }
        /// <summary>
        /// 缓存目录
        /// </summary>
        public string CacheRoot { get; }
        /// <summary>
        /// 备份文件目录
        /// </summary>
        public string BackupRoot { get; }
        /// <summary>
        /// 网址公开目录
        /// </summary>
        public string PublicRoot { get; }
        /// <summary>
        /// 可访问的资源目录
        /// </summary>
        public string AssetRoot { get; }
        /// <summary>
        /// 内部文件夹
        /// </summary>
        public string OnlineDiskRoot { get; }
        public string Deeplink { get; }
        /// <summary>
        /// 配置 cors 跨域的域名
        /// </summary>
        public string AllowedHosts { get; }

    }
}
