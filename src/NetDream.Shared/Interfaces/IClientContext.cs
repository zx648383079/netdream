using NetDream.Shared.Interfaces.Entities;
using System.Diagnostics.CodeAnalysis;

namespace NetDream.Shared.Interfaces
{
    public interface IClientContext
    {
        public string Ip { get; }
        public string UserAgent { get; }

        public string Host { get; }

        public string Language { get; }

        public int PlatformId { get; }

        public int UserId { get; }

        public string ClientName { get; }

        public int Now { get; }
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool TryGetUser([NotNullWhen(true)] out IUserProfile? user);
    }
}
