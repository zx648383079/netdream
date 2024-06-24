using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.OpenPlatform.Forms
{
    public class PlatformForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte Type { get; set; }
        [Required]
        public string Domain { get; set; } = string.Empty;
        public byte SignType { get; set; }
        public string SignKey { get; set; } = string.Empty;
        public byte EncryptType { get; set; }
        public string PublicKey { get; set; } = string.Empty;
        public int AllowSelf { get; set; }
    }
}
