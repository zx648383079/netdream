using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Legwork.Models
{
    public class OrderListItem : IWithServiceModel, IWithUserModel, IWithProviderModel, IWithWaiterModel
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public int UserId { get; set; }
        public int ServiceId { get; set; }
        public int Amount { get; set; }
        public decimal OrderAmount { get; set; }
        public int WaiterId { get; set; }
        public byte Status { get; set; }
        public int UpdatedAt { get; set; }
        public int CreatedAt { get; set; }
        public ServiceLabelItem? Service { get; set; }
        public IUser? User { get; set; }
        public IUser? Provider { get; set; }
        public IUser? Waiter { get; set; }
    }
}
