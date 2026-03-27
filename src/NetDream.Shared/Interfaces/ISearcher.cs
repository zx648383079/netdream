using NetDream.Shared.Models;
using NetDream.Shared.Repositories;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface ISearcher
    {

        public void Index(ModuleTargetType type, int id, string[] words);
        public void Search(PaginationForm form);

        /// <summary>
        /// 分词
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public IEnumerable<string> Cut(string text);
    }
}
