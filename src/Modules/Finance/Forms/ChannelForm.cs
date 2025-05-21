using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Finance.Forms
{
    public class ChannelForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
