using NetDream.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetDream.Shared.Interfaces
{
    public interface ISearcher
    {

        public void Index(string id, object data);
        public void Search(PaginationForm form);

    }
}
