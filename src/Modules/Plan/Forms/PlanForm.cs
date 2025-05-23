using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Plan.Forms
{
    public class PlanForm
    {
        public int Id { get; set; }
        [Required]
        public int TaskId { get; set; }
        public byte PlanType { get; set; }
        [Required]
        public string PlanTime { get; set; }
        public byte Amount { get; set; }
        public byte Priority { get; set; }
    }
}
