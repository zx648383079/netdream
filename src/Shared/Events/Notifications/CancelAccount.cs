using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Shared.Events.Notifications
{
    public record CancelAccount(IUserProfile User, int Timestamp) : INotification
    {
    }
}
