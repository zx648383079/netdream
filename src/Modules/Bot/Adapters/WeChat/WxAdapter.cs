using NetDream.Modules.Bot.Forms;
using NetDream.Shared.Interfaces;
using System;

namespace NetDream.Modules.Bot.Adapters.WeChat
{
    public class WxAdapter : IPlatformAdapter
    {
        public int PlatformId => throw new NotImplementedException();

        public string AuthType => throw new NotImplementedException();

        public IUser AuthUser(string openId)
        {
            throw new NotImplementedException();
        }

        public int AuthUserId(string openId)
        {
            throw new NotImplementedException();
        }

        public IOperationResult DeleteMedia(string mediaId)
        {
            throw new NotImplementedException();
        }

        public IOperationResult Listen()
        {
            throw new NotImplementedException();
        }

        public IOperationResult PullMedia(string type)
        {
            throw new NotImplementedException();
        }

        public IOperationResult PullTemplate(Action<TemplateForm> cb)
        {
            throw new NotImplementedException();
        }

        public IOperationResult PullUser(string openId)
        {
            throw new NotImplementedException();
        }

        public IOperationResult PullUsers(Action<FansForm> cb)
        {
            throw new NotImplementedException();
        }

        public IOperationResult PushMedia(object model)
        {
            throw new NotImplementedException();
        }

        public IOperationResult PushMenu(object[] items)
        {
            throw new NotImplementedException();
        }

        public IOperationResult PushNews(object model)
        {
            throw new NotImplementedException();
        }

        public IOperationResult PushQr(object model)
        {
            throw new NotImplementedException();
        }

        public IOperationResult Receive()
        {
            throw new NotImplementedException();
        }

        public IOperationResult Reply(object data)
        {
            throw new NotImplementedException();
        }

        public IOperationResult SendAnyUsers(string[] openid, string content)
        {
            throw new NotImplementedException();
        }

        public IOperationResult SendGroup(string group, string content)
        {
            throw new NotImplementedException();
        }

        public IOperationResult SendTemplate(string openid, object data)
        {
            throw new NotImplementedException();
        }

        public IOperationResult SendUsers(string content)
        {
            throw new NotImplementedException();
        }
    }
}
