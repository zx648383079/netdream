using NetDream.Modules.Book.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetDream.Modules.Book.Models
{
    public interface IWithBookModel
    {
        public int BookId { get; }

        public BookEntity? Book { set; }
    }
}
