using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Shop.Backend.Forms
{
    public class ActivityForm
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Thumb { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int ScopeType { get; set; }
        /// <summary>
        /// , 连接的 int[]
        /// </summary>
        public string Scope { get; set; } = string.Empty;
        public string Configure { get; set; } = string.Empty;
        public int StartAt { get; set; }

        public int EndAt { get; set; }
    }
}
