using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Auth.Forms
{
    public class CodeRequestForm
    {
        [Required]
        public string ToType { get; set; }
        [Required]
        public string To { get; set; }
        [Required]
        public string Event { get; set; }
    }

    public class CodeVerifyForm
    {
        [Required]
        public string ToType { get; set; }
        [Required]
        public string To { get; set; }
        [Required]
        public string Event { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
