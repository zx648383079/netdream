using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.OnlineDisk.Forms
{
    public class ServerFileForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string Md5 { get; set; } = string.Empty;
        [Required]
        public string Thumb { get; set; } = string.Empty;
        public long Size { get; set; }
    }
}
