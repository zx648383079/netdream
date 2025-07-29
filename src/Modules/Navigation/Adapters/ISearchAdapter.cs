using NetDream.Modules.Navigation.Forms;
using NetDream.Modules.Navigation.Models;
using NetDream.Shared.Interfaces;
using NetDream.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Navigation.Adapters
{
    public interface ISearchAdapter
    {
        public IPage<PageListItem> Search(QueryForm form);

        public IOperationResult<PageModel> Get(int id);

        public IOperationResult<PageModel> Create(PageForm data);

        public IOperationResult<PageModel> Update(int id, PageForm data);

        public IOperationResult Remove(int id);

        public string[] Suggest(string keywords);
    }
}
