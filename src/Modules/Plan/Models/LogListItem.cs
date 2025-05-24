using NetDream.Modules.Plan.Entities;

namespace NetDream.Modules.Plan.Models
{
    public class LogListItem : LogEntity, IWithTaskModel
    {
        public TaskLabelItem? Task { get; set; }
    }
}
