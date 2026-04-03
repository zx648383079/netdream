using System.Threading;
using System.Threading.Tasks;

namespace NetDream.Shared.Events
{
    public interface IEventBus
    {
        /// <summary>
        /// 下发通知，单对多
        /// </summary>
        /// <typeparam name="TNotification"></typeparam>
        /// <param name="notification"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task Publish<TNotification>(TNotification notification, CancellationToken token = default) where TNotification : INotification;
        /// <summary>
        /// 发送请求，并获取结果，单对单
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken token = default);
    }
}
