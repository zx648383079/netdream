using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.UserAccount.Repositories;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetDream.Modules.Auth.Repositories
{
    public class BanRepository(
        AuthContext db, 
        IClientContext client,
        IUserRepository userStore)
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
            var user = userStore.GetProfile(userId);
            if (user is null)
            {
                return;
            }
            Ban(user.Email, AuthRepository.ACCOUNT_TYPE_EMAIL);
            Ban(user.Mobile, AuthRepository.ACCOUNT_TYPE_MOBILE);
            var items = db.OAuth.Where(i => i.UserId == userId).ToArray();
            foreach (var item in items)
            {
                var type = OAUTH_TYPE_MAPS[item.Vendor];
                Ban(item.Identity, type, item.PlatformId);
                Ban(item.Unionid, type, item.PlatformId);
            }
            db.Users.Where(i => i.Id == userId && i.Status >= UserRepository.STATUS_ACTIVE)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, UserRepository.STATUS_FROZEN));
        
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
            db.BanAccounts.Add(new BanAccountEntity()
            {
                UserId = client.UserId,
                ItemKey = itemKey,
                ItemType = itemType,
                PlatformId = platformId,
                CreatedAt = client.Now
            });
            db.SaveChanges();
        }

        public bool IsBan(string itemKey, int itemType = -1, int platformId = 0)
        {
            if (itemType < 0)
            {
                return db.BanAccounts.Where(i => i.ItemKey == itemKey).Any();
            }
            return db.BanAccounts.Where(i => i.ItemKey == itemKey && i.ItemType == itemType && i.PlatformId == platformId).Any();
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
                db.BanAccounts.Where(i => i.ItemKey == itemKey).ExecuteDelete();
                return;
            }
            db.BanAccounts.Where(i => i.ItemKey == itemKey && i.ItemType == itemType && i.PlatformId == platformId)
                .ExecuteDelete();
        }

        public IPage<BanAccountEntity> GetList(string keywords, int page = 1)
        {
            return db.BanAccounts.Search(keywords, "item_key")
                .ToPage(page);
        }

        public void Remove(int id)
        {
            db.BanAccounts.Where(i => i.Id == id).ExecuteDelete();
        }

        public void RemoveUser(int userId)
        {
            var user = userStore.GetProfile(userId);
            if (user is null)
            {
                return;
            }
            Unban(user.Email, AuthRepository.ACCOUNT_TYPE_EMAIL);
            Unban(user.Mobile, AuthRepository.ACCOUNT_TYPE_MOBILE);
            var items = db.OAuth.Where(i => i.UserId == userId).ToArray();
            foreach (var item in items)
            {
                var type = OAUTH_TYPE_MAPS[item.Vendor];
                Unban(item.Identity, type, item.PlatformId);
                Unban(item.Unionid, type, item.PlatformId);
            }
            db.Users.Where(i => i.Id == userId && i.Status < UserRepository.STATUS_ACTIVE)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, UserRepository.STATUS_ACTIVE));
   
        }
    }
}
