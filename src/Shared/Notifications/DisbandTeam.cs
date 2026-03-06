using MediatR;

namespace NetDream.Shared.Notifications
{
    public record DisbandTeam(int TeamId, int Timestamp): INotification
    {

    }
}
