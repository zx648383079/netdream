using NetDream.Modules.Plan.Entities;

namespace NetDream.Modules.Plan.Models
{
    public class DayListItem : DayEntity, IWithTaskModel
    {
        public TaskLabelItem Task { get; set; }
    }
}
