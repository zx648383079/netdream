using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Shop.Backend.Forms
{
    public class ArticleForm
    {
        public int Id { get; set; }

        public int CatId { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Brief { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string File { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
