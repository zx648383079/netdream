using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Catering.Forms
{
    public class RoleForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
    }
}
