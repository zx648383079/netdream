using NetDream.Shared.Interfaces;

namespace NetDream.Shared.Models
{
    public class EnvironmentConfiguration : IEnvironment
    {
        public const string EnvironmentKey = "Environment";
        public string Root { get; set; } = string.Empty;

        public string PublicRoot { get; set; } = string.Empty;

        public string AssetRoot { get; set; } = string.Empty;

        public string OnlineDiskRoot { get; set; } = string.Empty;

        public string Deeplink { get; set; } = "zodream";

        public string AllowedHosts { get; } = string.Empty;
    }
}
