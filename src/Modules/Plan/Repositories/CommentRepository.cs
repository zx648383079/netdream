using NetDream.Modules.Plan.Forms;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
using System.Linq;

namespace NetDream.Modules.Plan.Repositories
{
    public class CommentRepository(PlanContext db, 
        IClientContext client,
        ICommentRepository comment)
    {
        public IPage<ICommentItem> GetList(CommentQueryForm form)
        {
            return comment.Search(ModuleTargetType.Plan, form.Task, form);
        }

        public IOperationResult Create(CommentForm data)
        {
            var task = db.Tasks.Where(i => i.Id == data.TaskId).SingleOrDefault();
            if (task is null)
            {
                return OperationResult.Fail("任务错误");
            }
            if (task.UserId != client.UserId &&
                !ShareRepository.IsShareUser(db, [task.Id, task.ParentId], client.UserId))
            {
                return OperationResult.Fail("无权限评论");
            }
            var log_id = 0;
            if (task.UserId == client.UserId)
            {
                var time = client.Now - 3600;
                log_id = db.Logs.Where(i => (i.TaskId == task.Id || i.ChildId == task.Id)
                 && i.CreatedAt >= time).OrderByDescending(i => i.CreatedAt)
                 .Select(i => i.Id).FirstOrDefault();
            }
            return comment.Create(client.UserId, ModuleTargetType.Plan, task.Id, data.Content);
        }



        public IOperationResult Remove(int id)
        {
            var res = comment.GetSource(ModuleTargetType.Plan, id);
            if (!res.Succeeded)
            {
                return res;
            }
            if (res.Result.User == client.UserId)
            {
                return comment.Remove(client.UserId, ModuleTargetType.Plan, id);
            }
            var exist = db.Tasks.Where(i => i.Id == res.Result.Article && i.UserId == client.UserId)
                .Any();
            if (!exist)
            {
                return OperationResult.Fail("无权限操作");
            }
            return comment.Remove(client.UserId, ModuleTargetType.Plan, id);
        }
    }
}
