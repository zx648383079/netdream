using MediatR;
using NetDream.Shared.Helpers;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;

namespace NetDream.Shared.Notifications
{
    public record BulletinRequest(int[] Users, string Title, string Content, LinkExtraRule[] ExtraRule, BulletinType Type, int Sender, int SendAt) : INotification
    {

        public static BulletinRequest Create(IClientContext client, int[] users, 
            string title, string content, BulletinType type = BulletinType.Other)
        {
            return new BulletinRequest(users, title, content, [], type, client.UserId, client.Now);
        }

        public static BulletinRequest Create(IClientContext client, int[] users,
            string title, string content, LinkExtraRule[] extraRule, BulletinType type = BulletinType.Other)
        {
            return new BulletinRequest(users, title, content, extraRule, type, client.UserId, client.Now);
        }
        public static BulletinRequest Create(int[] users,
            string title, string content, BulletinType type = BulletinType.Other)
        {
            return new BulletinRequest(users, title, content, [], type, 0, TimeHelper.TimestampNow());
        }

        public static BulletinRequest Create(int[] users,
           string title, string content, LinkExtraRule[] extraRule, BulletinType type = BulletinType.Other)
        {
            return new BulletinRequest(users, title, content, extraRule, type, 0, TimeHelper.TimestampNow());
        }

        public static BulletinRequest Create(string title, string content, LinkExtraRule[] extraRule, BulletinType type = BulletinType.Other)
        {
            return new BulletinRequest([], title, content, extraRule, type, 0, TimeHelper.TimestampNow());
        }

        public static BulletinRequest CreateAt(IClientContext client, ILinkRuler ruler, 
            int[] users, string title, string link)
        {
            return CreateLink(client, ruler, users, title, link, BulletinType.At);
        }
        public static BulletinRequest CreateLink(IClientContext client, ILinkRuler ruler,
            int[] users, string title, string link, BulletinType type = BulletinType.At)
        {
            var tag = "[查看]";
            return Create(client, users, 
                title, tag, [
                    ruler.FormatLink(tag, link)
                ], type);
        }
    }


}
