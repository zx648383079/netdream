using NetDream.Shared.Models;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace NetDream.Shared.Interfaces
{
    public interface IKeepAliveService
    {

        public bool TryGet(int user, [NotNullWhen(true)] out IKeepAliveClient? client);

        public bool TryGet(string guestToken, [NotNullWhen(true)] out IKeepAliveClient? client);
        public bool TryGet(int user, string token, string ip, [NotNullWhen(true)] out IKeepAliveClient? client);
    }

    public interface IKeepAliveClient
    {
        public Task SendAsync(ModuleTargetType type, object message);
        /// <summary>
        /// API 取出
        /// </summary>
        /// <returns></returns>
        public object Pop();
    }
}
