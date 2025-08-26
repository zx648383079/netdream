using MediatR;
using System.Collections.Generic;

namespace NetDream.Shared.Notifications
{
    public record SendCodeRequest(string Target, string Template, string Code, string Nickname, string Timestamp)
        : IRequest<bool>
    {
    }

    public record SendTextRequest(string Target, string Template, IDictionary<string, string> Data, string Timestamp)
        : IRequest<bool>
    {
    }
}
