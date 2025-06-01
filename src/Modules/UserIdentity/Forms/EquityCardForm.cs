using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.UserIdentity.Forms
{
    public class EquityCardForm
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public string Configure { get; set; } = string.Empty;
    }
}
