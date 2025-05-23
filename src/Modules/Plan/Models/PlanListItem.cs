using NetDream.Modules.Plan.Entities;

namespace NetDream.Modules.Plan.Models
{
    public class PlanListItem : PlanEntity, IWithTaskModel
    {
        public TaskLabelItem? Task {  get; set; }
    }
}
