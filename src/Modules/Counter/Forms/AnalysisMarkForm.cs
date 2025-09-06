using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Counter.Forms
{
    public class AnalysisMarkForm
    {
        public int Type { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
