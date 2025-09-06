using NetDream.Modules.Counter.Entities;

namespace NetDream.Modules.Counter.Models
{
    public class StayTimeModel : StayTimeLogEntity, IWithClientModel
    {
        public ClientLabelItem? Client { get; set; }
    }
}
