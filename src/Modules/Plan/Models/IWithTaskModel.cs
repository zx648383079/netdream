namespace NetDream.Modules.Plan.Models
{
    internal interface IWithTaskModel
    {
        public int TaskId { get; }

        public TaskLabelItem? Task { set; }
    }
}