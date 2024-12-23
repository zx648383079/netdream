using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Contact.Entities;
using NetDream.Modules.Contact.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Contact.Repositories
{
    public class ContactRepository(ContactContext db, 
        IClientContext environment,
        ISystemBulletin bulletin)
    {
        public FriendLinkEntity[] FriendLinks()
        {
            return db.FriendLinks.Where(i => i.Status == 1).ToArray();
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
            db.Feedbacks.Add(model);
            db.SaveChanges();
            return model;
        }

        public SubscribeEntity SaveSubscribe(SubscribeForm data)
        {
            var model = db.Subscribes.Where(i => i.Email == data.Email).Single();
            model ??= new SubscribeEntity();
            if (!string.IsNullOrWhiteSpace(data.Name) && model.Name != data.Name)
            {
                model.Name = data.Name;
                model.Status = 0;
            }
            model.Email = data.Email;
            db.Subscribes.Save(model);
            db.SaveChanges();
            return model;
        }

        public bool Unsubscribe(string email)
        {
            db.Subscribes.Where(i => i.Email == email)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, 1).SetProperty(i => i.UpdatedAt, environment.Now));
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
            db.FriendLinks.Save(model);
            db.SaveChanges();
            bulletin.SendAdministrator("友情链接申请", "[马上查看]", 98, [
                bulletin.Ruler.FormatLink("[马上查看]", "b/friend_link")
            ]);
            return model;
        }
    }
}
