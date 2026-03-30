using NetDream.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Article.Forms
{
    public class ArticleForm : IArticle
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public string Language { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public byte EditType { get; set; }
        [Required]
        public string Content { get; set; } = string.Empty;
        public int CatId { get; set; }
        public byte OriginalType { get; set; }
        public byte OpenType { get; set; }
        public string OpenRule { get; set; } = string.Empty;
        public byte PublishStatus { get; set; }
    }
}
