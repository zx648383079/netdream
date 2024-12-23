
using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.SEO.Forms
{
    
    public class WordForm
    {
        
        public int Id { get; set; }
        [Required]
        public string Words { get; set; } = string.Empty;
        
        public string ReplaceWords { get; set; } = string.Empty;
    }
}
