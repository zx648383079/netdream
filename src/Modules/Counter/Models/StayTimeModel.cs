using NetDream.Modules.Counter.Entities;

namespace NetDream.Modules.Counter.Models
{
    public class StayTimeModel : StayTimeLogEntity, IUserAgentFormatted
    {
        public string[] Device { get; set; }
        public string[] Browser { get; set; }
        public string[] Os { get; set; }
    }
}
