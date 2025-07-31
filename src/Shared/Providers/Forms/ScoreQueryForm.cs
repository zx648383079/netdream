using NetDream.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace NetDream.Shared.Providers.Forms
{
    public class ScoreQueryForm  : QueryForm
    {
        public int User { get; set; }
        public byte ItemType { get; set; }
        [Required]
        public int ItemId { get; set; }

        public byte FromType { get; set; }
        public int FromId { get; set; }
    }
}
