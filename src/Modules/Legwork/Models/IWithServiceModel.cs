namespace NetDream.Modules.Legwork.Models
{
    public interface IWithServiceModel
    {
        public int ServiceId { get; }

        public ServiceLabelItem? Service { set; }
    }
}
