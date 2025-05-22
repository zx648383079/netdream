using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Modules.Trade.Listeners
{
    public class PayRequestHandler : IRequestHandler<PayRequest, PayResult>
    {
        public Task<PayResult> Handle(PayRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
