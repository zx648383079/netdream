using NetDream.Shared.Interfaces;
using System;

namespace NetDream.Modules.Bot.Adapters
{
    public interface IPlatformAdapter : IReplyAdapter
    {
        public IOperationResult PullUser(string openId);

        public IOperationResult PullUsers(Action<object> cb);

        public IOperationResult PushMenu(object[] items);

        public IOperationResult PushQr(object model);

        public IOperationResult SendUsers(string content);
        public IOperationResult SendGroup(string group, string content);
        public IOperationResult SendAnyUsers(string[] openid, string content);

        public IOperationResult SendTemplate(string openid, object data);

        public IOperationResult PullTemplate(Action<object> cb);

        public IOperationResult PushMedia(object model);
        public IOperationResult PushNews(object model);

        public IOperationResult PullMedia(string type);

        public IOperationResult DeleteMedia(string mediaId);
    }
}
