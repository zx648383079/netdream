using NetDream.Modules.Plan.Entities;
using NetDream.Modules.Plan.Forms;
using NetDream.Modules.Plan.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Providers;
using System.Linq;

namespace NetDream.Modules.Plan.Repositories
{
    public class CommentRepository(PlanContext db, 
        IClientContext client,
        IUserRepository userStore)
    {
        public IPage<CommentListItem> GetList(QueryForm form, int task_id)
        {
            var model = db.Tasks.Where(i => i.Id == task_id).SingleOrDefault();
            if (model is null)
            {
                return new Page<CommentListItem>();
            }
            if (model.UserId != client.UserId &&
                !ShareRepository.IsShareUser([model.Id, model.ParentId], client.UserId))
            {
                return new Page<CommentListItem>();
                // throw new \Exception("无权限查看评论");
            }
            var taskIds = db.Tasks.Where(i => i.ParentId == task_id)
                .Select(i => i.Id).ToList();
            taskIds.Add(task_id);
            var res = db.Comments.Where(i => taskIds.Contains(i.TaskId))
                .OrderByDescending(i => i.Id).ToPage(form).CopyTo<CommentEntity, CommentListItem>();
            userStore.Include(res.Items);
            return res;
        }

        public IOperationResult<CommentEntity> Create(CommentForm data)
        {
            var task = db.Tasks.Where(i => i.Id == data.TaskId).SingleOrDefault();
            if (task is null)
            {
                return OperationResult<CommentEntity>.Fail("任务错误");
            }
            if (task.UserId != client.UserId &&
                !ShareRepository.IsShareUser([task.Id, task.ParentId], client.UserId))
            {
                return OperationResult<CommentEntity>.Fail("无权限评论");
            }
            var log_id = 0;
            if (task.UserId == client.UserId)
            {
                var time = client.Now - 3600;
                log_id = db.Logs.Where(i => (i.TaskId == task.Id || i.ChildId == task.Id)
                 && i.CreatedAt >= time).OrderByDescending(i => i.CreatedAt)
                 .Select(i => i.Id).FirstOrDefault();
            }
            var comment = new CommentEntity()
            {
                UserId = client.UserId,
                TaskId = task.Id,
                LogId = log_id,
                Content = data.Content,
                Type = data.Type,
                Status = 0
            };
            db.Comments.Save(comment, client.Now);
            db.SaveChanges();
            return OperationResult.Ok(comment);
        }



        public IOperationResult Remove(int id)
        {
            var model = db.Comments.Where(i => i.Id == id).SingleOrDefault();
            if (model is null)
            {
                return OperationResult.Fail("无权限操作");
            }
            if (model.UserId == client.UserId)
            {
                db.Comments.Remove(model);
                db.SaveChanges();
                return OperationResult.Ok();
            }
            var exist = db.Tasks.Where(i => i.Id == model.TaskId && i.UserId == client.UserId)
                .Any();
            if (!exist)
            {
                return OperationResult.Fail("无权限操作");
            }
            db.Comments.Remove(model);
            db.SaveChanges();
            return OperationResult.Ok();
        }
    }
}
