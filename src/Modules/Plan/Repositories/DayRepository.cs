using NetDream.Modules.Plan.Entities;
using NetDream.Modules.Plan.Forms;
using NetDream.Modules.Plan.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System;
using System.Linq;

namespace NetDream.Modules.Plan.Repositories
{
    public class DayRepository(PlanContext db, IClientContext client)
    {
        public IPage<DayListItem> GetList(string day, QueryForm form)
        {
            var dayFormat = DateOnly.Parse(day);
            var res = db.Days.Where(i => i.UserId == client.UserId && i.Amount > 0
            && i.Today == dayFormat)
                .OrderByDescending(i => i.Status)
                .OrderBy(i => i.Id).ToPage<DayEntity, DayListItem>(form);
            TaskRepository.Include(db, res.Items);
            return res;
        }

        public IOperationResult<DayModel> Detail(int id)
        {
            var day = db.Days.Where(i => i.Id == id && i.UserId == client.UserId)
                .SingleOrDefault();
            if (day is null)
            {
                return OperationResult.Fail<DayModel>("任务不存在");
            }
            var res = day.CopyTo<DayModel>();
            var task = db.Tasks.Where(i => i.Id == day.TaskId).SingleOrDefault()
                .CopyTo<TaskModel>();
            task.Children = db.Tasks.Where(i => i.ParentId == day.TaskId).ToArray();
            res.Task = task;
            return OperationResult.Ok(res);
        }

        public IOperationResult<DayEntity> Save(DayForm form)
        {
            if (form.Id > 0)
            {
                var day = db.Days.Where(i => i.Id == form.Id && i.UserId == client.UserId)
                .SingleOrDefault();
                if (day is null)
                {
                    return OperationResult<DayEntity>.Fail("错误");
                }
                day.Amount = (byte)form.Amount;
                db.Days.Save(day, client.Now);
                db.SaveChanges();
                return OperationResult.Ok(day);
            }
            var task = db.Tasks.Where(i => i.Id == form.TaskId && i.UserId == client.UserId).SingleOrDefault();
            if (task is null)
            {
                return OperationResult<DayEntity>.Fail("任务不存在");
            }
            return Add(task, (byte)form.Amount);
        }

        public IOperationResult<DayEntity> Add(TaskEntity task,
            byte amount = 1)
        {
            var day = DateOnly.FromDateTime(DateTime.Now);
            var model = db.Days.Where(i => i.TaskId == task.Id && i.Today == day)
                .FirstOrDefault();
            if (model is not null)
            {
                model.Amount += amount;
                db.Days.Save(model, client.Now);
                db.SaveChanges();
                return OperationResult.Ok(model);
            }
            model = new DayEntity()
            {
                UserId = task.UserId,
                TaskId = task.Id,
                Today = day,
                Amount = amount,
                Status = TaskRepository.STATUS_NONE
            };
            db.Days.Save(model, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(model);
        }

        public IOperationResult Remove(int id)
        {
            var day = db.Days.Where(i => i.Id == id && i.UserId == client.UserId)
                .SingleOrDefault();
            if (day is null)
            {
                return OperationResult.Fail("任务不存在");
            }
            new TaskRepository(db, client).Stop(id);
            db.Days.Remove(day);
            db.SaveChanges();
            return OperationResult.Ok();
        }

        public IOperationResult BatchAdd(int[] items)
        {
            var data = db.Tasks.Where(i => items.Contains(i.Id) && i.UserId == client.UserId)
                .ToArray();
            if (data.Length == 0)
            {
                return OperationResult.Fail("请选择任务");
            }
            foreach (var item in data)
            {
                Add(item, 1);
            }
            return OperationResult.Ok();
        }
    }
}
