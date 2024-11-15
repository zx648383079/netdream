using NetDream.Modules.SEO.Entities;
using NetDream.Shared.Repositories;
using NPoco;
using System.Collections.Generic;

namespace NetDream.Modules.SEO.Repositories
{
    public class WordRepository(IDatabase db): CRUDRepository<BlackWordEntity>(db)
    {
        public override IList<string> SearchKeys => ["words", "replace_words"];
    }
}
