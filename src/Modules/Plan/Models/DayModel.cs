using NetDream.Modules.Plan.Entities;

namespace NetDream.Modules.Plan.Models
{
    public class DayModel : DayEntity
    {
        public TaskEntity? Task { get; set; }
    }
}
