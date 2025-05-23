using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.ResourceStore.Forms
{
    public class ResourceForm
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int Size { get; set; }
        public float Score { get; set; }
        public byte PreviewType { get; set; }
        public int CatId { get; set; }
        public int Price { get; set; }
        public byte IsCommercial { get; set; }
        public byte IsReprint { get; set; }
    }
}
