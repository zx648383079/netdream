using NetDream.Modules.Bot.Events;
using NetDream.Shared.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.Bot.Listeners
{
    public class MessageListener(BotContext db) : INotificationHandler<MessageRequest>
    {
        public Task Handle(MessageRequest notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
