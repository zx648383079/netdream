using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.ResourceStore.Forms
{
    public class FileForm
    {
        public int Id { get; set; }
        [Required]
        public int ResId { get; set; }
        public byte FileType { get; set; }
        [Required]
        public string File { get; set; } = string.Empty;
    }

    public class ResourceFileForm
    {
        public int Id { get; set; }
        public byte FileType { get; set; }
        [Required]
        public string File { get; set; } = string.Empty;
    }
}
