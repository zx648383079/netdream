using NetDream.Shared.Interfaces.Entities;
using NetDream.Shared.Models;
using System;

namespace NetDream.Shared.Interfaces
{
    public interface ICommentRepository
    {
        public int Count(ModuleTargetType type);

        public int Count(ModuleTargetType type, DateTime startAt);
        public int Count(ModuleTargetType type, DateTime startAt, DateTime endAt);

        public IPage<ICommentItem> Search(ModuleTargetType type, int article, QueryForm form);
        public ICommentItem[] Get(ModuleTargetType type, int article, QueryForm form);
        public ICommentItem[] Get(ModuleTargetType type, QueryForm form);

        public IOperationResult Create(int user, ModuleTargetType type, int article, string content, int parent = 0);
        public IOperationResult Create(int user, ModuleTargetType type, int article, ICommentForm form);
        public IOperationResult Reply(int user, ModuleTargetType type, int article, int parent, string content);
        public IOperationResult Remove(int user, ModuleTargetType type, int comment);
        public IOperationResult<IUser> LastCommentator(string email);
        public IOperationResult<AgreeResult> Toggle(int user, ModuleTargetType type, int comment, bool agree);
        public IOperationResult Report(int user, ModuleTargetType type, int comment);
    }

    public interface ICommentForm
    {
        public string Content { get; }
    }

    public interface ICommentItem: ICreatedEntity
    {
        public string Content { get; set; }

        public IUser? User { get; set; }
    }
}
