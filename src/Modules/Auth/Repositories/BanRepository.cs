using NetDream.Modules.Auth.Entities;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NPoco;
using System;
using System.Collections.Generic;

namespace NetDream.Modules.Auth.Repositories
{
    public class BanRepository(IDatabase db, IClientContext client)
    {
        private readonly Dictionary<string, byte> OAUTH_TYPE_MAPS = new() {
            {AuthRepository.OAUTH_TYPE_QQ, AuthRepository.ACCOUNT_TYPE_OAUTH_QQ },
            {AuthRepository.OAUTH_TYPE_WX, AuthRepository.ACCOUNT_TYPE_OAUTH_WX },
            {AuthRepository.OAUTH_TYPE_WX_MINI, AuthRepository.ACCOUNT_TYPE_OAUTH_WX_MINI },
            {AuthRepository.OAUTH_TYPE_ALIPAY, AuthRepository.ACCOUNT_TYPE_OAUTH_ALIPAY },
            {AuthRepository.OAUTH_TYPE_TAOBAO, AuthRepository.ACCOUNT_TYPE_OAUTH_TAOBAO },
            {AuthRepository.OAUTH_TYPE_WEIBO, AuthRepository.ACCOUNT_TYPE_OAUTH_WEIBO },
            {AuthRepository.OAUTH_TYPE_GITHUB, AuthRepository.ACCOUNT_TYPE_OAUTH_GITHUB }
        };

        public void BanUser(int userId)
        {
            if (userId == client.UserId)
            {
                throw new Exception("不能拉黑自己");
            }
            var user = db.FindFirst<UserEntity>("email,mobile", "id=@0", userId);
            if (user is null)
            {
                return;
            }
            Ban(user.Email, AuthRepository.ACCOUNT_TYPE_EMAIL);
            Ban(user.Mobile, AuthRepository.ACCOUNT_TYPE_MOBILE);
            var items = db.Fetch<OauthEntity>("user_id=@0", userId);
            foreach (var item in items)
            {
                var type = OAUTH_TYPE_MAPS[item.Vendor];
                Ban(item.Identity, type, item.PlatformId);
                Ban(item.Unionid, type, item.PlatformId);
            }
            db.Update<UserEntity>(new Sql().Where("id=@0 and status>=@1", 
                userId, UserRepository.STATUS_ACTIVE), new()
            {
                {"status", UserRepository.STATUS_FROZEN}
            });
        }

        /**
         * 屏蔽
         * @param string itemKey
         * @param int itemType
         * @param int platformId
         * @return void
         * @throws \Exception
         */
        public void Ban(string itemKey, int itemType = 0, int platformId = 0)
        {
            if (string.IsNullOrWhiteSpace(itemKey))
            {
                return;
            }
            if (IsBan(itemKey, itemType, platformId))
            {
                return;
            }
            db.Insert(new BanAccountEntity()
            {
                UserId = client.UserId,
                ItemKey = itemKey,
                ItemType = itemType,
                PlatformId = platformId
            });
        }

        public bool IsBan(string itemKey, int itemType = -1, int platformId = 0)
        {
            if (itemType < 0)
            {
                return db.FindCount<BanAccountEntity>("item_key=@0", itemKey) > 0;
            }
            return db.FindCount<BanAccountEntity>("item_key=@0 and item_type=@1 and platform_id=@2", 
                itemKey, itemType, platformId) > 0;
        }

        public bool IsBanOAuth(string openId, string unionId, string vendor, int platformId)
        {
            var type = OAUTH_TYPE_MAPS[vendor];
            if (!string.IsNullOrWhiteSpace(openId) && IsBan(openId, type, platformId))
            {
                return true;
            }
            if (!string.IsNullOrWhiteSpace(unionId) && IsBan(unionId, type, platformId))
            {
                return true;
            }
            return false;
        }

        /**
         * 取消屏蔽
         * @param string itemKey
         * @param int itemType
         * @param int platformId
         */
        public void Unban(string itemKey, int itemType = -1, int platformId = 0)
        {
            if (itemType < 0)
            {
                db.DeleteWhere<BanAccountEntity>("item_key=@0", itemKey);
                return;
            }
            db.DeleteWhere<BanAccountEntity>("item_key=@0 and item_type=@1 and platform_id=@2",
                itemKey, itemType, platformId);
        }

        public Page<BanAccountEntity> GetList(string keywords, int page = 1)
        {
            var sql = new Sql();
            sql.Select().From<BanAccountEntity>(db);
            SearchHelper.Where(sql, "item_key", keywords);
            return db.Page<BanAccountEntity>(page, 20, sql);
        }

        public void Remove(int id)
        {
            db.DeleteById<BanAccountEntity>(id);
        }

        public void RemoveUser(int userId)
        {
            var user = db.FindFirst<UserEntity>("email,mobile", "id=@0", userId);
            if (user is null)
            {
                return;
            }
            Unban(user.Email, AuthRepository.ACCOUNT_TYPE_EMAIL);
            Unban(user.Mobile, AuthRepository.ACCOUNT_TYPE_MOBILE);
            var items = db.Fetch<OauthEntity>("user_id=@0", userId);
            foreach (var item in items)
            {
                var type = OAUTH_TYPE_MAPS[item.Vendor];
                Unban(item.Identity, type, item.PlatformId);
                Unban(item.Unionid, type, item.PlatformId);
            }
            db.Update<UserEntity>(new Sql().Where("id=@0 and status<@1",
                userId, UserRepository.STATUS_ACTIVE), new()
            {
                {"status", UserRepository.STATUS_ACTIVE}
            });
        }
    }
}
