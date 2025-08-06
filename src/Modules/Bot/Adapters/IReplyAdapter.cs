using NetDream.Shared.Interfaces;
using NetDream.Shared.Interfaces.Entities;

namespace NetDream.Modules.Bot.Adapters
{
    public interface IReplyAdapter
    {
        public int PlatformId { get; }

        public string AuthType { get; }

        public IOperationResult Listen();

        public IOperationResult Receive();

        public IOperationResult Reply(object data);

        public IUser AuthUser(string openId);
        public int AuthUserId(string openId);
    }
}
