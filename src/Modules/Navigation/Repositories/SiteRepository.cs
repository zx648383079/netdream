using NetDream.Shared.Providers;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Navigation.Repositories
{
    public class SiteRepository(IDatabase db)
    {
        const string BASE_KEY = "search";

        public TagProvider Tag()
        {
            return new TagProvider(db, BASE_KEY);
        }
    }
}
