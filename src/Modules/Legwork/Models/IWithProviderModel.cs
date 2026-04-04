using NetDream.Shared.Interfaces;

namespace NetDream.Modules.Legwork.Models
{
    public interface IWithProviderModel
    {
        public int ProviderId { get; }

        public IUser? Provider { set; }
    }
}