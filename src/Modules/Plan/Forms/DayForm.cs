using System.ComponentModel.DataAnnotations;

namespace NetDream.Modules.Plan.Forms
{
    public class DayForm
    {
        public int Id { get; set; }
        [Required]
        public int TaskId { get; set; }

        public int Amount { get; set; } = 1;
    }
}
