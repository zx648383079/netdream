using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Legwork.Models
{
    public interface IWithWaiterModel
    {
        public int WaiterId { get; }

        public IUser? Waiter { set; }
    }
}