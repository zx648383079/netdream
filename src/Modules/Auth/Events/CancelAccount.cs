using MediatR;
using NetDream.Modules.Auth.Entities;

namespace NetDream.Modules.Auth.Events
{
    public record CancelAccount(UserEntity User, int Timestamp) : INotification
    {
    }
}
