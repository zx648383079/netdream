using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Shared.Events
{
    public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public Task<TResponse> Handle(TRequest request, CancellationToken token);
    }
}
