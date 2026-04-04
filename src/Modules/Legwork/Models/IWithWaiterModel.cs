using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Legwork.Models
{
    public interface IWithWaiterModel
    {
        public int WaiterId { get; }

        public IUser? Waiter { set; }
    }
}