using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.UserIdentity.Forms
{
    public class BindingCardForm
    {
        [Required]
        public int User { get; set; }
        [Required]
        public int Card { get; set; }

        [Required]
        public string ExpiredAt { get; set; }
    }
}
