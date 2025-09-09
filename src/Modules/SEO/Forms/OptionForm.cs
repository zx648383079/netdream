using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.SEO.Forms
{
    public class OptionForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int ParentId { get; set; }
        [Required]
        public string Type { get; set; } = string.Empty;
        public int Visibility { get; set; }
        public string DefaultValue { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public int Position { get; set; }
    }

    public class OptionBatchForm
    {
        public Dictionary<int, string>? Option { get; set; }
        public OptionForm? Field { get; set; }
    }
}
