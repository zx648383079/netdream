using System.ComponentModel.DataAnnotations;

namespace NetDream.Shared.Providers.Forms
{
    public class TagForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
