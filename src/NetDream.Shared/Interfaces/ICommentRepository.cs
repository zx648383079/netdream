using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using NetDream.Shared.Repositories;

namespace NetDream.Shared.Interfaces
{
    public interface ICommentRepository
    {

        public IPage<ICommentItem> Search(ModuleTargetType type, int article, QueryForm form);

        public IOperationResult Create(int user, ModuleTargetType type, int article, string content, int parent = 0);
        public IOperationResult Reply(int user, ModuleTargetType type, int article, int parent, string content);
        public IOperationResult Remove(int user, ModuleTargetType type, int article, int comment);
    }

    public interface ICommentItem: ICreatedEntity
    {
        public string Content { get; set; }

        public IUser? User { get; set; }
    }
}
