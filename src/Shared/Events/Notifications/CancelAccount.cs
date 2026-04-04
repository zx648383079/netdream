using NetDream.Shared.Interfaces;

namespace NetDream.Shared.Events.Notifications
{
    public record CancelAccount(IUserProfile User, int Timestamp) : INotification
    {
    }
}
