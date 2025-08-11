using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Auth.Forms
{
    public class QrAuthorizeForm
    {
        [Required]
        public string Token { get; set; }

        public bool Confirm { get; set; }

        public bool Reject { get; set; }
    }
}
