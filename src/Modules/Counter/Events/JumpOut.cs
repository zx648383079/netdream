using MediatR;

namespace NetDream.Modules.Counter.Events
{
    public record JumpOutLog(
        string SessionId, 
        string Url, string Referrer, 
        string Ip, string UserAgent, 
        int Timestamp) : INotification
    {
    }
}
