using MediatR;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Shared.Notifications
{
    public record CancelAccount(IUserProfile User, int Timestamp) : INotification
    {
    }
}
