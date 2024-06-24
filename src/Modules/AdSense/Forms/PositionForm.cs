using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.AdSense.Forms
{
    public class PositionForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Code { get; set; } = string.Empty;
        public byte AutoSize { get; set; }
        public byte SourceType { get; set; }
        public string Width { get; set; } = string.Empty;
        public string Height { get; set; } = string.Empty;
        public string Template { get; set; } = string.Empty;
        public byte Status { get; set; }
    }
}
