using NetDream.Shared.Models;
using System;

namespace NetDream.Shared.Interfaces
{
    public interface ICommentRepository
    {
        public int Count(ModuleTargetType type);
        public int Count(int user, ModuleTargetType type);

        public int Count(ModuleTargetType type, DateTime startAt);
        public int Count(ModuleTargetType type, DateTime startAt, DateTime endAt);

        public IPage<ICommentItem> Search(ModuleTargetType type, int article, IQueryForm form);
        public ICommentItem[] Get(ModuleTargetType type, int article, IQueryForm form);
        public ICommentItem[] Get(ModuleTargetType type, IQueryForm form);
        /// <summary>
        /// 获取评论的发布来源
        /// </summary>
        /// <param name="type"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public IOperationResult<ICommentSource> GetSource(ModuleTargetType type, int comment);

        public IOperationResult Create(int user, ModuleTargetType type, int article, string content, int parent = 0);
        public IOperationResult Create(int user, ModuleTargetType type, int article, string content, LinkExtraRule[] rules, int parent = 0);
        public IOperationResult Create(int user, ModuleTargetType type, int article, ICommentForm form);
        public IOperationResult Reply(int user, ModuleTargetType type, int article, int parent, string content);
        public IOperationResult Remove(int user, ModuleTargetType type, int comment);
        public IOperationResult RemoveArticle(int user, ModuleTargetType type, int article);
        public IOperationResult<IUser> LastCommentator(string email);
        public IOperationResult<AgreeResult> Toggle(int user, ModuleTargetType type, int comment, bool agree);
        public IOperationResult Report(int user, ModuleTargetType type, int comment);
        /// <summary>
        /// 是否评过分
        /// </summary>
        /// <param name="user"></param>
        /// <param name="type"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        public bool Has(int user, ModuleTargetType type, int article);
        public IScoreSubtotal Score(ModuleTargetType type, int article);
        /// <summary>
        /// 平均分
        /// </summary>
        /// <param name="type"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        public float Avg(ModuleTargetType type, int article);
        /// <summary>
        /// 评分
        /// </summary>
        /// <param name="user"></param>
        /// <param name="type"></param>
        /// <param name="article"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public IOperationResult Scoring(int user, ModuleTargetType type, int article, byte score);
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

    public interface IWithArticleModel
    {
        public int ArticleId { get; }

        public IListArticleItem? Article { set; }
    }

    public interface ICommentSource
    {
        public int Id { get; }
        public int User { get; }
        public int Article { get; }
        public ModuleTargetType Type { get; }
    }

    public interface IScoreSubtotal
    {
        public int Total { get; }
        public int Good { get; }
        public int Middle { get; }
        public int Bad { get; }
        public float Avg { get; }

        public float FavorableRate { get; }
    }
}
