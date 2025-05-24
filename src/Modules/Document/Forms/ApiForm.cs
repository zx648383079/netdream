using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Document.Forms
{
    public class ApiForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
        public string Uri { get; set; } = string.Empty;

        public int ProjectId { get; set; }

        public int VersionId { get; set; }
        public byte Type { get; set; }
        public string Description { get; set; } = string.Empty;

        public int ParentId { get; set; }

        public FieldForm[] Header { get; set; }
        public FieldForm[] Request { get; set; }
        public FieldForm[] Response { get; set; }

    }
}
