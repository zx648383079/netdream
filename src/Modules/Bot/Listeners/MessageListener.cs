using MediatR;
using NetDream.Modules.Bot.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
