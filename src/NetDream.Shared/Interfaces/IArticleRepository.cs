using NetDream.Shared.Models;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface IArticleRepository
    {
        public IPage<IListArticleItem> Search(ModuleTargetType type, IQueryForm form);
        public IListArticleItem[] Get(ModuleTargetType type);

        public IOperationResult<IArticle> Get(ModuleTargetType type, int id);

        public IOperationResult Add(int user, ModuleTargetType type, IArticle item);
        public IOperationResult Update(int user, ModuleTargetType type, IArticle item);
        public void Remove(int user, ModuleTargetType type, int id);

        public void Include(ModuleTargetType type, IEnumerable<IWithArticleModel> items);
    }

    public interface IArticle
    {
        public int Id { get; }

        public string Title { get; }

        public string Description { get; }
        public string Keywords { get; }
        public string Content { get; }

    }
}
