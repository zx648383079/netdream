using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Book.Repositories
{
    public class BookRepository
    {
        private readonly IDatabase _db;

        public BookRepository(IDatabase db)
        {
            _db = db;
        }
    }
}
