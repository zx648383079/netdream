using NetDream.Modules.Contact.Entities;
using NetDream.Modules.Contact.Forms;
using NetDream.Shared.Extensions;
using NetDream.Shared.Interfaces;
using NPoco;
using System.Collections.Generic;

namespace NetDream.Modules.Contact.Repositories
{
    public class ContactRepository(IDatabase db, 
        IClientContext environment,
        ISystemBulletin bulletin)
    {
        public List<FriendLinkEntity> FriendLinks()
        {
            return db.Fetch<FriendLinkEntity>("WHERE status = 1");
        }

        public FeedbackEntity SaveFeedback(FeedbackForm data)
        {
            var model = new FeedbackEntity
            {
                Content = data.Content,
                Email = data.Email,
                Phone = data.Phone,
                UserId = environment.UserId
            };
            db.TrySave(model);
            return model;
        }

        public SubscribeEntity SaveSubscribe(SubscribeForm data)
        {
            var model = db.Single<SubscribeEntity>("WHERE email=@0", data.Email);
            model ??= new SubscribeEntity();
            if (!string.IsNullOrWhiteSpace(data.Name) && model.Name != data.Name)
            {
                model.Name = data.Name;
                model.Status = 0;
            }
            model.Email = data.Email;
            db.TrySave(model);
            return model;
        }

        public bool Unsubscribe(string email)
        {
            db.Update<SubscribeEntity>("SET status=1, updated_at=@0 WHERE email=@1", environment.Now, email);
            return true;
        }

        public FriendLinkEntity ApplyFriendLink(FriendLinkForm data)
        {
            var model = new FriendLinkEntity()
            {
                Name = data.Name,
                Brief = data.Brief,
                Url = data.Url,
                Logo = data.Logo,
                Email = data.Email,
                UserId= environment.UserId
            };
            db.TrySave(model);
            bulletin.SendAdministrator("友情链接申请", "[马上查看]", 98, [
                bulletin.Ruler.FormatLink("[马上查看]", "b/friend_link")
            ]);
            return model;
        }
    }
}
