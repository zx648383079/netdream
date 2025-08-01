using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Contact.Entities;
using NetDream.Modules.Contact.Forms;
using NetDream.Modules.Contact.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Contact.Repositories
{
    public class ContactRepository(ContactContext db, 
        IClientContext client,
        ISystemBulletin bulletin)
    {
        public FriendLink[] FriendLinks()
        {
            return db.FriendLinks.Where(i => i.Status == 1).Select(i => new FriendLink(i)).ToArray();
        }

        public IOperationResult<FeedbackEntity> SaveFeedback(FeedbackForm data)
        {
            var model = new FeedbackEntity
            {
                Content = data.Content,
                Email = data.Email,
                Phone = data.Phone,
                UserId = client.UserId,
                CreatedAt = client.Now,
                UpdatedAt = client.Now,
            };
            db.Feedbacks.Add(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult<SubscribeEntity> SaveSubscribe(SubscribeForm data)
        {
            var model = db.Subscribes.Where(i => i.Email == data.Email).Single();
            model ??= new SubscribeEntity();
            if (!string.IsNullOrWhiteSpace(data.Name) && model.Name != data.Name)
            {
                model.Name = data.Name;
                model.Status = 0;
            }
            model.Email = data.Email;
            db.Subscribes.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult Unsubscribe(string email)
        {
            db.Subscribes.Where(i => i.Email == email)
                .ExecuteUpdate(setters => setters.SetProperty(i => i.Status, 1).SetProperty(i => i.UpdatedAt, client.Now));
            return OperationResult.Ok();
        }

        public IOperationResult<FriendLinkEntity> ApplyFriendLink(FriendLinkForm data)
        {
            var model = new FriendLinkEntity()
            {
                Name = data.Name,
                Brief = data.Brief,
                Url = data.Url,
                Logo = data.Logo,
                Email = data.Email,
                UserId= client.UserId
            };
            db.FriendLinks.Save(model, client.Now);
            db.SaveChanges();
            bulletin.SendAdministrator("友情链接申请", "[马上查看]", 98, [
                bulletin.Ruler.FormatLink("[马上查看]", "b/friend_link")
            ]);
            return OperationResult.Ok(model);
        }
    }
}
