using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Auth.Forms
{
    public interface ICodeRequestForm
    {
        public string ToType { get; set; }

        public string To { get; set; }

        public string Event { get; set; }
    }

    public class CodeRequestForm: ICodeRequestForm
    {
        [Required]
        public string ToType { get; set; }
        [Required]
        public string To { get; set; }
        [Required]
        public string Event { get; set; }
    }

    public class CodeVerifyForm: ICodeRequestForm
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
