using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.OnlineDisk.Forms
{
    public class ServerForm
    {
        public int Id { get; set; }
        [Required]
        public string Ip { get; set; } = string.Empty;
        public int Port { get; set; }

        public byte CanUpload { get; set; }

        public string UploadUrl { get; set; } = string.Empty;
        [Required]
        public string DownloadUrl { get; set; } = string.Empty;
        [Required]
        public string PingUrl { get; set; } = string.Empty;
    }

    public class ServerLinkForm
    {
        public int Server { get; set; }

        public string Ip { get; set; }
        public int Port { get; set; }

        public string UploadUrl { get; set; } = string.Empty;
        [Required]
        public string DownloadUrl { get; set; } = string.Empty;
        [Required]
        public string PingUrl { get; set; } = string.Empty;

        public string[] Files { get; set; }
        public string Token { get; internal set; }
    }

    public class ServerSourceForm
    {
        public bool Linked { get; set; }

        [Required]
        public string ServerUrl { get; set; }

        public string UploadUrl { get; set; } = string.Empty;
        [Required]
        public string DownloadUrl { get; set; } = string.Empty;
        [Required]
        public string PingUrl { get; set; } = string.Empty;
    }
}
