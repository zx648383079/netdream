using NetDream.Modules.UserAccount.Entities;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Web;

namespace NetDream.Modules.UserAccount.Repositories
{
    public class SystemBulletinRepository(UserContext db, 
        ILinkRuler ruler,
        IClientContext client) : ISystemBulletin
    {

        public ILinkRuler Ruler => ruler;

        public int SendAt(int[] user, string title, string link)
        {
            return SendLink(user, title, link, BulletinRepository.TYPE_AT);
        }

        public int SendLink(int[] user, string title, string link, byte type = BulletinRepository.TYPE_AT)
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
                HttpUtility.HtmlEncode(content), type, client.UserId, extraRule);
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
                CreatedAt = client.Now,
                Items = user.Select(i => {
                    return new BulletinUserEntity()
                    {
                        UserId = i,
                        CreatedAt = client.Now,
                    };
                }).ToArray()
            };
            db.Bulletins.Add(bulletin);
            db.SaveChanges();
            return bulletin.Id;
        }

    }
}
