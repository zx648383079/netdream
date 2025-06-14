using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Document.Forms
{
    public class FieldForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Title { get; set; } = string.Empty;

        public bool IsRequired { get; set; }

        public string DefaultValue { get; set; } = string.Empty;
        public string Mock { get; set; } = string.Empty;

        public int ParentId { get; set; }

        public int ApiId { get; set; }
        public int Kind { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;

        public FieldForm[]? Children { get; set; }
    }
}
