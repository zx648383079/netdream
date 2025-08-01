using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Contact.Forms
{
    public class SubscribeForm
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
