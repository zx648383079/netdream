using MediatR;

namespace NetDream.Modules.Counter.Events
{
    public record VisitLog(
        string SessionId, int UserId, 
        string Url, string Referrer, 
        string Ip, string UserAgent, 
        int Timestamp) : INotification
    {
    }
}
