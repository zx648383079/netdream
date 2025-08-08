using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Auth.Forms
{
    public class CodeGenerateForm
    {
        [Required]
        public int Amount { get; set; }
        public string ExpiredAt { get; set; }
    }
}
