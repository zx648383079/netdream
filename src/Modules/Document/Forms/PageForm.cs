using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Document.Forms
{
    public class PageForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int ProjectId { get; set; }

        public int VersionId { get; set; }

        public int ParentId { get; set; }
        public byte Type { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
