namespace NetDream.Shared.Events.Notifications
{
    public record DisbandTeam(int TeamId, int Timestamp): INotification
    {

    }
}
