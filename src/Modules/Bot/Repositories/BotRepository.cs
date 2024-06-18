using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Bot.Repositories
{
    public class BotRepository
    {
        public const int PLATFORM_TYPE_WX = 0;
        public const int PLATFORM_TYPE_TELEGRAM = 0;

        /**
         * 未激活状态
         */
        public const int STATUS_INACTIVE = 4;
        /**
         * 激活状态
         */
        public const int STATUS_ACTIVE = 5;
        /**
         * 删除状态
         */
        public const int STATUS_DELETED = 0;
        /**
         * 普通订阅号
         */
        public const int TYPE_SUBSCRIBE = 0;
        /**
         * 认证订阅号
         */
        public const int TYPE_SUBSCRIBE_VERIFY = 1;
        /**
         * 普通服务号
         */
        public const int TYPE_SERVICE = 2;
        /**
         * 认证服务号
         */
        public const int TYPE_SERVICE_VERIFY = 3;

        /**
         * 取消关注
         */
        public const int STATUS_UNSUBSCRIBED = 0;
        /**
         * 关注状态
         */
        public const int STATUS_SUBSCRIBED = 1;
    }
}
