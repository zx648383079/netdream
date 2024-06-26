using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.SEO.Forms
{
    public class EmojiForm
    {
        public int Id { get; set; }
        public int CatId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; }
        [Required]
        public string Content { get; set; } = string.Empty;
    }

    public class EmojiCategoryForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }
}
