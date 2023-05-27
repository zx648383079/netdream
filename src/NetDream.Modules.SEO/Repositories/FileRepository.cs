using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.SEO.Repositories
{
    public class FileRepository
    {
        private readonly IDatabase _db;

        public FileRepository(IDatabase db)
        {
            _db = db;
        }
    }
}
