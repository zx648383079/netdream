using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Shop.Repositories
{
    public class ActivityRepository
    {
        public const int TYPE_AUCTION = 1; // 拍卖
        public const int TYPE_SEC_KILL = 2; // 秒杀
        public const int TYPE_GROUP_BUY = 3; // 团购
        public const int TYPE_DISCOUNT = 4; // 优惠
        public const int TYPE_MIX = 5; // 组合
        public const int TYPE_CASH_BACK = 6; // 返现
        public const int TYPE_PRE_SALE = 7; // 预售
        public const int TYPE_BARGAIN = 8; // 砍价
        public const int TYPE_LOTTERY = 9; // 抽奖
        public const int TYPE_FREE_TRIAL = 10; // 试用
        public const int TYPE_WHOLESALE = 11; // 批发

        public const int SCOPE_ALL = 0;
        public const int SCOPE_GOODS = 3;
        public const int SCOPE_BRAND = 2;
        public const int SCOPE_CATEGORY = 1;

        public const int STATUS_NONE = 0;
        public const int STATUS_END = 1;
        public const int STATUS_INVALID = 2;// 流拍

        public const int AUCTION_STATUS_NONE = 0;
        public const int AUCTION_STATUS_SUCCESS = 1;
        public const int AUCTION_STATUS_INVALID = 2; // 无效


        public const int AUCTION_MODE_COMMON = 0;
        public const int AUCTION_MODE_DUTCH = 1;
    }
}
