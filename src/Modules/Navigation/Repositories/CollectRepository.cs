using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Navigation.Entities;
using NetDream.Modules.Navigation.Forms;
using NetDream.Modules.Navigation.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Navigation.Repositories
{
    public class CollectRepository(NavigationContext db, IClientContext client)
    {

        public CollectGroupModel[] All()
        {
            var items = db.CollectGroups.Where(i => i.UserId == client.UserId)
                .OrderBy(i => i.Position)
                .ThenBy(i => i.Id)
                .ToArray();
            if (items.Length == 0)
            {
                return [];
            }
            var data = db.Collects.Where(i => i.UserId == client.UserId)
                .OrderBy(i => i.GroupId)
                .OrderBy(i => i.Position)
                .ThenBy(i => i.Id).ToArray();
            return items.Select(i => new CollectGroupModel(i)
            {
                Items = data.Where(j => j.GroupId == i.Id).Select(j => new CollectModel(j)).ToArray()
            }).ToArray();
        }

        public IOperationResult<CollectEntity> Save(CollectForm data)
        {
            var model = data.Id > 0 ? db.Collects.Where(i => i.Id == data.Id && i.UserId == client.UserId)
                .SingleOrDefault() :
                new CollectEntity();
            if (model is null)
            {
                return OperationResult.Fail<CollectEntity>("收藏不存在");
            }
            if (data.GroupId > 0)
            {
                model.GroupId = data.GroupId;
            }
            model.Name = data.Name;
            model.Link = data.Link;
            model.Position = data.Position;
            if (!Check(model))
            {
                return OperationResult.Fail<CollectEntity>("网址已存在");
            }
            model.UserId = client.UserId;
            if (model.GroupId <= 0)
            {
                model.GroupId = GetDefaultGroup(model.UserId);
            }
            db.Collects.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        private bool Check(CollectEntity model)
        {
            return !db.Collects.Where(i => i.Link == model.Link && i.UserId == client.UserId && i.Id != model.Id)
                .Any();
        }

        public int GetDefaultGroup(int user)
        {
            var id = db.CollectGroups.Where(i => i.Id == user)
                .Value(i => i.Id);
            if (id > 0)
            {
                return id;
            }
            var model = new CollectGroupEntity()
            {
                Name = "Default",
                UserId = user
            };
            db.CollectGroups.Add(model);
            db.SaveChanges();
            return model.Id;
        }

        public void Remove(int id)
        {
            db.Collects.Where(i => i.Id == id && i.UserId == client.UserId)
                .ExecuteDelete();
            db.SaveChanges();
        }

        public void Clear()
        {
            db.Collects.Where(i => i.UserId == client.UserId).ExecuteDelete();
            db.CollectGroups.Where(i => i.UserId == client.UserId).ExecuteDelete();
            db.SaveChanges();
        }

        public CollectGroupModel[] Reset()
        {
            return [];
        }


        public IOperationResult<CollectGroupEntity> GroupSave(CollectGroupForm data)
        {
            var model = data.Id > 0 ? db.CollectGroups.Where(i => i.Id == data.Id && i.UserId == client.UserId)
                .SingleOrDefault() :
                new CollectGroupEntity();
            if (model is null)
            {
                return OperationResult.Fail<CollectGroupEntity>("分组不存在");
            }
            model.Name = data.Name;
            model.Position = data.Position;
            model.UserId = client.UserId;
            db.CollectGroups.Save(model);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public void GroupRemove(int id)
        {
            db.CollectGroups.Where(i => i.Id == id && i.UserId == client.UserId).ExecuteDelete();
            db.Collects.Where(i => i.GroupId == id && i.UserId == client.UserId).ExecuteDelete();
            db.SaveChanges();
        }

        public bool IsCollected(string link)
        {
            return db.Collects.Where(i => i.Link == link && i.UserId == client.UserId).Any();
        }

        public IOperationResult<CollectGroupModel[]> BatchSave(CollectBatchForm data)
        {
            var userId = client.UserId;
            var groupItems = db.CollectGroups.Where(i => i.UserId == userId).Pluck(i => i.Id);
            var items = db.Collects.Where(i => i.UserId == userId).Pluck(i => i.Id);
            var groupIndex = -1;
            var groupMax = groupItems.Length - 1;
            var itemMax = items.Length - 1;
            var itemIndex = -1;
            var now = client.Now;
            foreach (var group in data.Data)
            {
                groupIndex++;
                var groupId = 0;
                var position = (byte)(groupIndex + 1);
                if (groupIndex > groupMax)
                {
                    var model = new CollectGroupEntity()
                    {
                        Name = group.Name,
                        UserId = userId,
                        Position = position
                    };
                    db.CollectGroups.Add(model);
                    db.SaveChanges();
                    groupId = model.Id;
                }
                else
                {
                    groupId = groupItems[groupIndex];
                    db.CollectGroups.Where(i => i.Id == groupId && i.UserId == userId)
                        .ExecuteUpdate(setters => setters.SetProperty(i => i.Name, group.Name)
                        .SetProperty(i => i.Position, position));
                }
                if (groupId <= 0)
                {
                    return OperationResult.Fail<CollectGroupModel[]>("保存失败");
                }
                foreach (var item in group.Items)
                {
                    itemIndex++;
                    position = (byte)(itemIndex + 1);
                    if (itemIndex > itemMax)
                    {
                        db.Collects.Add(new CollectEntity()
                        {
                            Name = item.Name,
                            Link = item.Link,
                            UserId = userId,
                            GroupId = groupId,
                            Position = position,
                            UpdatedAt = now,
                            CreatedAt = now
                        });
                    }
                    else
                    {
                        db.Collects.Where(i => i.Id == items[itemIndex] && i.UserId == client.UserId)
                            .ExecuteUpdate(setters => setters.SetProperty(i => i.Name, item.Name)
                            .SetProperty(i => i.GroupId, groupId)
                            .SetProperty(i => i.Link, item.Link)
                            .SetProperty(i => i.Position, position)
                            .SetProperty(i => i.UpdatedAt, now));
                    }
                }
            }
            if (groupIndex < groupMax)
            {
                var del = groupItems[(groupIndex + 1)..];
                db.CollectGroups.Where(i => i.UserId == userId && del.Contains(i.Id)).ExecuteDelete();
            }
            if (itemIndex < itemMax)
            {
                var del = items[(itemIndex + 1)..];
                db.Collects.Where(i => i.UserId == userId && del.Contains(i.Id)).ExecuteDelete();
            }
            return OperationResult.Ok(All());
        }
    }
}
