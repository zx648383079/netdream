using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Plan.Forms
{
    public class TaskForm
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public byte Status { get; set; }
        public int EveryTime { get; set; }
        public byte SpaceTime { get; set; }
        public int StartAt { get; set; }
        public byte PerTime { get; set; }
        public int TimeLength { get; set; }
    }
}
