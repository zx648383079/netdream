using MediatR;
using NetDream.Modules.Auth.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.Auth.Listeners
{
    public class CancelAccountListener : INotificationHandler<CancelAccount>
    {
        public Task Handle(CancelAccount notification, CancellationToken cancellationToken)
        {
            
            return Task.CompletedTask;
        }
    }
}
