using MediatR;

namespace NetDream.Modules.Counter.Events
{
    public record CounterStateLog(
        string SessionId, int UserId,
        string Url, string Referrer,
        string Ip, string UserAgent,
        byte Status,
        int Timestamp) : INotification
    {

        public string Latitude { get; init; } = string.Empty;
        public string Longitude { get; init; } = string.Empty;

        public int EnterAt { get; init; }
        public int LeaveAt { get; init; }
        public int LoadedAt { get; init; }
        public int DisplaySize { get; init; }
        public int Language { get; init; }

        public int LoadTime => LoadedAt == 0 ? 0 : (LoadedAt - EnterAt);

        public int GetTimeOrNow(int time)
        {
            return time > 0 ? time : Timestamp;
        }
    }
}
