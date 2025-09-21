using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.UserIdentity.Forms
{
    public class ZoneForm
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public byte IsOpen { get; set; }

        public byte Status { get; set; }
    }
}
