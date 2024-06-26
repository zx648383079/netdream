using NetDream.Modules.Auth.Entities;
using NetDream.Shared.Extensions;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Migrations;
using NetDream.Shared.Models;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace NetDream.Modules.Auth.Repositories
{
    public class BulletinRepository(IDatabase db, 
        LinkRule ruler,
        IClientEnvironment environment) : ISystemBulletin
    {
        public const byte TYPE_AT = 7;
        public const byte TYPE_COMMENT = 8;
        public const byte TYPE_AGREE = 6;
        public const byte TYPE_MESSAGE = 96;
        public const byte TYPE_OTHER = 99;
        public LinkRule Ruler => ruler;

        public int SendAt(int[] user, string title, string link)
        {
            return SendLink(user, title, link, TYPE_AT);
        }

        public int SendLink(int[] user, string title, string link, byte type = TYPE_AT)
        {
            var tag = "[查看]";
            return Message(user, title, tag, type, [
                Ruler.FormatLink(tag, link)
            ]);
        }

        /// <summary>
        /// SEND MESSAGE TO ANY USERS
        /// </summary>
        /// <param name="user"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="type"></param>
        /// <param name="extraRule"></param>
        /// <returns></returns>
        public int Message(int[] user, string title, string content, byte type = 0, 
            IEnumerable<LinkExtraRule>? extraRule = null)
        {
            return Send(user, 
                HttpUtility.HtmlEncode(title),
                HttpUtility.HtmlEncode(content), type, environment.UserId, extraRule);
        }

        /// <summary>
        /// 发送系统消息
        /// </summary>
        /// <param name="user"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="type"></param>
        /// <param name="extraRule"></param>
        /// <returns></returns>
        public int System(int[] user, string title, string content, 
            byte type = 99, IEnumerable<LinkExtraRule>? extraRule = null)
        {
            return Send(user, title, content, type, 0, extraRule);
        }
        /// <summary>
        /// 发送给管理员
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="type"></param>
        /// <param name="extraRule"></param>
        public int SendAdministrator(string title, string content, byte type = 99, IEnumerable<LinkExtraRule>? extraRule = null)
        {
            return Send([1], title, content, type, 0, extraRule);
        }

        public int Send(int[] user, string title, string content, byte type = 0, int sender = 0, IEnumerable<LinkExtraRule>? extraRule = null)
        {
            if (user.Length == 0)
            {
                return 0;
            }
            var bulletin = new BulletinEntity()
            {
                Title = title,
                Content = content,
                Type = type,
                UserId = sender,
                ExtraRule = extraRule is not null && extraRule.Any() ? JsonSerializer.Serialize(extraRule) 
                : string.Empty,
                CreatedAt = TimeHelper.TimestampNow()
            };
            if (!db.TrySave(bulletin))
            {
                return 0;
            }
            return db.InsertBatch(user.Select(i => {
                return new BulletinUserEntity()
                {
                    BulletinId = bulletin.Id,
                    UserId = i,
                    CreatedAt = bulletin.CreatedAt
                };
            }));
        }
    }
}
