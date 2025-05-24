using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Document.Forms
{
    public class ProjectForm
    {
        public int Id { get; set; }
        public uint CatId { get; internal set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
        public string Cover { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
    }
}
