using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.OpenPlatform.Forms
{
    public class TokenForm
    {
        [Required]
        public int Platform { get; set; }
        public string ExpiredAt { get; set; }
    }
}
